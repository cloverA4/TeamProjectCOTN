using Mono.Cecil;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropItem : MonoBehaviour
{
    Item _dropItem;
    public Item dropItem
    {
        get { return _dropItem; }   
        set { _dropItem = value; }
    }
    Sprite _itemIcon;


    public void Init(int _itemID)
    {
        _dropItem = Data.Instance.SearchItem(_itemID);
        if(_dropItem == null)
        {
            return;            
        }
        ItemSetting();
    }

    void ItemSetting()
    {
        GameObject _itemObj = Resources.Load<GameObject>("Prefab/Item"+_dropItem.ItemID);
        _itemIcon = _itemObj.GetComponent<Sprite>();
    }

    void DeleteItem()
    {

    }

}
