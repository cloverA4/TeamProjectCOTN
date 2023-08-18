using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Data : MonoBehaviour
{
    #region ��������

    string LINE_SPLIT = @"\r\n|\n\r|\n|\r";
    string SPLIT = ",";
    public int[] UnlockPirceToLevel = new int[3] { 2, 4, 7 };
    List<Item> ItemDataList = new List<Item>();
    public SaveData CharacterSaveData = new SaveData();

    #endregion

    private static Data instance = null;
    private void Awake()
    {
        if (null == instance)
        {
            instance = this;

            //�� ��ȯ�� �Ǵ��� �ı����� �ʰ� �Ѵ�.
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            //���� �� �̵��� �Ǿ��µ� �� ������ Hierarchy�� GameMgr�� ������ ���� �ִ�.
            //�׷� ��쿣 ���� ������ ����ϴ� �ν��Ͻ��� ��� ������ִ� ��찡 ���� �� ����.
            //�׷��� �̹� ���������� instance�� �ν��Ͻ��� �����Ѵٸ� �ڽ��� �������ش�.
            Destroy(this.gameObject);
        }

    }

    //���� �Ŵ��� �ν��Ͻ��� ������ �� �ִ� ������Ƽ. static�̹Ƿ� �ٸ� Ŭ�������� ���� ȣ���� �� �ִ�.
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
        CharacterSaveData._equipItemId = new List<int>();
        for (int i = 0; i < PlayerController.Instance.PlayerEquipItemList.Count; i++)
        {
            CharacterSaveData._equipItemId.Add(PlayerController.Instance.PlayerEquipItemList[i]._ItemID);
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
        // ���� ���࿡ �ʿ��� ���� ������ �Լ� ������� �ε�
        ReadItemData();
        LoadSaveData();
        yield return null;

        // �������� �ʿ��� ��� ���ӵ����͸� �ε尡�Ǹ� gmaeScene�ε�
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
        //���̽����Ͽ��� ���׷��̵� ������ �о����
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
                if(CharacterSaveData._equipItemId.Count <= 0)
                {
                    CharacterSaveData._equipItemId = null;
                }
            }
            else Debug.Log("������ �����ϴ�.");
        }
        else
        {
            Debug.Log("������ �����ϴ�.");
            CharacterSaveData._gold = 0;
            CharacterSaveData._dia = 0;
            CharacterSaveData._nowStage = Stage.Lobby;
            CharacterSaveData._nowFloor = floor.f1;
            CharacterSaveData._nowHP = 0;
            CharacterSaveData._equipItemId = null;
        }

        Debug.Log("�ε��Ϸ�");
    }


    void ReadItemData()
    {
        string path = Application.dataPath + "/Resources/Datas/csv_ItemList.csv";
        string[] lines;

        if (File.Exists(path))
        {
            string source;
            using (StreamReader sr = new StreamReader(path))
            {
                source = sr.ReadToEnd();

                lines = Regex.Split(source, LINE_SPLIT); // ���Խ� �����
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

    #endregion
}


#region Data Ŭ������ ����

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
[Serializable]
public class SaveData
{
    //�÷��̾� ����
    public int _gold;
    public int _dia;
    public int _nowHP;
    public floor _nowFloor = floor.f1;
    public Stage _nowStage = Stage.Lobby;
    public List<int> _equipItemId = new List<int>();
}

#endregion

#region Enum����
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


