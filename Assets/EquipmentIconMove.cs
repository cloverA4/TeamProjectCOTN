using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class EquipmentIconMove : MonoBehaviour
{
    [SerializeField] GameObject _itemIcon;
    [SerializeField] RectTransform _shovel;
    [SerializeField] RectTransform _weapon;
    [SerializeField] RectTransform _armor;
    [SerializeField] RectTransform _potion;



    // 아이템 아이콘 생성
    // 생성된 아이템 아이콘이 먹은 아이템 종류에따라 위치로 이동
    // 이동 후 삭제??

    public void ItemIconMove(Item _item)
    {
        switch (_item._itemType)
        {
            case ItemType.Shovel:
                break;
            case ItemType.Weapon:
                WeaponIcon(_item);
                break;
            case ItemType.Armor:
                break;
            case ItemType.Potion:
                break;
        }
    }
    void WeaponIcon(Item _item)
    {
        GameObject temp = Instantiate(_itemIcon, transform);
        temp.GetComponent<Image>().sprite = _item._ItemIcon;
        Vector3 tempPos = temp.GetComponent<RectTransform>().localPosition;
        tempPos = Vector3.zero;
        Vector3 targetPos = _weapon.GetComponent<RectTransform>().position;
        Debug.Log("웨펀 위치" +  tempPos);
        Debug.Log("위치 : " + tempPos);
        tempPos = Vector3.MoveTowards(tempPos, targetPos, 1);
    }
}
