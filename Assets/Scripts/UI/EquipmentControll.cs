using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class EquipmentControll : MonoBehaviour
{
    [SerializeField] Image _shovelImage;
    [SerializeField] Image _armorImage;
    [SerializeField] Image _weaponImage;
    [SerializeField] Image _potionImage;
    [SerializeField] GameObject _shovelSlot;
    [SerializeField] GameObject _armorSlot;
    [SerializeField] GameObject _weaponSlot;
    [SerializeField] GameObject _potionSlot;

    #region 장비 이미지 교체
    public void UpdataShovel()
    {
        if (PlayerController.Instance.EquipShovel != null)
        {
            _shovelImage.GetComponent<Image>().sprite = PlayerController.Instance.EquipShovel._ItemIcon;
            _shovelSlot.gameObject.SetActive(true);
            return;
        }
        _shovelSlot.gameObject.SetActive(false);
    }
    public void UpdataWeapon()
    {
        if (PlayerController.Instance.EquipWeapon != null)
        {
            _weaponImage.GetComponent<Image>().sprite = PlayerController.Instance.EquipWeapon._ItemIcon;
            _weaponSlot.gameObject.SetActive(true);
            return;
        }
        _weaponSlot.gameObject.SetActive(false);
    }
    public void UpdateArmor()
    {
        if (PlayerController.Instance.EquipArmor != null)
        {
            _armorImage.GetComponent<Image>().sprite = PlayerController.Instance.EquipArmor._ItemIcon;
            _armorSlot.gameObject.SetActive(true);
            return;
        }
        _armorSlot.gameObject.SetActive(false);
    }
    public void UpdatePotion()
    {
        if (PlayerController.Instance.EquipPotion != null)
        {
            _potionImage.GetComponent<Image>().sprite = PlayerController.Instance.EquipPotion._ItemIcon;
            _potionSlot.gameObject.SetActive(true);
            return;
        }
        _potionSlot.gameObject.SetActive(false);
    }

    public void EquipmentAllDisabel()
    {
        _shovelSlot.gameObject.SetActive(false);
        _weaponSlot.gameObject.SetActive(false);
        _armorSlot.gameObject.SetActive(false);
        _potionSlot.gameObject.SetActive(false);
    }

    #endregion


    #region 장비 장착 애니메이션

    public void ItemIconMove(Item item)
    {
        StartCoroutine(IconMove(item));
    }

    IEnumerator IconMove(Item item)
    {
        Image image = null;
        switch (item._itemType)
        {
            case ItemType.Shovel:
                image = _shovelImage;
                break;
            case ItemType.Weapon:
                image = _weaponImage;
                break;
            case ItemType.Armor:
                image = _armorImage;
                break;
            case ItemType.Potion:
                image = _potionImage;
                break;
        }
        if (image != null)
        {
            image.GetComponent<Image>().sprite = item._ItemIcon;
            var time = 0f;
            Debug.Log(UIManeger.Instance.GetComponent<RectTransform>().anchoredPosition);
           
            while (time < 1f)
            {
                time += Time.deltaTime;
                image.GetComponent<RectTransform>().anchoredPosition = Vector3.Lerp(UIManeger.Instance.GetComponent<RectTransform>().localPosition, new Vector3(0,0,0), time / 0.5f);
                yield return null;
            }
        }
        yield return null;
    }
   

    #endregion
}
