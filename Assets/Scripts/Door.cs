using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    
    void Start()
    {
        GetComponent<SpriteRenderer>().sortingOrder = (int)(transform.position.y + 0.5f) * -1; // y의 위치를 확인하고 이미지를 정렬시켜주는 구문
    }

    

    public void OpenDoor()
    {
        gameObject.SetActive(false);
    }
}
