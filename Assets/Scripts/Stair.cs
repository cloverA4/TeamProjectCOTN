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

    bool _eliteMonsterLive = true;

    private void Start()
    {
        _benStair = GetComponentsInChildren<SpriteRenderer>()[1];
    }

    //����Ʈ ���Ͱ� ��������� üũ�ϴ� �Լ� �������� �̵������� ȣ��

    public void EliteMonsterLive()
    {
        _eliteMonsterLive = true;
        _benStair.enabled = true;
    }

    //����Ʈ ���Ͱ� �׾����� �Ұ���ȯ�� _benStair���ӿ�����Ʈ���ֱ�, �ǵ����̹��� ����

    public void EliteMonsterDied()
    {
        _eliteMonsterLive = false;
        _benStair.enabled = false;
    }

    

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (_eliteMonsterLive)
            {
                //uiȣ�� "����Ʈ ���͸� óġ�ؾ��մϴ�!"
            }
            else
            {
                GameManager.Instance.NowStage = _nowStage;
                GameManager.Instance.NowFloor = _nowFloor;
                GameManager.Instance.FaidIn();
            }
        }
    }
}
