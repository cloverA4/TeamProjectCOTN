using System.Collections;
using TMPro;
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

    [SerializeField] GameObject _test;


    public void ItemIconMove(Item _item) 
    {
        switch (_item._itemType)
        {
            case ItemType.Shovel:
                StartCoroutine(ShovelMove(_item));
                break;
            case ItemType.Weapon:
                StartCoroutine(WeaponMove(_item));
                break;
            case ItemType.Armor:
                StartCoroutine(ArmorMove(_item));
                break;
            case ItemType.Potion:
                StartCoroutine(PotionMove(_item));
                break;
        }
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
                temp.GetComponent<RectTransform>().position = Vector3.MoveTowards(temp.GetComponent<RectTransform>().position, _weapon.position, 10f);
                Debug.Log(Vector3.Distance(temp.GetComponent<RectTransform>().position, targetPos));
                yield return null;  
            }
            if (Vector3.Distance(temp.GetComponent<RectTransform>().position, targetPos) == 0)
            {
                Destroy(temp);
                break;
            }
        }
        yield return null;
    }
    IEnumerator ArmorMove(Item _item)
    {
        GameObject temp = Instantiate(_itemIcon, transform);
        temp.GetComponent<Image>().sprite = _item._ItemIcon;
        Vector3 tempPos = temp.GetComponent<RectTransform>().position;
        Vector3 targetPos = _armor.GetComponent<RectTransform>().position;
        while (true)
        {
            if (Vector3.Distance(tempPos, targetPos) > 0.1f)
            {
                temp.GetComponent<RectTransform>().position = Vector3.MoveTowards(temp.GetComponent<RectTransform>().position, _armor.position, 10f);
                yield return null;
            }
            if (Vector3.Distance(temp.GetComponent<RectTransform>().position, targetPos) == 0)
            {
                Destroy(temp);
                break;
            }
        }
        yield return null;
    }
    IEnumerator ShovelMove(Item _item)
    {
        GameObject temp = Instantiate(_itemIcon, transform);
        temp.GetComponent<Image>().sprite = _item._ItemIcon;
        Vector3 tempPos = temp.GetComponent<RectTransform>().position;
        Vector3 targetPos = _shovel.GetComponent<RectTransform>().position;
        while (true)
        {
            if (Vector3.Distance(tempPos, targetPos) > 0.1f)
            {
                temp.GetComponent<RectTransform>().position = Vector3.MoveTowards(temp.GetComponent<RectTransform>().position, _shovel.position, 10f);
                yield return null;
            }
            if (Vector3.Distance(temp.GetComponent<RectTransform>().position, targetPos) == 0)
            {
                Destroy(temp);
                break;
            }
        }
        yield return null;
    }
    IEnumerator PotionMove(Item _item)
    {
        GameObject temp = Instantiate(_itemIcon, transform);
        temp.GetComponent<Image>().sprite = _item._ItemIcon;
        Vector3 tempPos = temp.GetComponent<RectTransform>().position;
        Vector3 targetPos = _potion.GetComponent<RectTransform>().position;
        while (true)
        {
            if (Vector3.Distance(tempPos, targetPos) > 0.1f)
            {
                temp.GetComponent<RectTransform>().position = Vector3.MoveTowards(temp.GetComponent<RectTransform>().position, _potion.position, 10f);
                yield return null;
            }
            if (Vector3.Distance(temp.GetComponent<RectTransform>().position, targetPos) == 0)
            {
                Destroy(temp);
                break;
            }
        }
        yield return null;
    }
}
