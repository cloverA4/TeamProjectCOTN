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
    // ���� �� ��� ������ ���� UI Ȱ��ȭ �ƴҰ�� ��Ȱ
    public void ActiveItemPrice(int price)
    {
        _itemPrice.GetComponent<Text>().text = price.ToString();
    }

    // �������� ���� ��� ������ �̸� �ִϸ��̼� ���

    public void ActiveItemInfo(string info)
    {
        _itemInfo.GetComponent<Text>().text = info;
    }

    // ������ ���� ���� ��ġ�� �÷��̾ ������� ������ ���� ������ ���

}
