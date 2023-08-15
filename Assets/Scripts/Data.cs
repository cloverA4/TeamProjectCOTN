using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Transactions;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Data : MonoBehaviour
{
    #region 전역변수

    SaveDataList _saveDataList;
    string LINE_SPLIT = @"\r\n|\n\r|\n|\r";
    string SPLIT = ",";
    public int[] UnlockPirceToLevel = new int[3] { 2, 4, 7 };
    public List<Item> ItemDataList = new List<Item>();

    #endregion

    private static Data instance = null;
    private void Awake()
    {
        if (null == instance)
        {
            instance = this;

            //씬 전환이 되더라도 파괴되지 않게 한다.
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            //만약 씬 이동이 되었는데 그 씬에도 Hierarchy에 GameMgr이 존재할 수도 있다.
            //그럴 경우엔 이전 씬에서 사용하던 인스턴스를 계속 사용해주는 경우가 많은 것 같다.
            //그래서 이미 전역변수인 instance에 인스턴스가 존재한다면 자신을 삭제해준다.
            Destroy(this.gameObject);
        }

    }

    //게임 매니저 인스턴스에 접근할 수 있는 프로퍼티. static이므로 다른 클래스에서 맘껏 호출할 수 있다.
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

    #region GameDataLoad

    public event EventHandler LoadingEnd;
    AsyncOperation asyncOperation;

    public IEnumerator LoadGame()
    {
        // 게임 진행에 필요한 게임 데이터 함수 순서대로 로드
        Read();
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

    public void LoadItemData() // loadingScene에서 호출
    {
        // 아이템 데이터 json파일 호출
    }

    void LoadSaveData()
    {
        // 세이브 데이타가 있을경우 세이브 데이타 호출 후 씬로드

        // 세이브 데이타가 없을 경우 기본값 설정해주는 함수 호출 후 씬 로드
    }

    void Read()
    {
        string path = Application.dataPath + "/Resources/Datas/csv_ItemList.csv";
        string[] lines;

        if (File.Exists(path))
        {
            string source;
            using (StreamReader sr = new StreamReader(path))
            {
                source = sr.ReadToEnd();

                lines = Regex.Split(source, LINE_SPLIT); // 정규식 사용방법
                string[] header = Regex.Split(lines[0], SPLIT);
                for (int i = 1; i < lines.Length; i++)
                {
                    string[] values = Regex.Split(lines[i], SPLIT);
                    if (values.Length == 0 || values[0] == "") continue;

                    Item data = new Item();
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
                            }
                            data = ul;
                            break;
                    }

                    ItemDataList.Add(data);
                }
            }
        }

        Debug.Log(ItemDataList.Count);
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
}

[Serializable]
public class Weapon : Item
{
    public int Attack;
    public WeaponType weaponType;
    public int UnlockPrice;
    public int Price;
}

[Serializable]
public class Armor : Item
{
    public int Defence;
    public int UnlockPrice;
    public int Price;
}

[Serializable]
public class Potion : Item
{
    public int Heal;
    public int UnlockPrice;
    public int Price;
}

[Serializable]
public class UnlockItem : Item
{
    public int MaxUnlockCount;
}

public class SaveDataList
{
    List<SaveData> _saveList = new List<SaveData>();
}
public class SaveData
{
    //플레이어 저장
    float _nowHP;
    float _maxHP;
    Vector3 _nowPosition = Vector3.zero;

    public SaveData(float NowHP, float MaxHP, Vector3 vec)
    {
        _nowHP = NowHP;
        _maxHP = MaxHP;
        _nowPosition = vec;
    }
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

#endregion


