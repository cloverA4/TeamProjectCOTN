using UnityEngine;
using System;
using Unity.VisualScripting;

public class Monster : MonoBehaviour
{
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

    public void Init(MonsterType Type) //���� Ÿ�� �� �⺻�� ����
    {
        _type = Type;
        _aggro = false;
        _turnCount = 0;

        switch (_type)
        {
            case MonsterType.Monster1: // ������ �ְ� ü��1. ����Ĺֿ� ����
                _maxTrunCount = 0;
                _monsterHP = 1;
                _monsterDamage = 1;
                break;
            case MonsterType.Monster2: // ���Ʒ��θ� �����̰� ü��2
                _maxTrunCount = 2;
                _monsterHP = 2;
                _monsterDamage = 1;
                MonsterLook = new Vector3(0, 1, 0);
                break; 
            case MonsterType.Monster3: // �¿�θ� �����̰� ü��2
                _maxTrunCount = 2;
                _monsterHP = 2;
                _monsterDamage = 1;
                MonsterLook = new Vector3(1, 0, 0);
                break;
            case MonsterType.Monster4: // �Ͽ� ���������� �����غ� ���¸� -> ���� �Ǵ� �̵� -> �����غ�����
                                        //�Ͽ� ���������� �����غ� ���°� �ƴϸ�->�����غ�
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
    }

    //void MonsterMove(object sender, EventArgs s)
    public void MonsterMove()
    {
        if (_aggro == false)
        {
            //�÷��̾���� �Ÿ� üũ�ؼ� �Ÿ��� 5���ϸ� ��׷θ� ���ش�
            if (Vector3.Distance(PlayerController.Instance.transform.position, transform.position) <= 5)
            {
                _aggro = true;
            }
        }
        else
        {
            if (_turnCount >= _maxTrunCount) //�÷��̾ �ν��߰� ��
            {
                //�ൿ
                switch (_type)
                {
                    case MonsterType.Monster1: //�ƹ����ϵ� �������� ����
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
        // Physics.Raycast (�������� �߻��� ��ġ, �߻� ����, �浹 ���, �ִ� �Ÿ�)
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
        //������� �ذ��� �ߴµ� ���� ����ĭ�� ���� ���ÿ� ����ٰ� �Ǵ��ϰ� �����̸� ���Ĺ���
        
        //���Ʒ��� �Ǵ� �¿�θ� �����̰� ü��2 - �̴ϼȶ������ ���� ������
        Vector3 Temp = transform.position + MonsterLook/2;        
        RaycastHit2D hitdata = Physics2D.Raycast(Temp, MonsterLook, 0.5f);
        
        if (hitdata)
        {
            //�÷��̾�� �����ϰ� �ƴϸ� �ڷε��� �� ��
            if (hitdata.collider.tag == "Player") PlayerController.Instance.NowHP -= _monsterDamage;//�÷��̾�� ����
            else
            {
                MonsterLook = MonsterLook * -1;
            }
        }
        else MoveMonster();
    }

    void MonsterPattern2()
    {
        //�ذ�����
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

    int _specialAttackCount = 0;
    bool _specialAttackReady = false;

    void EliteMonsterPattern()
    {
        Vector3 Temp;
        RaycastHit2D hitdata = new RaycastHit2D();

        _specialAttackCount++;

        if(_specialAttackReady)
        {
            //Ư������
            Temp = transform.position + MonsterLook / 2;
            hitdata = Physics2D.Raycast(Temp, MonsterLook, 100f, 1 << LayerMask.NameToLayer("Player"));

            if(hitdata)
            {
                if(hitdata.collider.CompareTag("Player")) PlayerController.Instance.NowHP -= _monsterDamage;
            }

            GetComponentsInChildren<SpriteRenderer>()[1].color = Color.white;
            _specialAttackCount = 0;
            _attackReady = false;
            return;
        }

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
                if (Vector2.Distance(transform.position, PlayerController.Instance.transform.position) > 1)
                {
                    if (_specialAttackCount >= 3) breathCharging();
                    else
                    {
                        if (transform.position.x > PlayerController.Instance.transform.position.x) action(Vector3.left);
                        else action(Vector3.right);
                    }
                }
                else
                {
                    if (transform.position.x > PlayerController.Instance.transform.position.x) action(Vector3.left);
                    else action(Vector3.right);
                }
            }
            else
            {
                if (_specialAttackCount >= 3 && Math.Abs(transform.position.y - PlayerController.Instance.transform.position.y) == 1)
                {
                    //y���� ���̰� 1�϶� y������ �����̸鼭 �극����¡
                    if (transform.position.y > PlayerController.Instance.transform.position.y) MoveMonsterY(Vector3.down);
                    else MoveMonsterY(Vector3.up);
                    breathCharging();
                }
                else if (PlayerController.Instance.IsX)
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
            if (PlayerController.Instance.transform.position.y == transform.position.y)
            {
                if (_specialAttackCount >= 3) breathCharging();
                else _attackReady = true;
            }
            else
            {
                _attackReady = true;
            }
        }
        //�������ݰ����ϸ� �극��ī���� �ʱ�ȭ �� ��������
        //�������� �Ұ����� �� �극�� �����ϸ� �극����¡
        //�������� �� �극�� �Ұ� �� �̵� y���� 1���̳� ��� y������ �̵��ϸ鼭 ��¡����(����ī��Ʈ 2�̻�)

        //�극��ī���ʹ� 3��° ��¡ 4��° �߻�
        //�̵� �� ������ 2�ϸ���
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
        //�극�� ��¡
        GetComponentsInChildren<SpriteRenderer>()[1].color = Color.red;
        _specialAttackReady = true;
    }


    #region ���� �̵��Լ�

    void MoveMonster()
    {
        transform.position += MonsterLook;
        //GetComponent<SpriteRenderer>().sortingOrder = (int)(transform.position.y - 1) * -1;
    }

    void MoveMonster2(Vector3 vec)
    {
        //�ش�������� ���̸� ��� �̵�, ��ֹ��� ������ ����
        MonsterLook = vec;
        Vector3 Temp = transform.position + MonsterLook / 2;
        RaycastHit2D hitdata = Physics2D.Raycast(Temp, MonsterLook, 0.5f);

        if (hitdata == false)
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
            //y�� ������ �ѹ��� �˻�
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
            //y�� ������ �ѹ��� �˻�
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
        //ü��üũ
        _monsterHP -= damage;
        if (_monsterHP <= 0)
        {
            //���
            gameObject.SetActive(false);
        }        
        //������ ü�»��� ȣ��
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
