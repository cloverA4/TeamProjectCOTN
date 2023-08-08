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
        _stairCollider.enabled = false; // 리지드바디 끄기
    }

    void Update()
    {
        //if () //엘리트몬스터가 죽었을때
        //{
        //    gameObject.GetComponent<BoxCollider2D>().enabled = false; // 자기자신의 박스콜라이더를 꺼주고
        //    _benStair.SetActive(false); // 
        //    _stairCollider.enabled = true;
        //}
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            //ui "엘리트 몬스터를 처치해야합니다!" 호출
        }
    }
}
