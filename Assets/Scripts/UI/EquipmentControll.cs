using System.Collections;
using UnityEngine;
using UnityEngine.UI;

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
        Vector3 targetPos = _weaponImage.GetComponent<RectTransform>().position;
        _weaponImage.GetComponent<RectTransform>().position = new Vector3(Camera.main.pixelWidth / 2, Camera.main.pixelHeight / 2);
        _weaponImage.GetComponent<Image>().sprite = _item._ItemIcon;
        Vector3 tempPos = _weaponImage.GetComponent<RectTransform>().position;
        var time = 0f;
        while (time < 1f)
        {
            time += Time.deltaTime;
            _weaponImage.GetComponent<RectTransform>().position = Vector3.Lerp(tempPos, targetPos, time / 0.5f);
            yield return null;
        }
        yield return null;
    }
    IEnumerator ArmorMove(Item _item)
    {
        Vector3 targetPos = _armorImage.GetComponent<RectTransform>().position;
        _armorImage.GetComponent<RectTransform>().position = new Vector3(Camera.main.pixelWidth / 2, Camera.main.pixelHeight / 2);
        _armorImage.GetComponent<Image>().sprite = _item._ItemIcon;
        Vector3 tempPos = _armorImage.GetComponent<RectTransform>().position;
        var time = 0f;
        while (time < 1f)
        {
            time += Time.deltaTime;
            _armorImage.GetComponent<RectTransform>().position = Vector3.Lerp(tempPos, targetPos, time / 0.5f);
            yield return null;
        }
        yield return null;
    }
    IEnumerator ShovelMove(Item _item)
    {
        Vector3 targetPos = _shovelImage.GetComponent<RectTransform>().position;
        _shovelImage.GetComponent<RectTransform>().position = new Vector3(Camera.main.pixelWidth / 2, Camera.main.pixelHeight / 2);
        _shovelImage.GetComponent<Image>().sprite = _item._ItemIcon;
        Vector3 tempPos = _shovelImage.GetComponent<RectTransform>().position;
        var time = 0f;
        while (time < 1f)
        {
            time += Time.deltaTime;
            _shovelImage.GetComponent<RectTransform>().position = Vector3.Lerp(tempPos, targetPos, time / 0.5f);
            yield return null;
        }
        yield return null;
    }
    IEnumerator PotionMove(Item _item)
    {
        Vector3 targetPos = _potionImage.GetComponent<RectTransform>().position;
        _potionImage.GetComponent<RectTransform>().position = new Vector3(Camera.main.pixelWidth / 2, Camera.main.pixelHeight / 2);
        _potionImage.GetComponent<Image>().sprite = _item._ItemIcon;
        Vector3 tempPos = _potionImage.GetComponent<RectTransform>().position;
        var time = 0f;
        while (time < 1f)
        {
            time += Time.deltaTime;
            _potionImage.GetComponent<RectTransform>().position = Vector3.Lerp(tempPos, targetPos, time / 0.5f);
            yield return null;
        }
        yield return null;
    }

    #endregion
}
