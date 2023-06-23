using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Experimental.GraphView.GraphView;

public class Data : MonoBehaviour
{
    PlayerCharacterData _playerData = new PlayerCharacterData();
    List<Item> ItemList = new List<Item>();

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
    
    public Item SearchItem(int itemID)
    {
        for (int i = 0; i < ItemList.Count; i++)
        {
            if (itemID == ItemList[i].ItemID)
            {
                return ItemList[i];
            }
        }
        return null;
    }

    private void Start()
    {
        
    }

}

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
    public ItemType _itemType = ItemType.Null;
}
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
    idle,
    attack,
    die,
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

    CharacterState state = CharacterState.idle;

    List<Item> _equpedItemList = new List<Item>();
}

class DropTable
{
    
}

