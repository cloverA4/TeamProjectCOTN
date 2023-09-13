using System.Collections;
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

    private void Update()
    {
       
    }

    public void ItemIconMove(Item _item)
    {
        switch (_item._itemType)
        {
            case ItemType.Shovel:
                break;
            case ItemType.Weapon:
                //StartCoroutine(WeaponMove(_item));
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
        Vector3 tempPos = temp.GetComponent<RectTransform>().position;
        Vector3 targetPos = _weapon.GetComponent<RectTransform>().position;
        temp.GetComponent<RectTransform>().position = targetPos;
    }
    IEnumerator WeaponMove(Item _item)
    {
        GameObject temp = Instantiate(_itemIcon, transform);
        temp.GetComponent<Image>().sprite = _item._ItemIcon;
        Vector3 tempPos = temp.GetComponent<RectTransform>().position;
        Vector3 targetPos = _weapon.GetComponent<RectTransform>().position;
        while (true)
        {
            if (Vector3.Distance(tempPos, targetPos) > 0.1f)
            {
                temp.GetComponent<RectTransform>().position = targetPos;
                Debug.Log("aaa");
                yield return null;
            }
            else if ((Vector3.Distance(tempPos, targetPos) < 0.1f))  break;
        }
        yield return null;
    }
}
