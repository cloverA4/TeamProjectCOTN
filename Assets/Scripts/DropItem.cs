using UnityEngine;
using static UnityEditor.Progress;

public class DropItem : MonoBehaviour
{
    [SerializeField] GameObject _ItemNameCanvas;
    [SerializeField] SpriteRenderer _ItemIcon;
    [SerializeField] ItemUI _itemUI;
    Item _item;
    DropItemType _dropItemType;


    public DropItemType DropItemType
    {
        private set { _dropItemType = value; }
        get { return _dropItemType; }
    }

    public Item Item 
    {
        private set { _item = value; }
        get { return _item; }
    }

    public void Init(Item item, DropItemType type = DropItemType.Drop)
    {
        _item = item;
        _ItemIcon.sprite = _item._ItemIcon;
        _dropItemType = type;

        _itemUI.ItemUIInit();

        switch (item._itemType)
        {
            case ItemType.Shovel:
                Shovel sv = (Shovel)item;
                if(type == DropItemType.Shop) _itemUI.ActiveItemPrice(sv.Price);
                else if(DropItemType == DropItemType.UnlockShop) _itemUI.ActiveItemUnlockPrice(sv.UnlockPrice);
                break;
            case ItemType.Weapon:
                Weapon wp = (Weapon)item;
                if (type == DropItemType.Shop) _itemUI.ActiveItemPrice(wp.Price);
                else if (DropItemType == DropItemType.UnlockShop) _itemUI.ActiveItemUnlockPrice(wp.UnlockPrice);
                break;
            case ItemType.Armor:
                Armor ar = (Armor)item;
                if (type == DropItemType.Shop) _itemUI.ActiveItemPrice(ar.Price);
                else if (DropItemType == DropItemType.UnlockShop) _itemUI.ActiveItemUnlockPrice(ar.UnlockPrice);
                break;
            case ItemType.Potion:
                Potion po = (Potion)item;
                if (type == DropItemType.Shop) _itemUI.ActiveItemPrice(po.Price);
                else if (DropItemType == DropItemType.UnlockShop) _itemUI.ActiveItemUnlockPrice(po.UnlockPrice);
                break;
            case ItemType.Unlock:
                UnlockItem ul = (UnlockItem)item;
                int level = 0;
                switch (ul._ItemID)
                {
                    case 601:
                        if (PlayerPrefs.HasKey("PlayerHPUpgradeLevel")) level = PlayerPrefs.GetInt("PlayerHPUpgradeLevel");
                        break;
                    case 602:
                        if (PlayerPrefs.HasKey("TreasureBoxUpgradeLevel")) level = PlayerPrefs.GetInt("TreasureBoxUpgradeLevel");
                        break;
                    case 603:
                        if (PlayerPrefs.HasKey("ComboUpgradeLevel")) level = PlayerPrefs.GetInt("ComboUpgradeLevel");
                        break;
                }
                int needDia = -1;
                for (int j = 0; j < PlayerController.Instance.UnlockSaveData.unlockNeedDias.Count; j++)
                {
                    if (PlayerController.Instance.UnlockSaveData.unlockNeedDias[j].level == level)
                    {
                        needDia = PlayerController.Instance.UnlockSaveData.unlockNeedDias[j].NeedDia;
                    }
                }
                _itemUI.ActiveItemUnlockPrice(needDia);
                break;
        }
    }

    public void OpenItemInfo()
    {
        switch (_item._itemType)
        {
            case ItemType.Currency:
                Currency cr = (Currency)_item;
                _itemUI.ActiveItemInfo(cr._Name);
                break;
            case ItemType.Shovel:
                Shovel sv = (Shovel)_item;
                _itemUI.ActiveItemInfo(sv.ItemInfo);
                break;
            case ItemType.Weapon:
                Weapon wp = (Weapon)_item;
                _itemUI.ActiveItemInfo(wp.ItemInfo);
                break;
            case ItemType.Armor:
                Armor ar = (Armor)_item;
                _itemUI.ActiveItemInfo(ar.ItemInfo);
                break;
            case ItemType.Potion:
                Potion po = (Potion)_item;
                _itemUI.ActiveItemInfo(po.ItemInfo);
                break;
            case ItemType.Unlock:
                UnlockItem ul = (UnlockItem)_item;
                _itemUI.ActiveItemInfo(ul.ItemInfo);
                break;
        }
    }

    public void CloseItemInfo()
    {
        _itemUI.CloseInfo();
    }

    public void ChangeItem(Item Changeitem)
    {
        CreateItemNameUI();
        _item = Changeitem;
        _ItemIcon.sprite = _item._ItemIcon;
        _dropItemType = DropItemType.Drop;
    }

    public void DeleteDropItem()
    {
        Destroy(gameObject);
    }

    public void CreateItemNameUI()
    {
        GameObject go = Instantiate(_ItemNameCanvas, transform);
        go.GetComponent<ItemNameCanvas>().InitItemName(Item._Name);
    }
}


public enum DropItemType
{
    Drop,
    Shop,
    UnlockShop,
}


