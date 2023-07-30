using UnityEngine;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.Burst.CompilerServices;
using System.Net;

public class Monster : MonoBehaviour
{
    MonsterType _type;
    public MonsterType Type { get { return _type; } }

    int _monsterHP = 0;
    int _monsterDamage = 0;
    int _turnCount;
    int _maxTrunCount;
    bool _aggro = false;
    Vector3 MonsterLook = Vector3.zero;

    void Start()
    {
        GameManager.Instance.MosterMoveEnvent += new EventHandler(MonsterMove);
    }

    public void Init(MonsterType Type) //몬스터 타입 별 기본값 세팅
    {
        _type = Type;
        _aggro = false;
        _turnCount = 0;

        switch (_type)
        {
            case MonsterType.Monster1: // 가만히 있고 체력1. 골드파밍용 몬스터
                _maxTrunCount = 0;
                _monsterHP = 1;
                _monsterDamage = 1;
                break;
            case MonsterType.Monster2: // 위아래로만 움직이고 체력2
                _maxTrunCount = 2;
                _monsterHP = 2;
                _monsterDamage = 1;
                MonsterLook = new Vector3(0, 1, 0);
                break; 
            case MonsterType.Monster3: // 좌우로만 움직이고 체력2
                _maxTrunCount = 2;
                _monsterHP = 2;
                _monsterDamage = 1;
                MonsterLook = new Vector3(1, 0, 0);
                break;
            case MonsterType.Monster4: // 턴에 진입했을때 공격준비 상태면 -> 공격 또는 이동 -> 공격준비해제
                                        //턴에 진입했을때 공격준비 상태가 아니면->공격준비
                _maxTrunCount = 1;
                _monsterHP = 2;
                _monsterDamage = 1;
                break;
            case MonsterType.EliteMonster:
                _maxTrunCount = 2;
                _monsterHP = 6;
                _monsterDamage = 2;
                break;
        }
    }

    void MonsterMove(object sender, EventArgs s)
    {
        if (_aggro == false)
        {
            //플레이어와의 거리 체크해서 거리가 5이하면 어그로를 켜준다
            if (Vector3.Distance(PlayerController.Instance.transform.position, transform.position) <= 5)
            {
                _aggro = true;
            }
        }
        else
        {
            if (_turnCount >= _maxTrunCount) //플레이어를 인식했고 턴
            {
                //행동
                switch (_type)
                {
                    case MonsterType.Monster1: //아무패턴도 존재하지 않음
                        break;
                    case MonsterType.Monster2:
                        MonsterPattern();
                        break;
                    case MonsterType.Monster3:
                        MonsterPattern();
                        break;
                    case MonsterType.Monster4:
                        MonsterPattern2();
                        break;
                    case MonsterType.EliteMonster:
                        EliteMonsterPattern();
                        break;
                }
                _turnCount = 0;
            }
            else
            {
                _turnCount++;
            }
        }
    }

    private void OnDrawGizmos()
    {
        float maxDistance = 0.5f;
        RaycastHit hit;
        // Physics.Raycast (레이저를 발사할 위치, 발사 방향, 충돌 결과, 최대 거리)
        bool isHit = Physics.Raycast(transform.position + MonsterLook/2, MonsterLook, out hit, maxDistance);

        Gizmos.color = Color.red;
        if (isHit)
        {
            Gizmos.DrawRay(transform.position + MonsterLook/2, MonsterLook * hit.distance);
        }
        else
        {
            Gizmos.DrawRay(transform.position + MonsterLook/2, MonsterLook * maxDistance);
        }
    }

    void MonsterPattern()
    {        
        //어느정도 해결은 했는데 서로 같은칸을 보고 동시에 비었다고 판단하고 움직이면 겹쳐버림
        
        //위아래로 또는 좌우로만 움직이고 체력2 - 이니셜라이즈에서 방향 정해짐
        Vector3 Temp = transform.position + MonsterLook/2;
        
        RaycastHit2D hitdata = Physics2D.Raycast(Temp, MonsterLook, 0.5f);
        
        if (hitdata)
        {
            //플레이어면 공격하고 아니면 뒤로돌아서 다시검사
            if (hitdata.collider.tag == "Player")
            {
                //플레이어면 공격
                PlayerController.Instance.NowHP -= _monsterDamage;
            }
            else
            {
                MonsterLook = MonsterLook * -1;
                hitdata = Physics2D.Raycast(Temp, MonsterLook, 1f);
                if(hitdata)
                {
                    if (hitdata.collider.tag == "Player")
                    {
                        //플레이어면 공격
                        PlayerController.Instance.NowHP -= _monsterDamage;
                    }
                    else
                    {
                        //행동종료
                    }
                }
                else
                {
                    monsterMove();
                }
            }
        }
        else
        {
            monsterMove();
        }
    }
    void monsterMove()
    {
        transform.position += MonsterLook;
        //GetComponent<SpriteRenderer>().sortingOrder = (int)(transform.position.y - 1) * -1;
    }

    void MonsterPattern2()
    {

    }

    void EliteMonsterPattern()
    {

    }

     public void TakeDamage(int damage)
    { 
        //체력체크
        _monsterHP -= damage;
        if (_monsterHP <= 0)
        {
            //사망
            gameObject.SetActive(false);
        }        
        //유아이 체력삭제 호출
    }
}

public enum MonsterType
{
    Monster1,
    Monster2,
    Monster3,
    Monster4,
    EliteMonster,
}
