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

    //엘리트 몬스터가 살아있을때 체크하는 함수 스테이지 이동때마다 호출

    public void EliteMonsterLive()
    {
        _benStair.enabled = true;
    }

    //엘리트 몬스터가 죽었을때 불값변환후 _benStair게임오브젝트꺼주기, 의도는이미지 삭제

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
                //ui호출 "엘리트 몬스터를 처치해야합니다!"
            }
        }
    }
}
