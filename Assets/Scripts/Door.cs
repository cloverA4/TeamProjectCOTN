using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Door : MonoBehaviour
{
    private int _wallCount;
    private int _WallCountReset = 2;
    //1
    //[SerializeField]
    //LayerMask objectLayer;

    void Start()
    {
        _wallCount = _WallCountReset;
        GetComponent<SpriteRenderer>().sortingOrder = (int)(transform.position.y + 100) * -2; // y�� ��ġ�� Ȯ���ϰ� �̹����� ���Ľ����ִ� ����     
    }

    public void DoorReset()
    {
        _wallCount = _WallCountReset;
        gameObject.SetActive(true);
    }

    public void updateWallCount()
    {
        _wallCount--;
        if(_wallCount <= 0)
        {
            OpenDoor();
        }
    }

    public void OpenDoor()
    {
        gameObject.SetActive(false);
    }
}
