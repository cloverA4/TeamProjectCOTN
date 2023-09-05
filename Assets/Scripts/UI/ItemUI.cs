using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemUI : MonoBehaviour
{
    [SerializeField] GameObject _itemPrice;
    [SerializeField] GameObject _itemInfo;
    [SerializeField] GameObject _itemName;

    void Start()
    {
        ItemUIInit();
    }

    void ItemUIInit()
    {
        _itemInfo.SetActive(false);
        _itemName.SetActive(false);
        _itemPrice.SetActive(false);
    }
    // 상점 일 경우 아이템 가격 UI 활성화 아닐경우 비활
    public void ActiveItemPrice(int price)
    {
        _itemPrice.GetComponent<Text>().text = price.ToString();
    }

    // 아이템을 먹을 경우 아이템 이름 애니메이션 출력

    public void ActiveItemInfo(string info)
    {
        _itemInfo.GetComponent<Text>().text = info;
    }

    // 아이템 기준 십자 위치에 플레이어가 있을경우 아이템 설명 유아이 출력

}
