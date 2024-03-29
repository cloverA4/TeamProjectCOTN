using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Data : MonoBehaviour
{
    #region 전역변수
    [SerializeField] GameObject _ItemPrefab;
    [SerializeField] GameObject _bloodEffect;
    
    List<int> _lockEquipItemIDList = new List<int>();
    List<int> _lockPotionList = new List<int>();
    List<int> _lockPassivesList = new List<int>();
    List<Item> ItemDataList = new List<Item>();

    string LINE_SPLIT = @"\r\n|\n\r|\n|\r";
    string SPLIT = ",";

    public int[] UnlockPirceToLevel = new int[3] { 2, 4, 7 };
    public SaveData CharacterSaveData = new SaveData();

    List<AudioClip> _soundEffect = new List<AudioClip>();
    public List<AudioClip> SoundEffect { get { return _soundEffect; } }
    public GameObject ItemPrefab { get { return _ItemPrefab; } }
    public GameObject BloodEffect { get { return _bloodEffect; } }
    public List<int> LockEquipItemIDList { get { return _lockEquipItemIDList; } }
    public List<int> LockPotionList { get { return _lockPotionList; } }
    public List<int> LockPassivesList { get { return _lockPassivesList; } }
    #endregion

    private static Data instance = null;
    private void Awake()
    {
        if (null == instance)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public static Data Instance
    {
        get
        {
            if (null == instance)
            {
                return null;
            }
            return instance;
        }
    }

    private void Start()
    {
        StartCoroutine(LoadGame());
    }

    public void SavePlayerData()
    {
        CharacterSaveData._gold = GameManager.Instance.Gold;
        CharacterSaveData._dia = GameManager.Instance.Dia;
        CharacterSaveData._nowStage = GameManager.Instance.NowStage;
        CharacterSaveData._nowFloor = GameManager.Instance.NowFloor;
        CharacterSaveData._nowHP = PlayerController.Instance.NowHP;

        if (PlayerController.Instance.EquipShovel != null)
        {
            CharacterSaveData._equipShovelID = PlayerController.Instance.EquipShovel._ItemID;
        }
        else
        {
            CharacterSaveData._equipShovelID = 0;
        }

        if (PlayerController.Instance.EquipWeapon != null)
        {
            CharacterSaveData._equipWeaponID = PlayerController.Instance.EquipWeapon._ItemID;
        }
        else
        {
            CharacterSaveData._equipWeaponID = 0;
        }

        if (PlayerController.Instance.EquipArmor != null)
        {
            CharacterSaveData._equipArmorID = PlayerController.Instance.EquipArmor._ItemID;
        }
        else
        {
            CharacterSaveData._equipArmorID = 0;
        }

        if (PlayerController.Instance.EquipPotion != null)
        {
            CharacterSaveData._equipPotionID = PlayerController.Instance.EquipPotion._ItemID;
        }
        else
        {
            CharacterSaveData._equipPotionID = 0;
        }


        string Json = JsonUtility.ToJson(CharacterSaveData);

        string TempPath = Application.persistentDataPath + "/CharacterSaveData.json";

        using (StreamWriter outStream = File.CreateText(TempPath))
        {
            outStream.Write(Json);
        }
    }

    #region GameDataLoad

    public event EventHandler LoadingEnd;
    AsyncOperation asyncOperation;

    public IEnumerator LoadGame()
    {
        ReadItemData(); //아이템 테이블
        LoadSaveData(); //플레이어 저장 정보
        UPdatelockList(); //해금아이템 정보
        LoadSound(); // 사운드 소스
        yield return null;

        // 마지막에 필요한 모든 게임데이터를 로드가되면 gmaeScene로드
        asyncOperation = SceneManager.LoadSceneAsync("GameScene");
        asyncOperation.allowSceneActivation = false;

        while (true)
        {
            if (asyncOperation.progress >= 0.9f)
            {
                yield return new WaitForSeconds(3f);
                LoadingEnd?.Invoke(this, EventArgs.Empty);
                break;
            }
            yield return null;
        }
    }

    public void SceneChange()
    {
        StopCoroutine(LoadGame());
        asyncOperation.allowSceneActivation = true;
    }

    void LoadSaveData()
    {
        //제이슨파일에서 업그레이드 데이터 읽어오기
        if (File.Exists(Application.persistentDataPath + "/CharacterSaveData.json"))
        {
            string json = "";
            using (StreamReader inStream = new StreamReader(Application.persistentDataPath + "/CharacterSaveData.json"))
            {
                json = inStream.ReadToEnd();
            }

            if (string.IsNullOrEmpty(json) == false)
            {
                CharacterSaveData = JsonUtility.FromJson<SaveData>(json);
                
                if(CharacterSaveData._unlockItemId.Count <= 0)
                {
                    BaseUnlockItemAdd();
                }
            }
            else Debug.Log("내용이 없습니다.");
        }
        else
        {
            CharacterSaveData._gold = 0;
            CharacterSaveData._dia = 0;
            CharacterSaveData._nowStage = Stage.Lobby;
            CharacterSaveData._nowFloor = floor.f1;
            CharacterSaveData._nowHP = 0;

            BaseUnlockItemAdd();
        }
    }

    void BaseUnlockItemAdd()
    {
        CharacterSaveData._unlockItemId.Clear();
        CharacterSaveData._unlockItemId.Add(201);
        CharacterSaveData._unlockItemId.Add(301);
        CharacterSaveData._unlockItemId.Add(303);
        CharacterSaveData._unlockItemId.Add(305);
        CharacterSaveData._unlockItemId.Add(401);
        CharacterSaveData._unlockItemId.Add(501);
    }

    public void UPdatelockList() //해금한경우 호출해서 새로만들것
    {
        _lockEquipItemIDList.Clear();
        _lockPotionList.Clear();
        _lockPassivesList.Clear();
        for (int i = 0; i < ItemDataList.Count; i++)
        {
            if (ItemDataList[i]._itemType == ItemType.Currency) continue;
            if (ItemDataList[i]._itemType == ItemType.Unlock) continue;

            bool _isUnlocked = false;
            for(int j = 0; j < CharacterSaveData._unlockItemId.Count; j++)
            {
                if (ItemDataList[i]._ItemID == CharacterSaveData._unlockItemId[j])
                {
                    _isUnlocked = true;
                    break;
                }
            }

            if(_isUnlocked == false)
            {
                if (ItemDataList[i]._itemType == ItemType.Weapon || ItemDataList[i]._itemType == ItemType.Armor)
                {
                    _lockEquipItemIDList.Add(ItemDataList[i]._ItemID);
                }
                else if(ItemDataList[i]._itemType == ItemType.Potion)
                {
                    _lockPotionList.Add(ItemDataList[i]._ItemID);
                }
            }
        }
        UpdateLockPassivesList();
    }

    void UpdateLockPassivesList()
    {
        if (PlayerPrefs.HasKey("PlayerHPUpgradeLevel"))
        {
            UnlockItem ul = (UnlockItem)GetItemInfo(601);
            if (PlayerPrefs.GetInt("PlayerHPUpgradeLevel") < ul.MaxUnlockCount)
            {
                _lockPassivesList.Add(601);
            }
        }
        else
        {
            _lockPassivesList.Add(601);
        }

        if (PlayerPrefs.HasKey("TreasureBoxUpgradeLevel"))
        {
            UnlockItem ul = (UnlockItem)GetItemInfo(602);
            if (PlayerPrefs.GetInt("TreasureBoxUpgradeLevel") < ul.MaxUnlockCount)
            {
                _lockPassivesList.Add(602);
            }
        }
        else
        {
            _lockPassivesList.Add(602);
        }

        if (PlayerPrefs.HasKey("ComboUpgradeLevel"))
        {
            UnlockItem ul = (UnlockItem)GetItemInfo(603);
            if (PlayerPrefs.GetInt("ComboUpgradeLevel") < ul.MaxUnlockCount)
            {
                _lockPassivesList.Add(603);
            }
        }
        else
        {
            _lockPassivesList.Add(603);
        }
    }

    void ReadItemData()
    {
        TextAsset txt = (TextAsset)Resources.Load("Datas/csv_ItemList") as TextAsset;
        string[] lines;

        lines = Regex.Split(txt.text, LINE_SPLIT); // 정규식 사용방법
        string[] header = Regex.Split(lines[0], SPLIT);
        for (int i = 1; i < lines.Length; i++)
        {
            string[] values = Regex.Split(lines[i], SPLIT);
            if (values.Length == 0 || values[0] == "") continue;

            Item data = null;
            switch ((ItemType)(int.Parse(values[2])))
            {
                case ItemType.Currency:
                    Currency cr = new Currency();
                    for (int j = 0; j < header.Length && j < values.Length; j++)
                    {
                        if (header[j] == "ItemID") cr._ItemID = int.Parse(values[j]);
                        else if (header[j] == "Name") cr._Name = values[j];
                        else if (header[j] == "ItemType") cr._itemType = (ItemType)(int.Parse(values[j]));
                    }
                    data = cr;
                    break;
                case ItemType.Shovel:
                    Shovel sv = new Shovel();
                    for (int j = 0; j < header.Length && j < values.Length; j++)
                    {
                        if (header[j] == "ItemID") sv._ItemID = int.Parse(values[j]);
                        else if (header[j] == "Name") sv._Name = values[j];
                        else if (header[j] == "ItemType") sv._itemType = (ItemType)(int.Parse(values[j]));
                        else if (header[j] == "1") sv.ShovelPower = int.Parse(values[j]);
                        else if (header[j] == "2") sv.UnlockPrice = int.Parse(values[j]);
                        else if (header[j] == "3") sv.Price = int.Parse(values[j]);
                        else if (header[j] == "4") sv.ItemInfo = values[j];
                    }
                    data = sv;
                    break;
                case ItemType.Weapon:
                    Weapon wp = new Weapon();
                    for (int j = 0; j < header.Length && j < values.Length; j++)
                    {
                        if (header[j] == "ItemID") wp._ItemID = int.Parse(values[j]);
                        else if (header[j] == "Name") wp._Name = values[j];
                        else if (header[j] == "ItemType") wp._itemType = (ItemType)(int.Parse(values[j]));
                        else if (header[j] == "1") wp.Attack = int.Parse(values[j]);
                        else if (header[j] == "2") wp.weaponType = (WeaponType)int.Parse(values[j]);
                        else if (header[j] == "3") wp.UnlockPrice = int.Parse(values[j]);
                        else if (header[j] == "4") wp.Price = int.Parse(values[j]);
                        else if (header[j] == "5") wp.ItemInfo = values[j];
                        else if (header[j] == "6") wp.WeaponEffectType = (WeaponEffectType)int.Parse(values[j]);
                    }
                    data = wp;
                    break;
                case ItemType.Potion:
                    Potion pt = new Potion();
                    for (int j = 0; j < header.Length && j < values.Length; j++)
                    {
                        if (header[j] == "ItemID") pt._ItemID = int.Parse(values[j]);
                        else if (header[j] == "Name") pt._Name = values[j];
                        else if (header[j] == "ItemType") pt._itemType = (ItemType)(int.Parse(values[j]));
                        else if (header[j] == "1") pt.Heal = int.Parse(values[j]);
                        else if (header[j] == "2") pt.UnlockPrice = int.Parse(values[j]);
                        else if (header[j] == "3") pt.Price = int.Parse(values[j]);
                        else if (header[j] == "4") pt.ItemInfo = values[j];
                    }
                    data = pt;
                    break;
                case ItemType.Armor:
                    Armor ar = new Armor();
                    for (int j = 0; j < header.Length && j < values.Length; j++)
                    {
                        if (header[j] == "ItemID") ar._ItemID = int.Parse(values[j]);
                        else if (header[j] == "Name") ar._Name = values[j];
                        else if (header[j] == "ItemType") ar._itemType = (ItemType)(int.Parse(values[j]));
                        else if (header[j] == "1") ar.Defence = int.Parse(values[j]);
                        else if (header[j] == "2") ar.UnlockPrice = int.Parse(values[j]);
                        else if (header[j] == "3") ar.Price = int.Parse(values[j]);
                        else if (header[j] == "4") ar.ItemInfo = values[j];
                    }
                    data = ar;
                    break;
                case ItemType.Unlock:
                    UnlockItem ul = new UnlockItem();
                    for (int j = 0; j < header.Length && j < values.Length; j++)
                    {
                        if (header[j] == "ItemID") ul._ItemID = int.Parse(values[j]);
                        else if (header[j] == "Name") ul._Name = values[j];
                        else if (header[j] == "ItemType") ul._itemType = (ItemType)(int.Parse(values[j]));
                        else if (header[j] == "1") ul.MaxUnlockCount = int.Parse(values[j]);
                        else if (header[j] == "2") ul.ItemInfo = values[j];
                    }
                    data = ul;
                    break;
            }
            data._ItemIcon = Resources.Load<Sprite>("Item/Item" + data._ItemID);
            ItemDataList.Add(data);
        }
    }

    public Item GetItemInfo(int itemID)
    {
        for(int i = 0; i < ItemDataList.Count; i++)
        {
            if (ItemDataList[i]._ItemID == itemID)
            {
                return ItemDataList[i];
            }
        }
        return null;
    }

    void LoadSound()
    {
        _soundEffect.Add(Resources.Load<AudioClip>("SoundsUpdate/Enemy/en_cow_attack"));
        _soundEffect.Add(Resources.Load<AudioClip>("SoundsUpdate/Enemy/en_cow_hit")); 
        _soundEffect.Add(Resources.Load<AudioClip>("SoundsUpdate/Enemy/en_cow_death"));
        _soundEffect.Add(Resources.Load<AudioClip>("SoundsUpdate/Enemy/en_goblin_attack"));
        _soundEffect.Add(Resources.Load<AudioClip>("SoundsUpdate/Enemy/en_goblin_hit"));
        _soundEffect.Add(Resources.Load<AudioClip>("SoundsUpdate/Enemy/en_goblin_death"));
        _soundEffect.Add(Resources.Load<AudioClip>("SoundsUpdate/Enemy/en_slime_hit"));
        _soundEffect.Add(Resources.Load<AudioClip>("SoundsUpdate/Enemy/en_slime_death"));
        _soundEffect.Add(Resources.Load<AudioClip>("SoundsUpdate/Enemy/en_zombie_attack"));
        _soundEffect.Add(Resources.Load<AudioClip>("SoundsUpdate/Enemy/en_zombie_hit"));
        _soundEffect.Add(Resources.Load<AudioClip>("SoundsUpdate/Enemy/en_zombie_death"));
        _soundEffect.Add(Resources.Load<AudioClip>("SoundsUpdate/Enemy/en_Elite_Goblin_attack"));
        _soundEffect.Add(Resources.Load<AudioClip>("SoundsUpdate/Enemy/en_Elite_Goblin_hit"));
        _soundEffect.Add(Resources.Load<AudioClip>("SoundsUpdate/Enemy/en_Elite_Goblin_death"));
        _soundEffect.Add(Resources.Load<AudioClip>("SoundsUpdate/InterAction/Chest_Open"));
        _soundEffect.Add(Resources.Load<AudioClip>("SoundsUpdate/InterAction/ItemUse_potion"));
        _soundEffect.Add(Resources.Load<AudioClip>("SoundsUpdate/InterAction/Pickup_Diamond"));
        _soundEffect.Add(Resources.Load<AudioClip>("SoundsUpdate/Player/Player_ShopItemPurchase"));
        _soundEffect.Add(Resources.Load<AudioClip>("SoundsUpdate/Player/Player_Dig"));
        _soundEffect.Add(Resources.Load<AudioClip>("SoundsUpdate/Player/Player_OpenDoor"));
        _soundEffect.Add(Resources.Load<AudioClip>("SoundsUpdate/Player/Player_Hurt"));
        _soundEffect.Add(Resources.Load<AudioClip>("SoundsUpdate/Player/Player_Death"));
        _soundEffect.Add(Resources.Load<AudioClip>("SoundsUpdate/Ui/Ui_Select_Down"));
        _soundEffect.Add(Resources.Load<AudioClip>("SoundsUpdate/Ui/Ui_Select"));
    }

    #endregion
}


#region Data 클래스들 모음

public class StageStartPosition
{
    public Vector3 LobbyPosition = new Vector3(-28, 100, 0);
    public Vector3 Stage1F1 = new Vector3(-0, 99, 0);
    public Vector3 Stage1F2 = new Vector3(55, 108, 0);
    public Vector3 Stage1F3 = new Vector3(111, 89, 0);
    public Vector3 Stage1FBoss = new Vector3(169, 86, 0);
}

[Serializable]
public class Item
{
    public int _ItemID;
    public string _Name;
    public ItemType _itemType = ItemType.Currency;
    public Sprite _ItemIcon;
}

[Serializable]
public class Currency : Item
{
    public int Count;
}

[Serializable]
public class Shovel : Item
{
    public int ShovelPower;
    public int UnlockPrice;
    public int Price;
    public String ItemInfo;
    public ShoverEffectType ShoverEffectType;
}

[Serializable]
public class Weapon : Item
{
    public int Attack;
    public WeaponType weaponType;
    public int UnlockPrice;
    public int Price;
    public String ItemInfo;
    public WeaponEffectType WeaponEffectType;
}

[Serializable]
public class Armor : Item
{
    public int Defence;
    public int UnlockPrice;
    public int Price;
    public String ItemInfo;
}

[Serializable]
public class Potion : Item
{
    public int Heal;
    public int UnlockPrice;
    public int Price;
    public String ItemInfo;
}

[Serializable]
public class UnlockItem : Item
{
    public int MaxUnlockCount;
    public String ItemInfo;
}
[Serializable]
public class SaveData
{
    //플레이어 저장
    public int _gold;
    public int _dia;
    public int _nowHP;
    public floor _nowFloor = floor.f1;
    public Stage _nowStage = Stage.Lobby;
    public int _equipShovelID = 0;
    public int _equipWeaponID = 0;
    public int _equipArmorID = 0;
    public int _equipPotionID = 0;    
    public List<int> _unlockItemId = new List<int>();
}

#endregion

#region Enum모음
public enum ItemType
{
    Currency,
    Shovel,
    Weapon,
    Potion,
    Armor,
    Unlock,
}

public enum WeaponType
{
    Dagger,
    GreatSword,
    Spear,
}

public enum ShoverEffectType
{
    Normal,
    Titanium,
}

public enum WeaponEffectType
{
    Normal,
    Titanium,
    Administrator,
}

public enum CharacterState
{
    Live,
    Death,
}

public enum Stage
{
    Lobby = 0,
    Stage1 = 1,
    Stage2 = 2,
}

public enum floor
{
    f1 = 0,
    f2 = 1,
    f3 = 2,    
    fBoss = 3,
}

public enum BoxType
{
    Normal,
    Clear,
}

public enum SoundEffect
{
    CowAttack,//몬스터가 공격
    CowHit,//몬스터가 맞을때
    CowDeath,//몬스터 사망
    GoblinAttack,
    GoblinHit,
    GoblinDeath,
    SlimeHit,
    SlimeDeath,
    ZombieAttack,
    //좀비맞는거 추가
    ZombieHit,
    ZombieDeath,
    EliteGoblinAttack,
    //엘리트 맞는거 추가
    EliteGoblinHit,
    EliteGoblinDeath,
    BoxOpen,
    UsePotion,
    GetItem,
    UnLock,
    Dig,
    OpenDoor,
    PlayerHit,
    PlayerDeath,
    UIChange,
    UISelect,
}

#endregion


