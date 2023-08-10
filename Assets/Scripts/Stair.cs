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
    }

    //����Ʈ ���Ͱ� ��������� üũ�ϴ� �Լ� �������� �̵������� ȣ��

    public void EliteMonsterLive()
    {
        _benStair.enabled = true;
    }

    //����Ʈ ���Ͱ� �׾����� �Ұ���ȯ�� _benStair���ӿ�����Ʈ���ֱ�, �ǵ����̹��� ����

    public void EliteMonsterDied()
    {
        _benStair.enabled = false;
    }

    

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (GameManager.Instance.StageClear)
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
}
