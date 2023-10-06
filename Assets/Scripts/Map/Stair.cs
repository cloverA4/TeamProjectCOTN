using System;
using UnityEngine;
using UnityEngine.UI;

public class Stair : MonoBehaviour
{
    [SerializeField] private Stage _nowStage;
    [SerializeField] private floor _nowFloor;
    [SerializeField] SpriteRenderer _benStair;
    [SerializeField] Text _text;

    private void Start()
    {
        GameManager.Instance.EventEliteMonsterDie += new EventHandler(EliteMonsterDied);
        _text.gameObject.SetActive(false);
    }

    public void StageLock()
    {
        _benStair.enabled = !GameManager.Instance.StageClear;
    }

    public void EliteMonsterDied(object sender, EventArgs s)
    {
        StageLock();
    }

    public void SetText(string str)  // 계단 텍스트 출력
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
                SetText("엘리트 몬스터를 처치하세요!");
            }
        }
    }
}
