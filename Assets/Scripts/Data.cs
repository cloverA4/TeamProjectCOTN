using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using System.Security.Cryptography;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Experimental.GraphView.GraphView;

public class Data : MonoBehaviour
{
    #region ��������

    ItemTable _itemTable;

    //�������� ������
    Stage _nowStage = Stage.Lobby;

    public Stage NowStage
    {
        get { return _nowStage; }
        set { _nowStage = value; }
    }
    
    //ĳ���� ������
    PlayerCharacterData _player = new PlayerCharacterData();
    public PlayerCharacterData Player
    {
        get { return _player; }
        set { _player = value; }
    }

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

public class PlayerCharacterData
{
    float _hp;

    public float HP
    {
        get { return _hp; }
        set { _hp = value; }
    }

    float _maxHp;

    public float MaxHP
    {
        get { return _maxHp; }
        set { _maxHp = value; }
    }

    Transform _playerTransform;

    public Transform PlayerTransform
    {
        get { return _playerTransform; }
        set { _playerTransform = value; }
    }

    CharacterState _state = CharacterState.Live;

    public CharacterState State 
    {
        get { return _state; }
        set { _state = value; }
    }

    List<Item> _equpedItemList = new List<Item>();
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
    Stage3 = 3,
    StageBoss = 4,
}

#endregion


