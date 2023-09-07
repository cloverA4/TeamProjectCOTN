using UnityEngine;

public class DropItem : MonoBehaviour
{
    [SerializeField] SpriteRenderer _ItemIcon;
    [SerializeField] ItemUI _itemUI;
    Item _item;
    DropItemType _type;

    private void Start()
    {
        _itemUI.ItemUIInit();
    }

    public DropItemType ItemType
    {
        private set { _type = value; }
        get { return _type; }
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
        _type = type;
    }

    public void ChangeItem(Item Changeitem)
    {
        _item = Changeitem;
        _ItemIcon.sprite = _item._ItemIcon;
        _type = DropItemType.Drop;
    }

    public void DeleteDropItem()
    {
        Destroy(gameObject);
    }
}


public enum DropItemType
{
    Drop,
    Shop,
    UnlockShop,
}
