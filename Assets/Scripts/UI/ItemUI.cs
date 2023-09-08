using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class ItemUI : MonoBehaviour
{
    [SerializeField] GameObject _itemPrice;
    [SerializeField] GameObject _itemInfo;
    [SerializeField] GameObject _itemName;


    public void ItemUIInit()
    {
        gameObject.SetActive(true);
        _itemInfo.SetActive(false);
        _itemName.SetActive(false);
        _itemPrice.SetActive(false);
    }
    // ���� �� ��� ������ ���� UI Ȱ��ȭ �ƴҰ�� ��Ȱ
    public void ActiveItemPrice(int price)
    {
        _itemPrice.GetComponent<Text>().text = price.ToString();
        _itemPrice.gameObject.SetActive(true);
    }

    public void ActiveItemUnlockPrice(int price)
    {
        //���̾ƻ���
    }

    public void CloseInfo()
    {
        //���� ����
    }

    // �������� ���� ��� ������ �̸� �ִϸ��̼� ���

    public void ActiveItemName(string itemName)
    {
        _itemName.GetComponent<Text>().text = itemName;
        _itemName.gameObject.SetActive(true);
    }

    // ������ ���� ���� ��ġ�� �÷��̾ ������� ������ ���� ������ ���

    public void ActiveItemInfo(String info)
    {
        _itemInfo.GetComponent<Text>().text = info;
        _itemInfo.gameObject.SetActive(true);
    }

}
