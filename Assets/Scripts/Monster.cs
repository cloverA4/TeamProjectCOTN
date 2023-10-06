using UnityEngine;
using System;
using Unity.Burst.CompilerServices;

public class Monster : MonoBehaviour
{
    AudioSource _audio;
    [SerializeField] MonsterHPUI _monsterHPUI;
    [SerializeField] GameObject _monsterNormalAttackEffect;

    MonsterType _type;
    public MonsterType Type { get { return _type; } }

    int _monsterHP = 0;
    int _monsterDamage = 0;
    int _turnCount;
    int _maxTrunCount;
    bool _aggro = false;
    bool _attackReady;
    Vector3 MonsterLook = Vector3.zero;
    LayerMask _normalLayerMask;

    Animator _animator;
    SpriteRenderer _childSpriteRenderer;

    void Start()
    {
        UIManeger.Instance.EventVolumeChange += new EventHandler(VolumeChange);
        //GameManager.Instance.MosterMoveEnvent += new EventHandler(MonsterMove);
        _audio = GetComponent<AudioSource>();
        _animator = GetComponentsInChildren<Animator>()[0];
        _childSpriteRenderer = GetComponentsInChildren<SpriteRenderer>()[1];

        _normalLayerMask =
                  (1 << LayerMask.NameToLayer("Wall")) |
                  (1 << LayerMask.NameToLayer("Npc")) |
                  (1 << LayerMask.NameToLayer("Monster")) |
                  (1 << LayerMask.NameToLayer("Player"));
    }

    public void VolumeChange(object sender, EventArgs s)
    {
        _audio.volume = UIManeger.Instance.EffectVolume;
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
                _monsterDamage = 2;
                break;
            case MonsterType.Monster2: // ���Ʒ��θ� �����̰� ü��2
                _maxTrunCount = 2;
                _monsterHP = 2;
                _monsterDamage = 2;
                MonsterLook = new Vector3(0, 1, 0);
                break; 
            case MonsterType.Monster3: // �¿�θ� �����̰� ü��2
                _maxTrunCount = 2;
                _monsterHP = 2;
                _monsterDamage = 2;
                MonsterLook = new Vector3(1, 0, 0);
                break;
            case MonsterType.Monster4: // �Ͽ� ���������� �����غ� ���¸� -> ���� �Ǵ� �̵� -> �����غ�����
                                        //�Ͽ� ���������� �����غ� ���°� �ƴϸ�->�����غ�
                _maxTrunCount = 1;
                _monsterHP = 2;
                _monsterDamage = 2;
                break;
            case MonsterType.EliteMonster:
                _maxTrunCount = 1;
                _monsterHP = 6;
                _monsterDamage = 4;
                break;
        }
        _monsterHPUI.Init(_monsterHP);
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
                        _childSpriteRenderer.sortingOrder = (int)(transform.position.y - 1) * -1;
                        if (PlayerController.Instance.gameObject.transform.position.x > gameObject.transform.position.x){
                            _childSpriteRenderer.flipX = true;
                        }
                        else{
                            _childSpriteRenderer.flipX = false;
                        }
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
        bool isHit = Physics.Raycast(transform.position + MonsterLook/2, MonsterLook, out hit, maxDistance, _normalLayerMask);

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
        RaycastHit2D hitdata = Physics2D.Raycast(Temp, MonsterLook, 0.5f, _normalLayerMask);
        
        if (hitdata)
        {
            //�÷��̾�� �����ϰ� �ƴϸ� �ڷε��� �� ��
            if (hitdata.collider.tag == "Player")
            {
                MonsterAttackEffectPos(MonsterLook);
                MonsterAttack();//�÷��̾�� ����
            }
            else
            {
                MonsterLook = MonsterLook * -1;
                _childSpriteRenderer.flipX = !_childSpriteRenderer.flipX;
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
                if (transform.position.y > PlayerController.Instance.transform.position.y)
                {
                    action(Vector3.down);
                }
                else
                {
                    action(Vector3.up);
                }
            }
            else if (transform.position.y == PlayerController.Instance.transform.position.y)
            {
                if (transform.position.x > PlayerController.Instance.transform.position.x)
                {
                    action(Vector3.left);
                    _childSpriteRenderer.flipX = false;
                }
                else
                {
                    action(Vector3.right);
                    _childSpriteRenderer.flipX = true;
                }
            }
            else
            {
                if (PlayerController.Instance.IsX)
                {
                    if (transform.position.x > PlayerController.Instance.transform.position.x)
                    {
                        MoveMonsterX(Vector3.left);
                        _childSpriteRenderer.flipX = false;
                    }
                    else 
                    {
                        MoveMonsterX(Vector3.right);
                        _childSpriteRenderer.flipX = true;
                    }
                }
                else
                {
                    if (transform.position.y > PlayerController.Instance.transform.position.y) MoveMonsterY(Vector3.down);
                    else MoveMonsterY(Vector3.up);
                }                
            }

            //GetComponentsInChildren<SpriteRenderer>()[1].color = Color.white;
            _animator.SetTrigger("Idle");
            _attackReady = false;
        }
        else
        {
            //GetComponentsInChildren<SpriteRenderer>()[1].color = Color.red;
            _animator.SetTrigger("AttackMotion");
            _attackReady = true;
        }
    }
        

    void AttackCheck(Vector3 vec)
    {
        MonsterLook = vec;
        Vector3 Temp = transform.position + MonsterLook / 2;
        RaycastHit2D hitdata = Physics2D.Raycast(Temp, MonsterLook, 0.5f, _normalLayerMask);

        if (hitdata)
        {
            if (hitdata.collider.tag == "Player")
            {
                MonsterAttackEffectPos(vec);
                MonsterAttack();
            }
        }

        if (transform.position.x > PlayerController.Instance.transform.position.x) _childSpriteRenderer.flipX = false;
        else _childSpriteRenderer.flipX = true;
    }

    void MonsterAttackEffectPos(Vector3 vec)
    {
        if (_monsterNormalAttackEffect != null)
        {
            GameObject NormalAttack = null;

            if (vec == Vector3.up)
            {
                NormalAttack = Instantiate(_monsterNormalAttackEffect, transform.position + new Vector3(0, 0.8f, 0), Quaternion.Euler(0f, 0f, 90f));
            }
            else if (vec == Vector3.down)
            {
                NormalAttack = Instantiate(_monsterNormalAttackEffect, transform.position + new Vector3(0, -0.8f, 0), Quaternion.Euler(0f, 0f, -90f));
            }
            else if (vec == Vector3.left)
            {
                NormalAttack = Instantiate(_monsterNormalAttackEffect, transform.position + new Vector3(-0.8f, 0.2f, 0), Quaternion.identity);
                NormalAttack.GetComponent<SpriteRenderer>().flipX = true;
            }
            else if (vec == Vector3.right)
            {
                NormalAttack = Instantiate(_monsterNormalAttackEffect, transform.position + new Vector3(0.8f, 0.2f, 0), Quaternion.identity);
            }
            Destroy(NormalAttack, 0.2f);
        }
    }
    int _attackMoveCount = 0;
    int _specialAttackCount = 0;
    [SerializeField]
    private GameObject _ElitemonsterAttackEffect;

    void EliteMonsterPattern()
    {
        Vector3 Temp;
        RaycastHit2D hitdata = new RaycastHit2D();
        
        _specialAttackCount++;

        if(_attackReady)
        {
            //Ư������
            hitdata = Physics2D.Raycast(gameObject.transform.position, MonsterLook, 100f, (1 << LayerMask.NameToLayer("Wall")));

            if(hitdata)
            {
                Vector3 newXPosition = new Vector3((hitdata.distance - 1) / 2, 0, 0);
                if (MonsterLook == Vector3.left) newXPosition = -newXPosition + MonsterLook;
                else if (MonsterLook == Vector3.right) newXPosition = newXPosition + MonsterLook;

                Vector3 CenterPoint = transform.position + newXPosition;

                Collider2D[] colliders = Physics2D.OverlapBoxAll(CenterPoint, new Vector3(hitdata.distance - 1, 1, 1), 0f, (1 << LayerMask.NameToLayer("Monster")) | (1 << LayerMask.NameToLayer("Player")));
                foreach (Collider2D collider in colliders)
                {
                    if (collider.CompareTag("Player")) MonsterAttack();
                    else if(collider.CompareTag("Monster"))
                    {
                        collider.GetComponent<Monster>().TakeDamage(_monsterDamage);
                    }
                }
            }

            GameObject SpecialEffect = Instantiate(_ElitemonsterAttackEffect, transform.position + MonsterLook, Quaternion.identity);
            SpecialEffect.GetComponent<EliteMonsterThrowDagger>().Init(transform.position + MonsterLook , MonsterLook);

            //GetComponentsInChildren<SpriteRenderer>()[1].color = Color.white;
            _animator.SetTrigger("Idle"); // Ư����ǿ��� �⺻������� ���ƿ�
            _specialAttackCount = 0;
            _attackMoveCount = 0;
            _attackReady = false;
            return;
        }
        
        if (_attackMoveCount >= 1)
        {
            if(Vector2.Distance(transform.position, PlayerController.Instance.transform.position) <= 1)
            {
                //����
                if (PlayerController.Instance.transform.position.x == transform.position.x)
                {
                    if(PlayerController.Instance.transform.position.y > transform.position.y)
                    {
                        //��������
                        Temp = transform.position + Vector3.up / 2;
                        hitdata = Physics2D.Raycast(Temp, Vector3.up, 0.5f, _normalLayerMask);
                    }
                    else
                    {
                        //�Ʒ�����
                        Temp = transform.position + Vector3.down / 2;
                        hitdata = Physics2D.Raycast(Temp, Vector3.down, 0.5f, _normalLayerMask);
                    }
                }
                else if(PlayerController.Instance.transform.position.y == transform.position.y)
                {
                    if (PlayerController.Instance.transform.position.x > transform.position.x)
                    {
                        //�����ʿ� ����
                        Temp = transform.position + Vector3.right / 2;
                        hitdata = Physics2D.Raycast(Temp, Vector3.right, 0.5f, _normalLayerMask);
                    }
                    else
                    {
                        //���ʿ� ����
                        Temp = transform.position + Vector3.left / 2;
                        hitdata = Physics2D.Raycast(Temp, Vector3.left, 0.5f, _normalLayerMask);
                    }
                }

                if(hitdata)
                {
                    if(hitdata.collider.CompareTag("Player"))
                    {
                        if (MonsterLook == Vector3.up)
                        {
                            _animator.SetTrigger("AttackUp");
                        }
                        else if (MonsterLook == Vector3.down)
                        {
                            _animator.SetTrigger("AttackDown");
                        }
                        else if (MonsterLook == Vector3.right)
                        {
                            _animator.SetTrigger("AttackRight");
                        }
                        else if (MonsterLook == Vector3.left)
                        {
                            _animator.SetTrigger("AttackLefts");
                        }
                        MonsterAttack();
                    }

                    
                }
                else
                {
                    Debug.Log("�÷��̾���� �Ÿ��� 1�̸��ε� �����¿쿡 ����????");
                }
                _specialAttackCount = 0;
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
            _attackMoveCount = 0;
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
            _childSpriteRenderer.sortingOrder = (int)(transform.position.y - 1) * -1;
            //x���̵�
            if (PlayerController.Instance.transform.position.x > transform.position.x)
            {
                Temp = transform.position + Vector3.right / 2;
                hitdata = Physics2D.Raycast(Temp, Vector3.right, 0.5f, _normalLayerMask);
                vec = Vector3.right;
                _animator.SetTrigger("Right");
                _childSpriteRenderer.flipX = true;
            }
            else
            {
                Temp = transform.position + Vector3.left / 2;
                hitdata = Physics2D.Raycast(Temp, Vector3.left, 0.5f, _normalLayerMask);
                vec = Vector3.left;
                _animator.SetTrigger("Left");
                _childSpriteRenderer.flipX = false;
            }

            if (!hitdata)
            {
                transform.position += vec;

                if (vec == Vector3.right)
                {
                    _animator.SetTrigger("Right");
                    _childSpriteRenderer.flipX = true;
                }
                else if (vec == Vector3.left)
                {
                    _animator.SetTrigger("Left");
                    _childSpriteRenderer.flipX = false;
                }
            }
        }
        else if(type == 1)
        {
            //y���̵�
            if (PlayerController.Instance.transform.position.y > transform.position.y)
            {
                Temp = transform.position + Vector3.up / 2;
                hitdata = Physics2D.Raycast(Temp, Vector3.up, 0.5f, _normalLayerMask);
                vec = Vector3.up;
                _animator.SetTrigger("Up");
            }
            else
            {
                Temp = transform.position + Vector3.down / 2;
                hitdata = Physics2D.Raycast(Temp, Vector3.down, 0.5f, _normalLayerMask);
                vec = Vector3.down;
                _animator.SetTrigger("Down");
            }

            if (!hitdata)
            {
                transform.position += vec;

                if (vec == Vector3.up)
                {
                    _animator.SetTrigger("Up");
                }
                else if (vec == Vector3.down)
                {
                    _animator.SetTrigger("Down");
                }
            }
        }
    }

    void breathCharging()
    {
        if(PlayerController.Instance.transform.position.x > transform.position.x)
        {
            MonsterLook = Vector3.right;
            _childSpriteRenderer.flipX = true;
        }
        else
        {
            MonsterLook = Vector3.left;
            _childSpriteRenderer.flipX = false;
        }
        //�극�� ��¡
        //GetComponentsInChildren<SpriteRenderer>()[1].color = Color.red;
        _animator.SetTrigger("AttackSpecial"); // Ư�����ݸ��
        _attackReady = true;
    }


    #region ���� �̵��Լ�

    void MoveMonster()
    {
        transform.position += MonsterLook;
        if (MonsterLook == Vector3.right)
        {
            _animator.SetTrigger("Right");
            _childSpriteRenderer.flipX = true;
        }
        else if (MonsterLook == Vector3.left)
        {
            _animator.SetTrigger("Left");
            _childSpriteRenderer.flipX = false;
        }
        else if (MonsterLook == Vector3.up)
        {
            _animator.SetTrigger("Up");
        }
        else if (MonsterLook == Vector3.down)
        {
            _animator.SetTrigger("Down");
        }
        // �ڽĽ�������Ʈ Layer�� �̵��� �ٲٱ�
        _childSpriteRenderer.sortingOrder = (int)(transform.position.y - 1) * -1;
    }

    void MoveMonster2(Vector3 vec)
    {
        //�ش�������� ���̸� ��� �̵�, ��ֹ��� ������ ����
        MonsterLook = vec;
        Vector3 Temp = transform.position + MonsterLook / 2;
        RaycastHit2D hitdata = Physics2D.Raycast(Temp, MonsterLook, 0.5f, _normalLayerMask);
        _childSpriteRenderer.sortingOrder = (int)(transform.position.y - 1) * -1;

        if (hitdata == false)
        {
            MoveMonster();
        }
    }

    void MoveMonsterX(Vector3 vec)
    {
        MonsterLook = vec;
        Vector3 Temp = transform.position + MonsterLook / 2;
        RaycastHit2D hitdata = Physics2D.Raycast(Temp, MonsterLook, 0.5f, _normalLayerMask);

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
        RaycastHit2D hitdata = Physics2D.Raycast(Temp, MonsterLook, 0.5f, _normalLayerMask);

        if (hitdata)
        {
            //x�� ������ �ѹ��� �˻�
            if (transform.position.x > PlayerController.Instance.transform.position.x)
            {
                MoveMonster2(Vector3.left);
                _childSpriteRenderer.flipX = false;
            }
            else
            {
                MoveMonster2(Vector3.right);
                _childSpriteRenderer.flipX = true;
            }
        }
        else
        {
            MoveMonster();
        }
    }

    #endregion

    void MonsterAttack()
    {
        PlayerController.Instance.TakeDamage(_monsterDamage);
        switch (Type)
        {
            case MonsterType.Monster2:
                _audio.clip = Data.Instance.SoundEffect[(int)SoundEffect.ZombieAttack];
                break;
            case MonsterType.Monster3:
                _audio.clip = Data.Instance.SoundEffect[(int)SoundEffect.CowAttack];
                break;
            case MonsterType.Monster4:
                _audio.clip = Data.Instance.SoundEffect[(int)SoundEffect.GoblinAttack];
                break;
            case MonsterType.EliteMonster:
                _audio.clip = Data.Instance.SoundEffect[(int)SoundEffect.EliteGoblinAttack];
                break;
        }
        _audio.Play();
    }


    public void TakeDamage(int damage)
    { 
        //ü��üũ
        _monsterHP -= damage;
        //������ ü�� ȣ��
        _monsterHPUI.MonsterHPUpdata(damage);
        GameObject go = Instantiate(Data.Instance.BloodEffect, transform.position, Quaternion.identity);
        Destroy(go, 0.5f);
        if (_monsterHP <= 0)
        {
            //���
            MonsterSound.Instance.MonsterDeath(Type);
            ItemDrop();
            gameObject.SetActive(false);
            PlayerController.Instance.UpCoinMultiple();

            if (Type == MonsterType.EliteMonster)
            {
                GameManager.Instance.EliteMonsterDie();
            }
        }
        else
        {
            switch (Type)
            {
                case MonsterType.Monster1:
                    _audio.clip = Data.Instance.SoundEffect[(int)SoundEffect.SlimeHit];
                    break;
                case MonsterType.Monster2:
                    _audio.clip = Data.Instance.SoundEffect[(int)SoundEffect.ZombieHit];
                    break;
                case MonsterType.Monster3:
                    _audio.clip = Data.Instance.SoundEffect[(int)SoundEffect.CowHit];
                    break;
                case MonsterType.Monster4:
                    _audio.clip = Data.Instance.SoundEffect[(int)SoundEffect.GoblinHit];
                    break;
                case MonsterType.EliteMonster:
                    _audio.clip = Data.Instance.SoundEffect[(int)SoundEffect.EliteGoblinHit];
                    break;
            }
            _audio.Play();
        }
    }

    void ItemDrop()
    {
        int dropCount = UnityEngine.Random.Range(3, 5);

        RaycastHit2D hitdata = Physics2D.Raycast(transform.position, MonsterLook, 0.1f, 1 << LayerMask.NameToLayer("DropItem"));
        if(hitdata)
        {
            if(hitdata.collider.GetComponent<DropItem>().Item._itemType == ItemType.Currency)
            {
                Currency cur = (Currency)hitdata.collider.GetComponent<DropItem>().Item;
                cur.Count += dropCount;
            }
        }
        else
        {
            GameObject go = Instantiate(Data.Instance.ItemPrefab, GameManager.Instance.ItemPool.transform);
            go.transform.position = transform.position;

            Currency cr = (Currency)Data.Instance.GetItemInfo(102);
            cr.Count = dropCount;

            go.GetComponent<DropItem>().Init(cr);

            if (Vector3.Distance(transform.position, PlayerController.Instance.transform.position) <= 1) go.GetComponent<DropItem>().OpenItemInfo();
        }
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
