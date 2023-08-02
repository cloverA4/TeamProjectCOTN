using UnityEngine;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.Burst.CompilerServices;
using System.Net;
using JetBrains.Annotations;

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
        GameManager.Instance.MosterMoveEnvent += new EventHandler(MonsterMove);
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

    void MonsterMove(object sender, EventArgs s)
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
            //�÷��̾�� �����ϰ� �ƴϸ� �ڷε��Ƽ� �ٽð˻�
            if (hitdata.collider.tag == "Player") PlayerController.Instance.NowHP -= _monsterDamage;//�÷��̾�� ����
            else
            {
                MonsterLook = MonsterLook * -1;
                hitdata = Physics2D.Raycast(Temp, MonsterLook, 0.5f);
                if(hitdata)
                {
                    if (hitdata.collider.tag == "Player") PlayerController.Instance.NowHP -= _monsterDamage;//�÷��̾�� ����
                }
                else monsterMove();
            }
        }
        else monsterMove();
    }
    void monsterMove()
    {
        transform.position += MonsterLook;
        //GetComponent<SpriteRenderer>().sortingOrder = (int)(transform.position.y - 1) * -1;
    }
    
    void MonsterPattern2()
    {
        //�ذ�����
        if (_attackReady)
        {
            GetComponent<SpriteRenderer>().color = Color.white;
            if (Vector2.Distance(transform.position, PlayerController.Instance.transform.position) > 1)
            {
                //�̵�
                if (transform.position.x == PlayerController.Instance.transform.position.x)
                {
                    //y�� �̵�
                    if (transform.position.y > PlayerController.Instance.transform.position.y)
                    {
                        MoveCheck(Vector3.down);
                    }
                    else
                    {
                        MoveCheck(Vector3.up);
                    }
                }
                else if (transform.position.x > PlayerController.Instance.transform.position.x)
                {
                    MoveCheck(Vector3.left);
                }
                else if (transform.position.x < PlayerController.Instance.transform.position.x)
                {
                    MoveCheck(Vector3.right);
                }
            }
            else
            {
                //����
                if (transform.position.x == PlayerController.Instance.transform.position.x) AttackCheck(Vector3.up);
                else AttackCheck(Vector3.left);
            }
            _attackReady = false;
        }
        else
        {
            GetComponent<SpriteRenderer>().color = Color.red;
            _attackReady = true;
        }
    }

    void MoveCheck(Vector3 vec)
    {
        Vector3 Temp = transform.position + vec / 2;
        RaycastHit2D hitdata = Physics2D.Raycast(Temp, vec, 0.5f);

        if (vec == Vector3.left || vec == Vector3.right)
        {
            if (hitdata)
            {
                hitdata = Physics2D.Raycast(Temp, Vector3.up, 0.5f);

                if (hitdata)
                {
                    hitdata = Physics2D.Raycast(Temp, Vector3.down, 0.5f);

                    if (hitdata) transform.position += vec * -1;
                    else transform.position += Vector3.down;
                }
                else transform.position += Vector3.up;
            }
            else transform.position += vec;
        }
        else if(vec == Vector3.up || vec == Vector3.down)
        {
            if (hitdata)
            {
                hitdata = Physics2D.Raycast(Temp, Vector3.left, 0.5f);

                if (hitdata)
                {
                    hitdata = Physics2D.Raycast(Temp, Vector3.right, 0.5f);

                    if (hitdata) transform.position += vec * -1;
                    else transform.position += Vector3.right;
                }
                else transform.position += Vector3.left;
            }
            else transform.position += vec;
        }
    }

    void AttackCheck(Vector3 vec)
    {
        Vector3 Temp = transform.position + vec / 2;
        RaycastHit2D hitdata = Physics2D.Raycast(Temp, vec, 0.5f);

        if (hitdata)
        {
            if (hitdata.collider.tag == "Player") PlayerController.Instance.NowHP -= _monsterDamage;
            else
            {
                //�÷��̾ �ƴ� �ٸ��� �ִ�. �׷� ������ üũ
                hitdata = Physics2D.Raycast(Temp, vec * -1, 0.5f);
                if (hitdata)
                {
                    if (hitdata.collider.tag == "Player") PlayerController.Instance.NowHP -= _monsterDamage;
                }
            }
        }
        else
        {
            //�Ÿ��� 1�ȿ��ִµ� x���� �ٸ���? �ٵ� ���ʿ� ����. �׷� ������ üũ
            hitdata = Physics2D.Raycast(Temp, vec * -1, 0.5f);
            if (hitdata)
            {
                if (hitdata.collider.tag == "Player") PlayerController.Instance.NowHP -= _monsterDamage;
            }
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
            //Ư������

            Temp = transform.position + MonsterLook / 2;
            hitdata = Physics2D.Raycast(Temp, MonsterLook, 100f, 1 << LayerMask.NameToLayer("Player"));

            if(hitdata)
            {
                if(hitdata.collider.CompareTag("Player")) PlayerController.Instance.NowHP -= _monsterDamage;
            }

            GetComponent<SpriteRenderer>().color = Color.white;
            _specialAttackCount = 0;
            _attackMoveCount = 0;
            _attackReady = false;
            return;
        }
        
        if (_attackMoveCount >= 1)
        {
            if(Vector2.Distance(transform.position, PlayerController.Instance.transform.position) < 1)
            {                
                //����
                if (PlayerController.Instance.transform.position.x == transform.position.x)
                {
                    if(PlayerController.Instance.transform.position.y > transform.position.y)
                    {
                        //��������
                        Temp = transform.position + Vector3.up / 2;
                        hitdata = Physics2D.Raycast(Temp, Vector3.up, 0.5f);
                    }
                    else
                    {
                        //�Ʒ�����
                        Temp = transform.position + Vector3.down / 2;
                        hitdata = Physics2D.Raycast(Temp, Vector3.down, 0.5f);
                    }
                }
                else if(PlayerController.Instance.transform.position.y == transform.position.y)
                {
                    if (PlayerController.Instance.transform.position.x > transform.position.x)
                    {
                        //�����ʿ� ����
                        Temp = transform.position + Vector3.right / 2;
                        hitdata = Physics2D.Raycast(Temp, Vector3.right, 0.5f);
                    }
                    else
                    {
                        //���ʿ� ����
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
                    Debug.Log("�÷��̾���� �Ÿ��� 1�̸��ε� �����¿쿡 ����????");
                }
                _specialAttackCount = 0;
                _attackMoveCount = 0;
            }
            else
            {
                //�̵�
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
        //�������ݰ����ϸ� �극��ī���� �ʱ�ȭ �� ��������
        //�������� �Ұ����� �� �극�� �����ϸ� �극����¡
        //�������� �� �극�� �Ұ� �� �̵� y���� 1���̳� ��� y������ �̵��ϸ鼭 ��¡����(����ī��Ʈ 2�̻�)

        //�극��ī���ʹ� 3��° ��¡ 4��° �߻�
        //�̵� �� ������ 2�ϸ���
    }
    void EliteMonsterMove(int type)
    {
        Vector3 Temp;
        Vector3 vec;
        RaycastHit2D hitdata = new RaycastHit2D();
        if (type == 0)
        {
            //x���̵�
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
            //y���̵�
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
        //�극�� ��¡
        GetComponent<SpriteRenderer>().color = Color.red;
        _attackReady = true;
    }

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
