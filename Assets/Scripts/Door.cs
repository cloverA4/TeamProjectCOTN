using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Door : MonoBehaviour
{
    private int _wallCount;
    private int _WallCountReset = 2;
    SpriteRenderer _doorSortingOrder;
    //1
    //[SerializeField]
    //LayerMask objectLayer;

    void Start()
    {
        _wallCount = _WallCountReset;
        _doorSortingOrder = GetComponentsInChildren<SpriteRenderer>()[1];
        _doorSortingOrder.sortingOrder = (int)transform.position.y * -1; // y의 위치를 확인하고 이미지를 정렬시켜주는 구문     
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
