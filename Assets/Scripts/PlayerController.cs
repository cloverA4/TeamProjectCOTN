using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using Unity.VisualScripting;
using static UnityEditor.Progress;
using UnityEditor.Experimental.GraphView;

public class PlayerController : MonoBehaviour
{
    private static PlayerController instance;

    //priteRenderer _spriter; // 변수 선언과 초기화하기
    SpriteRenderer _childSpriteRenderer;
    Animator _animator;
    bool _fixanime = false;
    [SerializeField] LayerMask _layerMask;
    [SerializeField] MakeFog2 _MakeFog2;
    [SerializeField] SaveInfoData UnlockSaveData;

    bool _isSuccess = true;
    bool _isDubbleClick = true;

    float _lobbyMoveDelay = 0f;

    public bool IsX { get; private set; }

    //캐릭터 데이터
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



    public List<Item> PlayerEquipItemList { private set; get; }
    public List<Item> BaseEquipItem = new List<Item>();

    private void Awake()
    {
        if (null == instance)
        {
            instance = this;

            //씬 전환이 되더라도 파괴되지 않게 한다.
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            //만약 씬 이동이 되었는데 그 씬에도 Hierarchy에 GameMgr이 존재할 수도 있다.
            //그럴 경우엔 이전 씬에서 사용하던 인스턴스를 계속 사용해주는 경우가 많은 것 같다.
            //그래서 이미 전역변수인 instance에 인스턴스가 존재한다면 자신을 삭제해준다.
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
        //Debug.Log(GameManager.Instance.NowStage); 스테이지확인
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
            

            if (Input.GetKeyDown(KeyCode.UpArrow)) // 위 화살표를 입력 받았을때
            {
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
            else if (Input.GetKeyDown(KeyCode.DownArrow)) // 아래 화살표를 입력 받았을때
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
            else if (Input.GetKeyDown(KeyCode.RightArrow)) // 오른쪽 화살표를 입력 받았을때
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
            else if (Input.GetKeyDown(KeyCode.LeftArrow)) // 왼쪽 화살표를 입력 받았을때
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

    //IEnumerator SmoothMove(Vector3 targetPosition) // 충돌체가 먼저 앞에 있어야하므로 이구문은 사용x
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
        RaycastHit2D hitdata = Physics2D.Raycast(Temp, vec, 0.5f, _layerMask);
        // 왼쪽으로 빔을쏘는         
        foreach (Item item in PlayerEquipItemList)
        {
            if (item._itemType == ItemType.Weapon)
            {
                Weapon weapon = (Weapon)item;
                switch (weapon.weaponType)
                {
                    case WeaponType.Dagger:
                        // Dagger 무기의 범위 설정 및 동작
                        if (hitdata)
                        {
                            if (hitdata.collider.tag == "WeedWall")
                            {
                                
                            }
                        }
                        break;
                    case WeaponType.GreatSword:
                        GreatSwordAttack(vec);
                        // GreatSword 무기의 범위 설정 및 동작
                        break;
                    case WeaponType.Spear:
                        // Spear 무기의 범위 설정 및 동작
                        break;

                }
            }
        }

        if (hitdata)
        {
            Debug.Log(hitdata.collider.tag);
            if (hitdata.collider.tag == "WeedWall") // weedwall이 힛데이타에 태그로 들어왓다면
            {
                //Debug.Log(hitdata.collider.gameObject); // 힛데이타콜라이더게임오브젝트에 대한 정보가 출력된다
                //Destroy(hitdata.collider.gameObject); // 힛데이타콜라이더게임오브젝트를 파괴한다
                //setActive활용해서 벽부수는 표현해보기
                hitdata.collider.GetComponent<Wall>().DamageWall(_shovelPower);

            }
            else if (hitdata.collider.tag == "Door") // Door이(가) 힛데이타에 태그로 들어왓다면
            {
                hitdata.collider.GetComponent<Door>().OpenDoor();
            }
            else if (hitdata.collider.tag == "BadRock") // BadRock이 힛데이타에 태그로 들어왓다면
            {
                //hitdata.collider.GetComponent<Door>().InvincibilityWall();
            }
            else if (hitdata.collider.tag == "ShopWall") // ShopWall이 힛데이타에 태그로 들어왓다면
            {
                //hitdata.collider.GetComponent<Door>().InvincibilityWall();
            }
            else if (hitdata.collider.tag == "Stair")
            {
                _childSpriteRenderer.sortingOrder = (int)(transform.position.y - 1) * -1; // 레이어 값변환
                Move(vec);
            }
            else if (hitdata.collider.tag == "Monster")
            {
                hitdata.collider.GetComponent<Monster>().TakeDamage(_damage);
            }
            else if (hitdata.collider.tag == "Item")
            {
                _childSpriteRenderer.sortingOrder = (int)(transform.position.y - 1) * -1; // 레이어 값변환
                Move(vec);
                

                DropItem dropItem = hitdata.collider.GetComponent<DropItem>();
                switch (dropItem.Item._itemType)
                {
                    case ItemType.Currency:
                        //해당하는 재화를 상승 시키고 드랍아이템 삭제
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
                        //해당 아이템의 해금가격 만큼의 재화가 있는지 검사 후 구매 진행
                        //구매 시 해금데이터를 변경하고, 드랍아이템을 삭제 후 재화 변경
                        break;
                }
            }
        }
        else
        {
            _childSpriteRenderer.sortingOrder = (int)(transform.position.y - 1) * -1; // 레이어 값변환
            Move(vec);
        }
        _MakeFog2.UpdateFogOfWar();

    }


    void GreatSwordAttack(Vector3 direction)
    {
        Vector3 swordCenter = (Vector3)transform.position + direction; // 대검의 중심 위치 계산

        if (direction == Vector3.down)
        {
            Vector3 boxSize = new Vector3(0.5f, 1.5f, 0);
            Collider2D[] colliders = Physics2D.OverlapBoxAll(swordCenter, boxSize, 0f, _layerMask);
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
            Vector3 boxSize = new Vector3(0.5f, 1.5f,0f);
            Collider2D[] colliders = Physics2D.OverlapBoxAll(swordCenter, boxSize, 0f, _layerMask);
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
            Vector3 boxSize = new Vector3(1.5f, 0.5f,0f);
            Collider2D[] colliders = Physics2D.OverlapBoxAll(swordCenter, boxSize, 0f, _layerMask);
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
            Vector3 boxSize = new Vector3(1.5f, 0.5f,0);
            Collider2D[] colliders = Physics2D.OverlapBoxAll(swordCenter, boxSize, 0f, _layerMask);
            foreach (Collider2D collider in colliders)
            {
                if (collider.CompareTag("Enemy"))
                {
                    collider.GetComponent<Monster>().TakeDamage(_damage);
                }
            }
        }

        // 대검 범위 내에 있는 모든 콜라이더들을 검출



    }


    void GetItem(DropItem dropItem)
    {
        for (int i = 0; i < PlayerEquipItemList.Count; i++)
        {
            switch (dropItem.Item._itemType)
            {
                case ItemType.Shovel:
                    Shovel shovel = (Shovel)dropItem.Item;
                    Debug.Log($"먹을 아이템 {dropItem.Item._ItemID}");
                    if (PlayerEquipItemList[i]._itemType == ItemType.Shovel)
                    {
                        Shovel temp = new Shovel();
                        temp = (Shovel)PlayerEquipItemList[i];
                        PlayerEquipItemList[i] = shovel;
                        dropItem.ChangeItem(temp);

                        Debug.Log($"착용한 아이템 {PlayerEquipItemList[i]._ItemID}");
                        Debug.Log($"버려진 아이템 {dropItem.Item._ItemID}");
                        return;
                    }
                    break;
                case ItemType.Weapon:
                    Weapon weapon = (Weapon)dropItem.Item;
                    Debug.Log($"먹을 아이템 {dropItem.Item._ItemID}");
                    if (PlayerEquipItemList[i]._itemType == ItemType.Weapon)
                    {
                        Weapon temp = new Weapon();
                        temp = (Weapon)PlayerEquipItemList[i];
                        PlayerEquipItemList[i] = weapon;
                        dropItem.ChangeItem(temp);

                        Debug.Log($"착용한 아이템 {PlayerEquipItemList[i]._ItemID}");
                        Debug.Log($"버려진 아이템 {dropItem.Item._ItemID}");
                        return;
                    }
                    break;
                case ItemType.Armor:
                    Armor armor = (Armor)dropItem.Item;
                    Debug.Log($"먹을 아이템 {dropItem.Item._ItemID}");
                    if (PlayerEquipItemList[i]._itemType == ItemType.Armor)
                    {
                        Armor temp = new Armor();
                        temp = (Armor)PlayerEquipItemList[i];
                        PlayerEquipItemList[i] = armor;
                        dropItem.ChangeItem(temp);

                        Debug.Log($"착용한 아이템 {PlayerEquipItemList[i]._ItemID}");
                        Debug.Log($"버려진 아이템 {dropItem.Item._ItemID}");
                        return;
                    }
                    break;
                case ItemType.Potion:
                    Potion potion = (Potion)dropItem.Item;
                    Debug.Log($"먹을 아이템 {dropItem.Item._ItemID}");
                    if (PlayerEquipItemList[i]._itemType == ItemType.Potion)
                    {
                        Potion temp = new Potion();
                        temp = (Potion)PlayerEquipItemList[i];
                        PlayerEquipItemList[i] = potion;
                        dropItem.ChangeItem(temp);

                        Debug.Log($"착용한 아이템 {PlayerEquipItemList[i]._ItemID}");
                        Debug.Log($"버려진 아이템 {dropItem.Item._ItemID}");
                        return;
                    }
                    break;
                case ItemType.Unlock:
                    UnlockItem Unlock = (UnlockItem)dropItem.Item;
                    int level = 0;
                    switch(Unlock._ItemID)
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

                    if(needDia != -1 && GameManager.Instance.Dia >= needDia)
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
        }

        PlayerEquipItemList.Add(dropItem.Item);
        dropItem.DeleteDropItem();
    }

    void Move(Vector3 vec)
    {
        transform.position += vec;
        _MakeFog2.UpdateFogOfWar();
        GetComponent<SpriteRenderer>().sortingOrder = (int)(transform.position.y - 1) * -1; // 레이어 값변환
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
        //게임메니져에서 각종 데이터 로드가 끝나면, 그때 실행
        BaseEquipItem.Add(Data.Instance.GetItemInfo(201));
        BaseEquipItem.Add(Data.Instance.GetItemInfo(301));

        if (Data.Instance.CharacterSaveData._equipItemId == null)
        {
            Debug.Log("기본아이템 장착");
            InitEquipItem();
            Debug.Log(BaseEquipItem.Count+"이닛함수 밑");
        }
        else
        {
            PlayerEquipItemList.Clear();
            for (int i = 0; i < Data.Instance.CharacterSaveData._equipItemId.Count; i++)
            {
                Debug.Log($"저장된 아이템아이디 {Data.Instance.CharacterSaveData._equipItemId[i]}");
                PlayerEquipItemList.Add(Data.Instance.GetItemInfo(Data.Instance.CharacterSaveData._equipItemId[i]));
            }
        }

        //캐릭터 스텟
        UpdateCharacterState();
    }

    #region

    void UpdateCharacterState()
    {
        _damage = 0;
        _def = 0;
        _shovelPower = 0;

        //최대체력 
        _maxHP = 6;
        if (PlayerPrefs.HasKey("PlayerHPUpgradeLevel"))
        {
            _maxHP += PlayerPrefs.GetInt("PlayerHPUpgradeLevel") * 2;
        }
        //현재체력
        if (Data.Instance.CharacterSaveData._nowHP <= 0)
        {
            _nowHp = _maxHP;
        }
        else _nowHp = Data.Instance.CharacterSaveData._nowHP;

        for (int i = 0; i < PlayerEquipItemList.Count; i++)
        {
            //공격력
            if (PlayerEquipItemList[i]._itemType == ItemType.Weapon)
            {
                Weapon wp = (Weapon)PlayerEquipItemList[i];
                _damage += wp.Attack;
            }
            //방어력
            else if (PlayerEquipItemList[i]._itemType == ItemType.Armor)
            {
                Armor am = (Armor)PlayerEquipItemList[i];
                _def += am.Defence;
            }
            //삽 공격력
            else if (PlayerEquipItemList[i]._itemType == ItemType.Shovel)
            {
                Shovel sv = (Shovel)PlayerEquipItemList[i];
                _shovelPower += sv.ShovelPower;
            }
        }
        Debug.Log($"현재 공격력" + _damage);
        //코인배수 - 콤보구현을 어디서 하냐에 따라서 거기에 적용
        //상자? 게임매니저에 넣어야될듯?
    }

    public void InitEquipItem()
    {   
        PlayerEquipItemList.Clear();
        for (int i = 0; i < BaseEquipItem.Count; i++)
        {
            PlayerEquipItemList.Add(BaseEquipItem[i]);
        }
        
    }

    #endregion

    void Death()
    {
        GameManager.Instance.StageFail();
    }
}
