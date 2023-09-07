using UnityEngine;
using System;

public class Monster : MonoBehaviour
{
    [SerializeField] MonsterHPUI _monsterHPUI;
    MonsterType _type;
    public MonsterType Type { get { return _type; } }

    int _monsterHP = 0;
    int _monsterDamage = 0;
    int _turnCount;
    int _maxTrunCount;
    bool _aggro = false;
    bool _attackReady;
    Vector3 MonsterLook = Vector3.zero;

    void Start()
    {
        //GameManager.Instance.MosterMoveEnvent += new EventHandler(MonsterMove);
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
                _maxTrunCount = 1;
                _monsterHP = 6;
                _monsterDamage = 2;
                break;
        }
        _monsterHPUI.Init(_monsterHP);
    }

    //void MonsterMove(object sender, EventArgs s)
    public void MonsterMove()
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
            //플레이어면 공격하고 아니면 뒤로돌고 턴 끝
            if (hitdata.collider.tag == "Player") PlayerController.Instance.NowHP -= _monsterDamage;//플레이어면 공격
            else if(hitdata.collider.tag == "Item") MoveMonster();
            else
            {
                MonsterLook = MonsterLook * -1;
            }
        }
        else MoveMonster();
    }

    void MonsterPattern2()
    {
        //해골패턴
        if (_attackReady)
        {
            Action<Vector3> action;            
            if (Vector2.Distance(transform.position, PlayerController.Instance.transform.position) > 1)
            {
                action = MoveMonster2;
            }
            else
            {
                action = AttackCheck;
            }

            if (transform.position.x == PlayerController.Instance.transform.position.x)
            {
                if (transform.position.y > PlayerController.Instance.transform.position.y) action(Vector3.down);
                else action(Vector3.up);
            }
            else if (transform.position.y == PlayerController.Instance.transform.position.y)
            {
                if (transform.position.x > PlayerController.Instance.transform.position.x) action(Vector3.left);
                else action(Vector3.right);
            }
            else
            {
                if (PlayerController.Instance.IsX)
                {
                    if (transform.position.x > PlayerController.Instance.transform.position.x) MoveMonsterX(Vector3.left);
                    else MoveMonsterX(Vector3.right);
                }
                else
                {
                    if (transform.position.y > PlayerController.Instance.transform.position.y) MoveMonsterY(Vector3.down);
                    else MoveMonsterY(Vector3.up);
                }                
            }

            GetComponentsInChildren<SpriteRenderer>()[1].color = Color.white;
            _attackReady = false;
        }
        else
        {
            GetComponentsInChildren<SpriteRenderer>()[1].color = Color.red;
            _attackReady = true;
        }
    }
        

    void AttackCheck(Vector3 vec)
    {
        MonsterLook = vec;
        Vector3 Temp = transform.position + MonsterLook / 2;
        RaycastHit2D hitdata = Physics2D.Raycast(Temp, MonsterLook, 0.5f);

        if (hitdata)
        {
            if (hitdata.collider.tag == "Player") PlayerController.Instance.NowHP -= _monsterDamage;
        }
    }

    int _attackMoveCount = 0;
    int _specialAttackCount = 0;

    void EliteMonsterPattern()
    {
        Vector3 Temp;
        RaycastHit2D hitdata = new RaycastHit2D();

        _specialAttackCount++;

        if(_attackReady)
        {
            //특수공격

            Temp = transform.position + MonsterLook / 2;
            hitdata = Physics2D.Raycast(Temp, MonsterLook, 100f, 1 << LayerMask.NameToLayer("Player"));

            if(hitdata)
            {
                if(hitdata.collider.CompareTag("Player")) PlayerController.Instance.NowHP -= _monsterDamage;
            }

            GetComponentsInChildren<SpriteRenderer>()[1].color = Color.white;
            _specialAttackCount = 0;
            _attackMoveCount = 0;
            _attackReady = false;
            return;
        }
        
        if (_attackMoveCount >= 1)
        {
            if(Vector2.Distance(transform.position, PlayerController.Instance.transform.position) < 1)
            {                
                //공격
                if (PlayerController.Instance.transform.position.x == transform.position.x)
                {
                    if(PlayerController.Instance.transform.position.y > transform.position.y)
                    {
                        //위에있음
                        Temp = transform.position + Vector3.up / 2;
                        hitdata = Physics2D.Raycast(Temp, Vector3.up, 0.5f);
                    }
                    else
                    {
                        //아래있음
                        Temp = transform.position + Vector3.down / 2;
                        hitdata = Physics2D.Raycast(Temp, Vector3.down, 0.5f);
                    }
                }
                else if(PlayerController.Instance.transform.position.y == transform.position.y)
                {
                    if (PlayerController.Instance.transform.position.x > transform.position.x)
                    {
                        //오른쪽에 있음
                        Temp = transform.position + Vector3.right / 2;
                        hitdata = Physics2D.Raycast(Temp, Vector3.right, 0.5f);
                    }
                    else
                    {
                        //왼쪽에 있음
                        Temp = transform.position + Vector3.left / 2;
                        hitdata = Physics2D.Raycast(Temp, Vector3.left, 0.5f);
                    }
                }

                if(hitdata)
                {
                    if(hitdata.collider.CompareTag("Player"))
                    {
                        PlayerController.Instance.NowHP -= _monsterDamage;
                    }
                }
                else
                {
                    Debug.Log("플레이어까지 거리가 1미만인데 상하좌우에 없다????");
                }
                _specialAttackCount = 0;
                _attackMoveCount = 0;
            }
            else
            {
                //이동
                if (PlayerController.Instance.transform.position.y == transform.position.y)
                {
                    if (_specialAttackCount >= 3) breathCharging();
                    else EliteMonsterMove(0);
                }
                else
                {
                    if(Math.Abs(PlayerController.Instance.transform.position.y - transform.position.y) == 1)
                    {
                        if (_specialAttackCount >= 3)
                        {
                            EliteMonsterMove(1);
                            breathCharging();
                        }
                        else EliteMonsterMove(0);
                    }
                    else EliteMonsterMove(1);
                }
            }
        }
        else
        {
            if (PlayerController.Instance.transform.position.y == transform.position.y)
            {
                if (_specialAttackCount >= 3) breathCharging();
            }
            _attackMoveCount++;
        }
        //근접공격가능하면 브레스카운터 초기화 및 근접공격
        //근접공격 불가능할 시 브레스 가능하면 브레스차징
        //근접공격 및 브레스 불가 시 이동 y값이 1차이날 경우 y축으로 이동하면서 차징까지(차지카운트 2이상)

        //브레스카운터는 3턴째 차징 4턴째 발사
        //이동 및 공격은 2턴마다
    }
    void EliteMonsterMove(int type)
    {
        Vector3 Temp;
        Vector3 vec;
        RaycastHit2D hitdata = new RaycastHit2D();
        if (type == 0)
        {
            //x축이동
            if (PlayerController.Instance.transform.position.x > transform.position.x)
            {
                Temp = transform.position + Vector3.right / 2;
                hitdata = Physics2D.Raycast(Temp, Vector3.right, 0.5f);
                vec = Vector3.right;
            }
            else
            {
                Temp = transform.position + Vector3.left / 2;
                hitdata = Physics2D.Raycast(Temp, Vector3.left, 0.5f);
                vec = Vector3.left;
            }

            if (!hitdata)
            {
                transform.position += vec;
            }
        }
        else if(type == 1)
        {
            //y축이동
            if (PlayerController.Instance.transform.position.y > transform.position.y)
            {
                Temp = transform.position + Vector3.up / 2;
                hitdata = Physics2D.Raycast(Temp, Vector3.up, 0.5f);
                vec = Vector3.up;
            }
            else
            {
                Temp = transform.position + Vector3.down / 2;
                hitdata = Physics2D.Raycast(Temp, Vector3.down, 0.5f);
                vec = Vector3.down;
            }

            if (!hitdata)
            {
                transform.position += vec;
            }
        }

    }

    void breathCharging()
    {
        if(PlayerController.Instance.transform.position.x > transform.position.x)
        {
            MonsterLook = Vector3.right;
        }
        else
        {
            MonsterLook = Vector3.left;
        }
        //브레스 차징
        GetComponentsInChildren<SpriteRenderer>()[1].color = Color.red;
        _attackReady = true;
    }


    #region 몬스터 이동함수

    void MoveMonster()
    {
        transform.position += MonsterLook;
        //GetComponent<SpriteRenderer>().sortingOrder = (int)(transform.position.y - 1) * -1;
    }

    void MoveMonster2(Vector3 vec)
    {
        //해당방향으로 레이를 쏘고 이동, 장애물이 있으면 종료
        MonsterLook = vec;
        Vector3 Temp = transform.position + MonsterLook / 2;
        RaycastHit2D hitdata = Physics2D.Raycast(Temp, MonsterLook, 0.5f);

        if (hitdata == false)
        {
            MoveMonster();
        }
        else if(hitdata.collider.tag == "Item")
        {
            MoveMonster();
        }
    }

    void MoveMonsterX(Vector3 vec)
    {
        MonsterLook = vec;
        Vector3 Temp = transform.position + MonsterLook / 2;
        RaycastHit2D hitdata = Physics2D.Raycast(Temp, MonsterLook, 0.5f);

        if (hitdata)
        {
            //y값 쪽으로 한번더 검사
            if (transform.position.y > PlayerController.Instance.transform.position.y)
            {
                MoveMonster2(Vector3.down);
            }
            else
            {
                MoveMonster2(Vector3.up);
            }
        }
        else
        {
            MoveMonster();
        }
    }

    void MoveMonsterY(Vector3 vec)
    {
        MonsterLook = vec;
        Vector3 Temp = transform.position + MonsterLook / 2;
        RaycastHit2D hitdata = Physics2D.Raycast(Temp, MonsterLook, 0.5f);

        if (hitdata)
        {
            //y값 쪽으로 한번더 검사
            if (transform.position.x > PlayerController.Instance.transform.position.x)
            {
                MoveMonster2(Vector3.left);
            }
            else
            {
                MoveMonster2(Vector3.right);
            }
        }
        else
        {
            MoveMonster();
        }
    }

    #endregion


    public void TakeDamage(int damage)
    { 
        //체력체크
        _monsterHP -= damage;
        _monsterHPUI.MonsterHPUpdata(_monsterHP);
        if (_monsterHP <= 0)
        {
            //사망
            ItemDrop();
            gameObject.SetActive(false);
            if(Type == MonsterType.EliteMonster)
            {
                GameManager.Instance.EliteMonsterDie();
            }
        }        
        //유아이 체력삭제 호출
    }

    void ItemDrop()
    {
        GameObject go = Instantiate(Data.Instance.ItemPrefab, GameManager.Instance.ItemPool.transform);
        go.transform.position = transform.position;
        int dropCount = UnityEngine.Random.Range(3, 5);

        Currency cr = (Currency)Data.Instance.GetItemInfo(102);
        cr.Count = dropCount;
        go.GetComponent<DropItem>().Init(cr);
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
