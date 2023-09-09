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



    // ������ ������ ����
    // ������ ������ �������� ���� ������ ���������� ��ġ�� �̵�
    // �̵� �� ����??

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
        Debug.Log("���� ��ġ" +  tempPos);
        Debug.Log("��ġ : " + tempPos);
        tempPos = Vector3.MoveTowards(tempPos, targetPos, 1);
    }
}
