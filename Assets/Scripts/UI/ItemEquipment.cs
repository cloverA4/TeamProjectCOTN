using UnityEngine;
using UnityEngine.UI;

public class ItemEquipment : MonoBehaviour
{
    public void InitImage(Sprite _itemIcon)
    {
        gameObject.GetComponent<Image>().sprite = _itemIcon;
    }
}
