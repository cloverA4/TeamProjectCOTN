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
    
    public void ActiveItemPrice(int price) // ���� �� ��� ������ ���� UI Ȱ��ȭ �ƴҰ�� ��Ȱ
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
        //���� ����
        if(_itemInfo.activeSelf) _itemInfo.SetActive(false);
    }

    public void ActiveItemInfo(String info) // ������ ���� ���� ��ġ�� �÷��̾ ������� ������ ���� ������ ���
    {
        _itemInfo.GetComponent<Text>().text = info;
        _itemInfo.gameObject.SetActive(true);
    }

}
