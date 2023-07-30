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
            if (hitdata.collider.tag == "Player")
            {
                //�÷��̾�� ����
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
                        //�÷��̾�� ����
                        PlayerController.Instance.NowHP -= _monsterDamage;
                    }
                    else
                    {
                        //�ൿ����
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
