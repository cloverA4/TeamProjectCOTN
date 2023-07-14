using System;
using System.Collections.Generic;
using UnityEngine;

public class Data : MonoBehaviour
{
    #region ��������

    ItemTable _itemTable;

    
    #endregion


    void LoadDataFromJson()
    {
        TextAsset dataJson = Resources.Load<TextAsset>("TestCase/Json/ItemData");
        Debug.Log(dataJson);
        _itemTable = JsonUtility.FromJson<ItemTable>(dataJson.text);
        foreach (var It in _itemTable.itemTable )
        {
            
        }
    } // json ���� �ε� �Լ�

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

    void PlayerInit()
    {
        //�ε��ؿ� �÷��̾� ������ �Է�
    }
    
    public Item SearchItem(int itemID)
    {
        Item _tempItem;
        return null;
    } // ������ Ǯ���� ������ID�� ã�Ƽ� ������ ���� �޾ƿ��� �Լ�

}

#region Data Ŭ������ ����

[Serializable]
public class Item
{
    int _ItemID;

    public int ItemID
    {
        get { return _ItemID; }
        set
        {
            _ItemID = value;
        }
    }

    string _Name;
    public string Name
    {
        get { return _Name; }
        set
        {
            _Name = value;
        }
    }

    Sprite _itemImage;
    public ItemType _itemType = ItemType.Null;
}
public class ItemTable
{
    public List<Item> itemTable;
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
    Null,
    Shovel,
    Weapon,
    Armor,
    Potion,
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


