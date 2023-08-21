using UnityEngine;

public class DropItem : MonoBehaviour
{
    [SerializeField] SpriteRenderer _ItemIcon;
    Item _item;
    bool _isShopItem = false;
    public Item Item 
    {
        private set { _item = value; }
        get { return _item; }
    }

    public void Init(Item item, bool shop = false)
    {
        _item = item;
        _ItemIcon.sprite = _item._ItemIcon;
        _isShopItem = shop;
    }
}
