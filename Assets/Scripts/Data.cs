using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Experimental.GraphView.GraphView;

public class Data : MonoBehaviour
{
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
    public ItemType _itemType = ItemType.Armor;
}
public enum ItemType
{
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