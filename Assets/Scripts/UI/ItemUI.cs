using UnityEngine;
using UnityEngine.UI;
using System;

public class ItemUI : MonoBehaviour
{
    [SerializeField] GameObject _itemPrice;
    [SerializeField] GameObject _itemInfo;
    [SerializeField] GameObject _dia;

    public void ItemUIInit()
    {
        gameObject.SetActive(true);
        _itemInfo.SetActive(false);
        _itemPrice.SetActive(false);
        _dia.SetActive(false);
    }
    
    public void ActiveItemPrice(int price) // 상점 일 경우 아이템 가격 UI 활성화 아닐경우 비활
    {
        _itemPrice.GetComponent<Text>().text = price.ToString();
        _itemPrice.gameObject.SetActive(true);
        _dia.SetActive(false);
    }

    public void ActiveItemUnlockPrice(int price)
    {
        _itemPrice.GetComponent<Text>().text = price.ToString();
        _itemPrice.gameObject.SetActive(true);
        _dia.SetActive(true);
    }

    public void CloseInfo()
    {
        //정보 끄기
        if(_itemInfo.activeSelf) _itemInfo.SetActive(false);
    }

    public void ActiveItemInfo(String info) // 아이템 기준 십자 위치에 플레이어가 있을경우 아이템 설명 유아이 출력
    {
        _itemInfo.GetComponent<Text>().text = info;
        _itemInfo.gameObject.SetActive(true);
    }

}
