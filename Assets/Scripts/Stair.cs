using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stair : MonoBehaviour
{
    [SerializeField]
    private Stage _nowStage;
    [SerializeField]
    private floor _nowFloor;


    SpriteRenderer _benStair;

    private void Start()
    {
        _benStair = GetComponentsInChildren<SpriteRenderer>()[1];
        GameManager.Instance.EventEliteMonsterDie += new EventHandler(EliteMonsterDied);
    }

    //����Ʈ ���Ͱ� ��������� üũ�ϴ� �Լ� �������� �̵������� ȣ��

    public void StageLock()
    {
        _benStair.enabled = !GameManager.Instance.StageClear;
    }

    //����Ʈ ���Ͱ� �׾����� �Ұ���ȯ�� _benStair���ӿ�����Ʈ���ֱ�, �ǵ����̹��� ����

    public void EliteMonsterDied(object sender, EventArgs s)
    {
        StageLock();
    }

    

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (GameManager.Instance.StageClear == true)
            {
                GameManager.Instance.NowStage = _nowStage;
                GameManager.Instance.NowFloor = _nowFloor;
                GameManager.Instance.FaidIn();
            }
            else
            {
                //uiȣ�� "����Ʈ ���͸� óġ�ؾ��մϴ�!"
            }
        }
    }

    private void OnDestroy()
    {
        if(GameManager.Instance != null)
        {
            GameManager.Instance.EventEliteMonsterDie -= new EventHandler(EliteMonsterDied);
        }
    }
}
