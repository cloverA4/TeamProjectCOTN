using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using Unity.VisualScripting;

public class PlayerController : MonoBehaviour
{
    private static PlayerController instance;

    //priteRenderer _spriter; // ���� ����� �ʱ�ȭ�ϱ�
    SpriteRenderer _childSpriteRenderer;
    Animator _animator;
    bool _fixanime = false;
    [SerializeField] LayerMask _layerMask;
    [SerializeField] MakeFog2 _MakeFog2;
    [SerializeField] SaveInfoData UnlockSaveData;

    bool _isSuccess = true;
    bool _isDubbleClick = true;
    public bool IsX { get; private set; }

    //ĳ���� ������
    bool _isLive = true;
    public bool IsLive
    {
        get { return _isLive; }
        set { _isLive = value; }
    }

    int _nowHp;
    public int NowHP
    {
        get { return _nowHp; }
        set 
        {
            if(value <= _maxHP)
            {
                _nowHp = value;
            }            
            GameManager.Instance.PlayerHPUpdate();
            if (_nowHp <= 0)
            {
                _isLive = false;
                GameManager.Instance.StageFail();
            }
        }      
    }

    int _maxHP = 0;
    public int MaxHP
    {
        get { return _maxHP; }
        set { _maxHP = value; }
    }

    int _shovelPower = 0;

    public int ShovelPower
    {
        get { return _shovelPower; }
        set { _shovelPower = value; }
    }

    int _damage = 0;

    public int Damage
    {
        get { return _damage; }
        set { _damage = value; }
    }

    int _def = 0;

    public int Def
    {
        get { return _def; }
        set { _def = value; }
    }

    public List<Item> PlayerEquipItemList { private set; get; }
    public List<Item> BaseEquipItem = new List<Item>();

    private void Awake()
    {
        if (null == instance)
        {
            instance = this;

            //�� ��ȯ�� �Ǵ��� �ı����� �ʰ� �Ѵ�.
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            //���� �� �̵��� �Ǿ��µ� �� ������ Hierarchy�� GameMgr�� ������ ���� �ִ�.
            //�׷� ��쿣 ���� ������ ����ϴ� �ν��Ͻ��� ��� ������ִ� ��찡 ���� �� ����.
            //�׷��� �̹� ���������� instance�� �ν��Ͻ��� �����Ѵٸ� �ڽ��� �������ش�.
            Destroy(this.gameObject);
        }
    }

    public static PlayerController Instance
    {
        get
        {
            if (null == instance)
            {
                return null;
            }
            return instance;
        }
    }


    void Start()
    {
        //_childSpriteRenderer = GetComponentInChildrens<SpriteRenderer>()[1];
        _animator = GetComponentsInChildren<Animator>()[0];
        PlayerEquipItemList = new List<Item>();
        _childSpriteRenderer = GetComponentsInChildren<SpriteRenderer>()[1];
        IsX = true;
    }

    // Update is called once per frame
    void Update()
    {
        //if (Data.Instance.Player.State == CharacterState.Live && _isSuccess && _isDubbleClick)
        if (_isSuccess && _isDubbleClick)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow)) // �� ȭ��ǥ�� �Է� �޾�����
            {
                if (GameManager.Instance.NowStage != Stage.Lobby)
                {
                    if (!GameManager.Instance.IsSuccess()) return;
                }
                //â ����
                //â�� �������ִٸ� â��ũ��Ʈ�� �ִ� ����üũ���ϰ�
                //����
                //if (WeaponManager.Instance.NowEquip == WeaponType.ShortSword) // �����ʿ�!!!!!!! Data�� EnumŸ������ ItemType���� weapon��
                //                                                              // �־ ��������� ShortSword,GreatSword,Spear�� ����������
                //{
                //}
                //if (WeaponManager.Instance.NowEquip == WeaponType.GreatSword)
                //{
                //}
                //if (WeaponManager.Instance.NowEquip == WeaponType.Spear)
                //{
                //}

                //�ƴ϶�� ĳ���� ����
                MoveCharacter(Vector3.up);
                IsX = false;
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow)) // �Ʒ� ȭ��ǥ�� �Է� �޾�����
            {
                if (GameManager.Instance.NowStage != Stage.Lobby)
                {
                    if (!GameManager.Instance.IsSuccess()) return;
                }
                MoveCharacter(Vector3.down);
                IsX = false;
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow)) // ������ ȭ��ǥ�� �Է� �޾�����
            {
                if (GameManager.Instance.NowStage != Stage.Lobby)
                {
                    if (!GameManager.Instance.IsSuccess()) return;
                }
                
                //_animator.SetTrigger("Right1");
                MoveCharacter(Vector3.right);
                _childSpriteRenderer.flipX = true;
                //_animator.SetBool("Right", false);
                IsX = true;

            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow)) // ���� ȭ��ǥ�� �Է� �޾�����
            {
                if (GameManager.Instance.NowStage != Stage.Lobby)
                {
                    if (!GameManager.Instance.IsSuccess()) return;
                }
                MoveCharacter(Vector3.left);

                _childSpriteRenderer.flipX = false;
                
                //_isMove = false;
                //_transform.position = Vector3.Lerp(transform.)
                IsX = true;
            }

           
            
            



        }
    }

    private float moveSpeed = 10f;
    private bool isMoving = false;

    //IEnumerator SmoothMove(Vector3 targetPosition) // �浹ü�� ���� �տ� �־���ϹǷ� �̱����� ���x
    //{
    //    if (isMoving == true)
    //    {
    //        yield break;
    //    }
    //    isMoving = true;
    //    float startTime = Time.time;
    //    Vector3 startPosition = transform.position;

    //    while (Time.time < startTime + 1 / moveSpeed)
    //    {
    //        float t = (Time.time - startTime) * moveSpeed;
    //        transform.position = Vector3.MoveTowards(startPosition, targetPosition, t);
    //        yield return null;
    //    }

    //    transform.position = targetPosition;
    //    isMoving = false;
    //    if (isMoving == false) 
    //    {
    //        _MakeFog2.UpdateFogOfWar();
    //    }
    //}

    void MoveCharacter(Vector3 vec)
    {
        Vector3 Temp = transform.position;
        Vector3 targetPosition = Temp + vec;
        RaycastHit2D hitdata = Physics2D.Raycast(Temp, vec, 1f, _layerMask);

        // �������� �������         

        if (hitdata)
        {
            Debug.Log(hitdata.collider.tag);
            if (hitdata.collider.tag == "WeedWall") // weedwall�� ������Ÿ�� �±׷� ���Ӵٸ�
            {
                //Debug.Log(hitdata.collider.gameObject); // ������Ÿ�ݶ��̴����ӿ�����Ʈ�� ���� ������ ��µȴ�
                //Destroy(hitdata.collider.gameObject); // ������Ÿ�ݶ��̴����ӿ�����Ʈ�� �ı��Ѵ�
                //setActiveȰ���ؼ� ���μ��� ǥ���غ���
                hitdata.collider.GetComponent<Wall>().DamageWall(_shovelPower);

            }
            else if (hitdata.collider.tag == "Door") // Door��(��) ������Ÿ�� �±׷� ���Ӵٸ�
            {
                hitdata.collider.GetComponent<Door>().OpenDoor();
            }
            else if (hitdata.collider.tag == "BadRock") // BadRock�� ������Ÿ�� �±׷� ���Ӵٸ�
            {
                //hitdata.collider.GetComponent<Door>().InvincibilityWall();
            }
            else if (hitdata.collider.tag == "ShopWall") // ShopWall�� ������Ÿ�� �±׷� ���Ӵٸ�
            {
                //hitdata.collider.GetComponent<Door>().InvincibilityWall();
            }
            else if (hitdata.collider.tag == "Stair")
            {
                _animator.SetTrigger("MoveX");
                _childSpriteRenderer.sortingOrder = (int)(transform.position.y - 1) * -1; // ���̾� ����ȯ
                Move(vec);
            }
            else if (hitdata.collider.tag == "Monster")
            {
                hitdata.collider.GetComponent<Monster>().TakeDamage(_damage);
            }
            else if(hitdata.collider.tag == "Item")
            {
                _animator.SetTrigger("MoveX");
                _childSpriteRenderer.sortingOrder = (int)(transform.position.y - 1) * -1; // ���̾� ����ȯ
                Move(vec);

                DropItem dropItem = hitdata.collider.GetComponent<DropItem>();
                switch (dropItem.Item._itemType)
                {
                    case ItemType.Currency:
                        //�ش��ϴ� ��ȭ�� ��� ��Ű�� ��������� ����
                        Currency cr = (Currency)dropItem.Item;
                        if (cr._ItemID == 101) GameManager.Instance.Dia += cr.Count;
                        else if(cr._ItemID == 102) GameManager.Instance.Dia += cr.Count;
                        break;
                    case ItemType.Shovel:
                    case ItemType.Weapon:
                    case ItemType.Armor:
                    case ItemType.Potion:
                        GetItem(dropItem);
                        UpdateCharacterState();
                        break;
                    case ItemType.Unlock:
                        //�ش� �������� �رݰ��� ��ŭ�� ��ȭ�� �ִ��� �˻� �� ���� ����
                        //���� �� �رݵ����͸� �����ϰ�, ����������� ���� �� ��ȭ ����
                        break;
                }
            }
        }
        else
        {
            //TestmoveWay(vec,0);
            _animator.SetTrigger("MoveX");
            _childSpriteRenderer.sortingOrder = (int)(transform.position.y - 1) * -1; // ���̾� ����ȯ
            Move(vec);

        }
        _MakeFog2.UpdateFogOfWar();

    }

    void GetItem(DropItem dropItem)
    {
        for (int i = 0; i < PlayerEquipItemList.Count; i++)
        {
            switch (dropItem.Item._itemType)
            {
                case ItemType.Shovel:
                    Shovel shovel = (Shovel)dropItem.Item;
                    Debug.Log($"���� ������ {dropItem.Item._ItemID}");
                    if (PlayerEquipItemList[i]._itemType == ItemType.Weapon)
                    {
                        Shovel temp = new Shovel();
                        temp = (Shovel)PlayerEquipItemList[i];
                        PlayerEquipItemList[i] = shovel;
                        dropItem.ChangeItem(temp);

                        Debug.Log($"������ ������ {PlayerEquipItemList[i]._ItemID}");
                        Debug.Log($"������ ������ {dropItem.Item._ItemID}");
                        return;
                    }
                    break;
                case ItemType.Weapon:
                    Weapon weapon = (Weapon)dropItem.Item;
                    Debug.Log($"���� ������ {dropItem.Item._ItemID}");
                    if (PlayerEquipItemList[i]._itemType == ItemType.Weapon)
                    {
                        Weapon temp = new Weapon();
                        temp = (Weapon)PlayerEquipItemList[i];
                        PlayerEquipItemList[i] = weapon;
                        dropItem.ChangeItem(temp);

                        Debug.Log($"������ ������ {PlayerEquipItemList[i]._ItemID}");
                        Debug.Log($"������ ������ {dropItem.Item._ItemID}");
                        return;
                    }                   
                    break;
                case ItemType.Armor:
                    Armor armor = (Armor)dropItem.Item;
                    Debug.Log($"���� ������ {dropItem.Item._ItemID}");
                    if (PlayerEquipItemList[i]._itemType == ItemType.Weapon)
                    {
                        Armor temp = new Armor();
                        temp = (Armor)PlayerEquipItemList[i];
                        PlayerEquipItemList[i] = armor;
                        dropItem.ChangeItem(temp);
                        
                        Debug.Log($"������ ������ {PlayerEquipItemList[i]._ItemID}");
                        Debug.Log($"������ ������ {dropItem.Item._ItemID}");
                    }
                    return;
                case ItemType.Potion:
                    Potion potion = (Potion)dropItem.Item;
                    Debug.Log($"���� ������ {dropItem.Item._ItemID}");
                    if (PlayerEquipItemList[i]._itemType == ItemType.Weapon)
                    {
                        Weapon temp = new Weapon();
                        temp = (Weapon)PlayerEquipItemList[i];
                        PlayerEquipItemList[i] = potion;
                        dropItem.ChangeItem(temp);

                        Debug.Log($"������ ������ {PlayerEquipItemList[i]._ItemID}");
                        Debug.Log($"������ ������ {dropItem.Item._ItemID}");
                        return;
                    }
                    break;
            }
        }
        dropItem.DeleteDropItem();
    }



   

    void Move(Vector3 vec)
    {
        transform.position += vec;
        _MakeFog2.UpdateFogOfWar();
        GetComponent<SpriteRenderer>().sortingOrder = (int)(transform.position.y - 1) * -1; // ���̾� ����ȯ
        if (vec == Vector3.left)
        {
            _animator.SetTrigger("Left");
        }
        else if (vec == Vector3.right)
        {
            _animator.SetTrigger("Right");
        }
        else if (vec == Vector3.up)
        {
            _animator.SetTrigger("Up");
        }
        else if (vec == Vector3.down)
        {
            _animator.SetTrigger("Down");
        }
    }

    public void transfromUpdate(Vector3 vec)
    {
        transform.position = vec;
    }

    public void InitCharacterData()
    {
        //���Ӹ޴������� ���� ������ �ε尡 ������, �׶� ����
        BaseEquipItem.Add(Data.Instance.GetItemInfo(201));
        BaseEquipItem.Add(Data.Instance.GetItemInfo(301));

        //�ִ�ü�� 
        _maxHP = 3;
        for (int i = 0; i < UnlockSaveData.unlockCount.Count; i++)
        {
            if (UnlockSaveData.unlockCount[i].name == "HP")
            {
                _maxHP += UnlockSaveData.unlockCount[i].count;
            }
        }
        //����ü��
        if (Data.Instance.CharacterSaveData._nowHP <= 0)
        {
            _nowHp = _maxHP;
        }
        else _nowHp = Data.Instance.CharacterSaveData._nowHP;


        if(Data.Instance.CharacterSaveData._equipItemId == null)
        {
            Debug.Log("�⺻������ ����");
            InitEquipItem();
        }
        else
        {
            PlayerEquipItemList.Clear();
            for(int i = 0; i < Data.Instance.CharacterSaveData._equipItemId.Count; i++)
            {
                Debug.Log($"����� �����۾��̵� {Data.Instance.CharacterSaveData._equipItemId[i]}");
                PlayerEquipItemList.Add(Data.Instance.GetItemInfo(Data.Instance.CharacterSaveData._equipItemId[i]));
            }
        }
        
        //ĳ���� ����
        UpdateCharacterState();
    }

    #region

    void UpdateCharacterState()
    {
        _damage = 0;
        _def = 0;
        _shovelPower = 0;
        for (int i = 0; i < PlayerEquipItemList.Count; i++)
        {
            //���ݷ�
            if (PlayerEquipItemList[i]._itemType == ItemType.Weapon)
            {
                Weapon wp = (Weapon)PlayerEquipItemList[i];
                _damage += wp.Attack;
            }
            //����
            else if (PlayerEquipItemList[i]._itemType == ItemType.Armor)
            {
                Armor am = (Armor)PlayerEquipItemList[i];
                _def += am.Defence;
            }
            //�� ���ݷ�
            else if (PlayerEquipItemList[i]._itemType == ItemType.Shovel)
            {
                Shovel sv = (Shovel)PlayerEquipItemList[i];
                _shovelPower += sv.ShovelPower;
            }
        }
        Debug.Log($"���� ���ݷ�" + _damage);
        //���ι�� - �޺������� ��� �ϳĿ� ���� �ű⿡ ����
        //����? ���ӸŴ����� �־�ߵɵ�?
    }

    public void InitEquipItem()
    {
        PlayerEquipItemList.Clear();
        PlayerEquipItemList = BaseEquipItem;
    }

    #endregion

    void Death()
    {        
        GameManager.Instance.StageFail();
    }
}
