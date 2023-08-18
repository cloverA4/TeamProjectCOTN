using UnityEngine;
using static UnityEditor.Progress;

public class DropItem : MonoBehaviour
{
    [SerializeField] SpriteRenderer _ItemIcon;
    Item _item;
    public Item Item 
    {
        private set { _item = value; }
        get { return _item; }
    }

    public void Init(Item item)
    {
        _item = item;
        _ItemIcon.sprite = _item._ItemIcon;
    }
}
