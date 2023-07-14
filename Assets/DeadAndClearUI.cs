using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeadAndClearUI : MonoBehaviour
{
    [SerializeField] GameObject[] _checkMaskes;

    private void Start()
    {
        gameObject.SetActive(false);
    }

    private void Update()
    {
        gameObject.GetComponent<UIManeger>().ArrowUpdate();
    }


    private void OnEnable()  // ó�� ȭ���� Ȱ��ȭ���� �� üũ����ũ ��Ȱ��ȭ
    {
        for(int i = 0; i< _checkMaskes.Length; i++)
        {
            _checkMaskes[i].SetActive(false);
        }
    }


}
