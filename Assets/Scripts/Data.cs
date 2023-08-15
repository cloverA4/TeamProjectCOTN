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
    #region ��������

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

    #region GameDataLoad

    public event EventHandler LoadingEnd;
    AsyncOperation asyncOperation;

    public IEnumerator LoadGame()
    {
        // ���� ���࿡ �ʿ��� ���� ������ �Լ� ������� �ε�
        Read();
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

    public void LoadItemData() // loadingScene���� ȣ��
    {
        // ������ ������ json���� ȣ��
    }

    void LoadSaveData()
    {
        // ���̺� ����Ÿ�� ������� ���̺� ����Ÿ ȣ�� �� ���ε�

        // ���̺� ����Ÿ�� ���� ��� �⺻�� �������ִ� �Լ� ȣ�� �� �� �ε�
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

                lines = Regex.Split(source, LINE_SPLIT); // ���Խ� �����
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

public class SaveDataList
{
    List<SaveData> _saveList = new List<SaveData>();
}
public class SaveData
{
    //�÷��̾� ����
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


