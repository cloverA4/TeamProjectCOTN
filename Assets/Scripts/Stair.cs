using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stair : MonoBehaviour
{
    [SerializeField]
    private Stage _nowStage;
    [SerializeField]
    private floor _nowFloor;
    [SerializeField] Transform _startPoint;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            GameManager.Instance.NowStage = _nowStage;
            GameManager.Instance.NowFloor = _nowFloor;
            GameManager.Instance.StartPoint = _startPoint.position;
            GameManager.Instance.FaidIn();
        }
    }
}
