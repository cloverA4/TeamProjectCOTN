using System;
using System.Collections.Generic;
using UnityEngine;

public class Data : MonoBehaviour
{
    #region 전역변수

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
    } // json 파일 로드 함수

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

    void PlayerInit()
    {
        //로드해온 플레이어 데이터 입력
    }
    
    public Item SearchItem(int itemID)
    {
        Item _tempItem;
        return null;
    } // 아이템 풀에서 아이템ID를 찾아서 아이템 정보 받아오는 함수

}

#region Data 클래스들 모음

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


