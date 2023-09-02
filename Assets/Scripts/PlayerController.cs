using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using Unity.VisualScripting;
using static UnityEditor.Progress;
using UnityEditor.Experimental.GraphView;
using System.Drawing;
using Color = UnityEngine.Color;
using System.Net;
using Unity.Burst.CompilerServices;

public class PlayerController : MonoBehaviour
{
    private static PlayerController instance;

    //priteRenderer _spriter; // ���� ����� �ʱ�ȭ�ϱ�
    SpriteRenderer _childSpriteRenderer;
    Animator _animator;
    bool _fixanime = false;
    LayerMask _normalLayerMask;
    LayerMask _weaponCheckLayerMask;
    
    [SerializeField] MakeFog2 _MakeFog2;
    [SerializeField] SaveInfoData UnlockSaveData;

    bool _isSuccess = true;
    bool _isDubbleClick = true;

    float _lobbyMoveDelay = 0f;

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
            if (value <= _maxHP)
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

        
    Shovel _equipShovel;
    public Shovel EquipShovel { get { return _equipShovel; } }
    Weapon _equipWeapon;
    public Weapon EquipWeapon { get { return _equipWeapon; } }
    Armor _equipArmor;
    public Armor EquipArmor { get { return _equipArmor; } }
    Potion _equipPotion;
    public Potion EquipPotion { get { return _equipPotion; } }

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
        _childSpriteRenderer = GetComponentsInChildren<SpriteRenderer>()[1];
        IsX = true;
        //Debug.Log(GameManager.Instance.NowStage); ��������Ȯ��
        _normalLayerMask =
                  (1 << LayerMask.NameToLayer("Wall")) |
                  (1 << LayerMask.NameToLayer("Npc")) |
                  (1 << LayerMask.NameToLayer("Stair"));

        _weaponCheckLayerMask =
                  (1 << LayerMask.NameToLayer("Wall")) |
                  (1 << LayerMask.NameToLayer("Monster"));
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.NowStage == Stage.Lobby)
        {
            _lobbyMoveDelay += Time.deltaTime;
        }

        //if (Data.Instance.Player.State == CharacterState.Live && _isSuccess && _isDubbleClick)
        if (_isSuccess && _isDubbleClick)
        {
         
            if (Input.GetKeyDown(KeyCode.UpArrow)) // �� ȭ��ǥ�� �Է� �޾�����
            {
                Debug.Log(_equipWeapon.weaponType);
                if (GameManager.Instance.NowStage != Stage.Lobby)
                {
                    if (!GameManager.Instance.IsSuccess()) return;
                    MoveCharacter(Vector3.up);
                }
                else if (GameManager.Instance.NowStage == Stage.Lobby)
                {
                    if (_lobbyMoveDelay >= 0.4f)
                    {
                        MoveCharacter(Vector3.up);
                        _lobbyMoveDelay = 0f;
                    }
                }

                IsX = false;
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow)) // �Ʒ� ȭ��ǥ�� �Է� �޾�����
            {
                if (GameManager.Instance.NowStage != Stage.Lobby)
                {
                    if (!GameManager.Instance.IsSuccess()) return;
                    MoveCharacter(Vector3.down);
                }
                else if (GameManager.Instance.NowStage == Stage.Lobby)
                {
                    if (_lobbyMoveDelay >= 0.4f)
                    {
                        MoveCharacter(Vector3.down);
                        _lobbyMoveDelay = 0f;
                    }
                }
                IsX = false;
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow)) // ������ ȭ��ǥ�� �Է� �޾�����
            {
                if (GameManager.Instance.NowStage != Stage.Lobby)
                {
                    if (!GameManager.Instance.IsSuccess()) return;
                    MoveCharacter(Vector3.right);
                }
                else if (GameManager.Instance.NowStage == Stage.Lobby)
                {
                    if (_lobbyMoveDelay >= 0.4f)
                    {
                        MoveCharacter(Vector3.right);
                        _lobbyMoveDelay = 0f;
                    }
                }
                _childSpriteRenderer.flipX = true;
                IsX = true;
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow)) // ���� ȭ��ǥ�� �Է� �޾�����
            {
                if (GameManager.Instance.NowStage != Stage.Lobby)
                {
                    if (!GameManager.Instance.IsSuccess()) return;
                    MoveCharacter(Vector3.left);
                }
                else if (GameManager.Instance.NowStage == Stage.Lobby)
                {
                    if (_lobbyMoveDelay >= 0.4f)
                    {
                        MoveCharacter(Vector3.left);
                        _lobbyMoveDelay = 0f;
                    }
                }
                _childSpriteRenderer.flipX = false;
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
        Vector3 Temp = transform.position + vec / 2;
        RaycastHit2D hitdata2 = Physics2D.Raycast(Temp, vec, 2f, _weaponCheckLayerMask);

        if (hitdata2)
        {
            switch (_equipWeapon.weaponType)
            {
                case WeaponType.Dagger:
                    hitdata2 = Physics2D.Raycast(Temp, vec, 0.5f, _weaponCheckLayerMask);
                    if (hitdata2.collider.tag == "WeedWall")
                    {
                        break;
                    }
                    else if (hitdata2.collider.tag == "Monster")
                    {
                        hitdata2.collider.GetComponent<Monster>().TakeDamage(_damage);
                        return;
                    }
                    break;
                case WeaponType.Spear:
                    if (hitdata2.collider.tag == "WeedWall")
                    {
                        break;
                    }
                    else if (hitdata2.collider.tag == "Monster")
                    {
                        Debug.Log(hitdata2.collider.tag);
                        hitdata2.collider.GetComponent<Monster>().TakeDamage(_damage);
                        return;
                    }
                    break;
                case WeaponType.GreatSword:
                    if (hitdata2.collider.tag == "WeedWall")
                    {
                        hitdata2.collider.GetComponent<Wall>().DamageWall(_shovelPower);
                    }
                    else if (hitdata2.collider.tag == "Monster")
                    {
                        hitdata2.collider.GetComponent<Monster>().TakeDamage(_damage);
                    }
                    break;
            }
        }

        RaycastHit2D hitdata = Physics2D.Raycast(Temp, vec, 0.5f, _normalLayerMask);

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
                _childSpriteRenderer.sortingOrder = (int)(transform.position.y - 1) * -1; // ���̾� ����ȯ
                Move(vec);
            }
            
            else if (hitdata.collider.tag == "Item")
            {
                _childSpriteRenderer.sortingOrder = (int)(transform.position.y - 1) * -1; // ���̾� ����ȯ
                Move(vec);
                

                DropItem dropItem = hitdata.collider.GetComponent<DropItem>();
                switch (dropItem.Item._itemType)
                {
                    case ItemType.Currency:
                        //�ش��ϴ� ��ȭ�� ��� ��Ű�� ��������� ����
                        Currency cr = (Currency)dropItem.Item;
                        if (cr._ItemID == 101) GameManager.Instance.Dia += cr.Count;
                        else if (cr._ItemID == 102) GameManager.Instance.Gold += cr.Count;
                        dropItem.DeleteDropItem();
                        break;
                    case ItemType.Shovel:
                    case ItemType.Weapon:
                    case ItemType.Armor:
                    case ItemType.Potion:
                        GetItem(dropItem);
                        UpdateCharacterState();
                        GameManager.Instance.GetEquipItem();
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
            _childSpriteRenderer.sortingOrder = (int)(transform.position.y - 1) * -1; // ���̾� ����ȯ
            Move(vec);
        }
        _MakeFog2.UpdateFogOfWar();

    }

    //void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.red;
    //    Gizmos.DrawWireCube(transform.position + new Vector3(+1, 0, 0), new Vector3(0, 3, 0));

    //}

    void GreatSwordAttack(Vector3 direction)
    {
        Vector3 swordCenter = (Vector3)transform.position + direction; // ����� �߽� ��ġ ���

        if (direction == Vector3.down)
        {
            Vector3 boxSize = new Vector3(0.5f, 1.5f, 0);
            Collider2D[] colliders = Physics2D.OverlapBoxAll(swordCenter, boxSize, 0f, _weaponCheckLayerMask);
            foreach (Collider2D collider in colliders)
            {
                if (collider.CompareTag("Enemy"))
                {
                    collider.GetComponent<Monster>().TakeDamage(_damage);
                }
            }
        }
        if (direction == Vector3.up)
        {
            Vector3 boxSize = new Vector3(0.5f, 1.5f, 0f);
            Collider2D[] colliders = Physics2D.OverlapBoxAll(swordCenter, boxSize, 0f, _weaponCheckLayerMask);
            foreach (Collider2D collider in colliders)
            {
                if (collider.CompareTag("Enemy"))
                {
                    collider.GetComponent<Monster>().TakeDamage(_damage);
                }
            }
        }
        if (direction == Vector3.left)
        {
            Vector3 boxSize = new Vector3(1.5f, 0.5f, 0f);
            Collider2D[] colliders = Physics2D.OverlapBoxAll(swordCenter, boxSize, 0f, _weaponCheckLayerMask);
            foreach (Collider2D collider in colliders)
            {
                if (collider.CompareTag("Enemy"))
                {
                    collider.GetComponent<Monster>().TakeDamage(_damage);
                }
            }
        }
        if (direction == Vector3.right)
        {
            Vector3 boxSize = new Vector3(1.5f, 0.5f, 0);
            Collider2D[] colliders = Physics2D.OverlapBoxAll(swordCenter, boxSize, 0f, _weaponCheckLayerMask);
            foreach (Collider2D collider in colliders)
            {
                if (collider.CompareTag("Enemy"))
                {
                    collider.GetComponent<Monster>().TakeDamage(_damage);
                }
            }
        }

        // ��� ���� ���� �ִ� ��� �ݶ��̴����� ����



    }


    void GetItem(DropItem dropItem)
    {
        switch (dropItem.Item._itemType)
        {
            case ItemType.Shovel:
                Shovel shovel = (Shovel)dropItem.Item;
                Debug.Log($"���� ������ {dropItem.Item._ItemID}");

                if(_equipShovel != null)
                {
                    //�������� ���� ������
                    Shovel temp = new Shovel();
                    temp = _equipShovel;
                    _equipShovel = shovel;
                    dropItem.ChangeItem(temp);
                    Debug.Log($"������ ������ {_equipShovel._ItemID}");
                    Debug.Log($"������ ������ {dropItem.Item._ItemID}");
                    return;
                }
                else
                {
                    _equipShovel = shovel;
                }
                break;
            case ItemType.Weapon:
                Weapon weapon = (Weapon)dropItem.Item;
                Debug.Log($"���� ������ {dropItem.Item._ItemID}");

                if(_equipWeapon != null)
                {
                    Weapon temp = new Weapon();
                    temp = _equipWeapon;
                    _equipWeapon = weapon;
                    dropItem.ChangeItem(temp);

                    Debug.Log($"������ ������ {_equipWeapon._ItemID}");
                    Debug.Log($"������ ������ {dropItem.Item._ItemID}");
                    return;
                }
                else
                {
                    _equipWeapon = weapon;
                }
                break;
            case ItemType.Armor:
                Armor armor = (Armor)dropItem.Item;
                Debug.Log($"���� ������ {dropItem.Item._ItemID}");
                if (_equipArmor != null)
                {
                    Armor temp = new Armor();
                    temp = _equipArmor;
                    _equipArmor = armor;
                    dropItem.ChangeItem(temp);

                    Debug.Log($"������ ������ {_equipArmor._ItemID}");
                    Debug.Log($"������ ������ {dropItem.Item._ItemID}");
                    return;
                }
                else
                {
                    _equipArmor = armor;
                }
                break;
            case ItemType.Potion:
                Potion potion = (Potion)dropItem.Item;
                Debug.Log($"���� ������ {dropItem.Item._ItemID}");
                if (_equipPotion != null)
                {
                    Potion temp = new Potion();
                    temp = _equipPotion;
                    _equipPotion = potion;
                    dropItem.ChangeItem(temp);

                    Debug.Log($"������ ������ {_equipPotion._ItemID}");
                    Debug.Log($"������ ������ {dropItem.Item._ItemID}");
                    return;
                }
                else
                {
                    _equipPotion = potion;
                }
                break;
            case ItemType.Unlock:
                UnlockItem Unlock = (UnlockItem)dropItem.Item;
                int level = 0;
                switch (Unlock._ItemID)
                {
                    case 601:
                        if (PlayerPrefs.HasKey("PlayerHPUpgradeLevel")) level = PlayerPrefs.GetInt("PlayerHPUpgradeLevel");
                        break;
                    case 602:
                        if (PlayerPrefs.HasKey("TreasureBoxUpgradeLevel")) level = PlayerPrefs.GetInt("TreasureBoxUpgradeLevel");
                        break;
                    case 603:
                        if (PlayerPrefs.HasKey("ComboUpgradeLevel")) level = PlayerPrefs.GetInt("ComboUpgradeLevel");
                        break;
                }
                int needDia = -1;
                for (int j = 0; j < UnlockSaveData.unlockNeedDias.Count; j++)
                {
                    if (UnlockSaveData.unlockNeedDias[j].level == level)
                    {
                        needDia = UnlockSaveData.unlockNeedDias[j].NeedDia;
                    }
                }

                if (needDia != -1 && GameManager.Instance.Dia >= needDia)
                {
                    GameManager.Instance.Dia -= needDia;

                    switch (Unlock._ItemID)
                    {
                        case 601:
                            PlayerPrefs.SetInt("PlayerHPUpgradeLevel", level + 1);
                            break;
                        case 602:
                            PlayerPrefs.SetInt("TreasureBoxUpgradeLevel", level + 1);
                            break;
                        case 603:
                            PlayerPrefs.SetInt("ComboUpgradeLevel", level + 1);
                            break;
                    }
                    Data.Instance.SavePlayerData();
                    UpdateCharacterState();
                }
                dropItem.DeleteDropItem();
                return;
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

        if (Data.Instance.CharacterSaveData._equipShovelID != 0)
        {
            _equipShovel = (Shovel)Data.Instance.GetItemInfo(Data.Instance.CharacterSaveData._equipShovelID);
        }
        else
        {
            Debug.Log("�⺻�� ����");
            _equipShovel = (Shovel)Data.Instance.GetItemInfo(201);
        }


        if (Data.Instance.CharacterSaveData._equipWeaponID != 0)
        {
            _equipWeapon = (Weapon)Data.Instance.GetItemInfo(Data.Instance.CharacterSaveData._equipWeaponID);
        }
        else
        {
            Debug.Log("�⺻���� ����");
            _equipWeapon = (Weapon)Data.Instance.GetItemInfo(301);
        }


        if (Data.Instance.CharacterSaveData._equipArmorID != 0)
        {
            _equipArmor = (Armor)Data.Instance.GetItemInfo(Data.Instance.CharacterSaveData._equipArmorID);
        }
        else
        {
            _equipArmor = null;
        }

        if (Data.Instance.CharacterSaveData._equipPotionID != 0)
        {
            _equipPotion = (Potion)Data.Instance.GetItemInfo(Data.Instance.CharacterSaveData._equipPotionID);
        }
        else
        {
            _equipArmor = null;
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

        //�ִ�ü�� 
        _maxHP = 6;
        if (PlayerPrefs.HasKey("PlayerHPUpgradeLevel"))
        {
            _maxHP += PlayerPrefs.GetInt("PlayerHPUpgradeLevel") * 2;
        }
        //����ü��
        if (Data.Instance.CharacterSaveData._nowHP <= 0)
        {
            _nowHp = _maxHP;
        }
        else _nowHp = Data.Instance.CharacterSaveData._nowHP;

        //���ݷ�
        if (_equipWeapon != null)
        {
            _damage += _equipWeapon.Attack;
        }
        Debug.Log($"���� ���ݷ�" + _damage);

        //����
        if (_equipArmor != null)
        {
            _def += _equipArmor.Defence;
        }

        //�� ���ݷ�
        if(_equipShovel != null)
        {
            _shovelPower += _equipShovel.ShovelPower;
        }
       
        //���ι�� - �޺������� ��� �ϳĿ� ���� �ű⿡ ����
        //����? ���ӸŴ����� �־�ߵɵ�?
    }

    public void BaseItemEquip()
    {
        _equipShovel = (Shovel)Data.Instance.GetItemInfo(201);
        _equipWeapon = (Weapon)Data.Instance.GetItemInfo(301);
        _equipArmor = null;
        _equipPotion = null;
    }

    #endregion

    void Death()
    {
        GameManager.Instance.StageFail();
    }
}
