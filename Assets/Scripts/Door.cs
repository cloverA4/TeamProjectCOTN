using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    
    void Start()
    {
        GetComponent<SpriteRenderer>().sortingOrder = (int)(transform.position.y + 0.5f) * -1; // y�� ��ġ�� Ȯ���ϰ� �̹����� ���Ľ����ִ� ����
    }

    

    public void OpenDoor()
    {
        gameObject.SetActive(false);
    }
}
