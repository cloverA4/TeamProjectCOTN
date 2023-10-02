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
    [SerializeField] Image _diaImage;
    [SerializeField] GameObject _shovelSlot;
    [SerializeField] GameObject _armorSlot;
    [SerializeField] GameObject _weaponSlot;
    [SerializeField] GameObject _potionSlot;
    [SerializeField] GameObject _diaSlot;

    private void Update()
    {
        if (_shovelImage.GetComponent<Transform>().position != new Vector3(0, 0, 0))
        {
            _shovelImage.GetComponent<RectTransform>().anchoredPosition = Vector3.Lerp(_shovelImage.GetComponent<RectTransform>().anchoredPosition, new Vector3(0, 0, 0), Time.deltaTime * 2f);
        }
        if (_armorImage.GetComponent<Transform>().position != new Vector3(0, 0, 0))
        {
            _armorImage.GetComponent<RectTransform>().anchoredPosition = Vector3.Lerp(_armorImage.GetComponent<RectTransform>().anchoredPosition, new Vector3(0, 0, 0), Time.deltaTime * 2f);

        }
        if (_weaponImage.GetComponent<Transform>().position != new Vector3(0, 0, 0))
        {
            _weaponImage.GetComponent<RectTransform>().anchoredPosition = Vector3.Lerp(_weaponImage.GetComponent<RectTransform>().anchoredPosition, new Vector3(0, 0, 0), Time.deltaTime * 2f);
        }
        if (_potionImage.GetComponent<Transform>().position != new Vector3(0, 0, 0))
        {
            _potionImage.GetComponent<RectTransform>().anchoredPosition = Vector3.Lerp(_potionImage.GetComponent<RectTransform>().anchoredPosition, new Vector3(0, 0, 0), Time.deltaTime * 2f);
        }
        if (_diaImage.GetComponent<Transform>().position != new Vector3(0, 0, 0))
        {
            _diaImage.GetComponent<RectTransform>().anchoredPosition = Vector3.Lerp(_diaImage.GetComponent<RectTransform>().anchoredPosition, new Vector3(0, 0, 0), Time.deltaTime * 2f);
        }
    }

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
        IconMove(item);
    }
    Image image = null;
    void IconMove(Item item)
    {
        if(image != null)
        {
            image.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, 0, 0);
        }
        
        switch (item._itemType)
        {
            case ItemType.Shovel: image = _shovelImage; break;
            case ItemType.Weapon: image = _weaponImage; break;
            case ItemType.Armor: image = _armorImage; break;
            case ItemType.Potion: image = _potionImage; break;
            case ItemType.Currency: image = _diaImage; break;
        }

        image.transform.SetParent(transform);
        image.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, 0, 0);

        switch (item._itemType)
        {
            case ItemType.Shovel: image.transform.SetParent(_shovelSlot.transform); break;
            case ItemType.Weapon: image.transform.SetParent(_weaponSlot.transform); break;
            case ItemType.Armor:  image.transform.SetParent(_armorSlot.transform); break;
            case ItemType.Potion: image.transform.SetParent(_potionSlot.transform); break;
            case ItemType.Currency: image.transform.SetParent(_diaSlot.transform); break;
        }
    }
   

    #endregion
}
