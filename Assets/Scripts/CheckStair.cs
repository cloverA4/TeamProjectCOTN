using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckStair : MonoBehaviour
{
    GameObject _benStair;
    BoxCollider2D _stairCollider;


    private void Awake()
    {
        _benStair = GetComponentsInChildren<GameObject>()[1];
        _stairCollider = GetComponentsInChildren<BoxCollider2D>()[2];
    }
    void Start()
    {
        _benStair.SetActive(true);
        _stairCollider.enabled = false; // ������ٵ� ����
    }

    void Update()
    {
        //if () //����Ʈ���Ͱ� �׾�����
        //{
        //    gameObject.GetComponent<BoxCollider2D>().enabled = false; // �ڱ��ڽ��� �ڽ��ݶ��̴��� ���ְ�
        //    _benStair.SetActive(false); // 
        //    _stairCollider.enabled = true;
        //}
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            //ui "����Ʈ ���͸� óġ�ؾ��մϴ�!" ȣ��
        }
    }
}
