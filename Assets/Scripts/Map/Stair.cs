using System;
using UnityEngine;
using UnityEngine.UI;

public class Stair : MonoBehaviour
{
    [SerializeField]
    private Stage _nowStage;
    [SerializeField]
    private floor _nowFloor;
    [SerializeField]
    SpriteRenderer _benStair;
    [SerializeField] Text _text;

    private void Start()
    {
        //_benStair = GetComponentsInChildren<SpriteRenderer>()[1];
        GameManager.Instance.EventEliteMonsterDie += new EventHandler(EliteMonsterDied);
        _text.gameObject.SetActive(false);
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
    public void SetText(string str)  // ��� �ؽ�Ʈ ���
    {
        _text.text = str;
        _text.gameObject.SetActive(true);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (GameManager.Instance.StageClear == true)
            {
                if(GameManager.Instance.NowStage == Stage.Lobby && _nowStage == Stage.Stage1)
                {
                    GameManager.Instance.Dia = 0;
                }
                GameManager.Instance.NowStage = _nowStage;
                GameManager.Instance.NowFloor = _nowFloor;
                GameManager.Instance.FaidIn();
            }
            else
            {
                //uiȣ�� "����Ʈ ���͸� óġ�ؾ��մϴ�!"
                SetText("����Ʈ ���͸� óġ�ϼ���!");
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