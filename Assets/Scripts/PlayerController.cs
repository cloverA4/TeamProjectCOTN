using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private static PlayerController instance;

    //priteRenderer _spriter; // 변수 선언과 초기화하기
    SpriteRenderer _childSpriteRenderer;
    Animator _animator;
    bool _fixanime = false;
    LayerMask _normalLayerMask;
    LayerMask _weaponCheckLayerMask;
    
    [SerializeField] MakeFog2 _MakeFog2;
    [SerializeField] SaveInfoData UnlockSaveData;

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

        
    Shovel _equipShovel;
    public Shovel EquipShovel { get { return _equipShovel; } }
    Weapon _equipWeapon;
    public Weapon EquipWeapon { get { return _equipWeapon; } }
    Armor _equipArmor;
    public Armor EquipArmor { get { return _equipArmor; } }
    Potion _equipPotion;
    public Potion EquipPotion { get { return _equipPotion; } }

    private float moveSpeed = 10f;
    private bool isMoving = false;

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
        _childSpriteRenderer = GetComponentsInChildren<SpriteRenderer>()[1];
        IsX = true;
        //Debug.Log(GameManager.Instance.NowStage); 스테이지확인
        _normalLayerMask =
                  (1 << LayerMask.NameToLayer("Wall")) |
                  (1 << LayerMask.NameToLayer("Npc")) |
                  (1 << LayerMask.NameToLayer("Stair")) |
                  (1 << LayerMask.NameToLayer("Monster")) |
                  (1 << LayerMask.NameToLayer("DropItem"));

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

        if (Input.GetKey(KeyCode.LeftShift))
        {
            //특수기능들 구현

            //물약
            if(Input.GetKeyDown(KeyCode.UpArrow))
            {
                if (_equipPotion == null) return;
                else
                {
                    if (GameManager.Instance.NowStage != Stage.Lobby && GameManager.Instance.NowFloor != floor.fBoss)
                    {
                        if (!GameManager.Instance.IsSuccess()) return;
                        UsePotion();
                    }
                }
            }
        }
        else
        {

            if (Input.GetKeyDown(KeyCode.UpArrow)) // 위 화살표를 입력 받았을때
            {
                Debug.Log(_equipWeapon.weaponType);
                if (GameManager.Instance.NowStage != Stage.Lobby && GameManager.Instance.NowFloor != floor.fBoss)
                {
                    if (!GameManager.Instance.IsSuccess()) return;
                    MoveCharacter(Vector3.up);
                }
                else
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
                if (GameManager.Instance.NowStage != Stage.Lobby && GameManager.Instance.NowFloor != floor.fBoss)
                {
                    if (!GameManager.Instance.IsSuccess()) return;
                    MoveCharacter(Vector3.down);
                }
                else
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
                if (GameManager.Instance.NowStage != Stage.Lobby && GameManager.Instance.NowFloor != floor.fBoss)
                {
                    if (!GameManager.Instance.IsSuccess()) return;
                    MoveCharacter(Vector3.right);
                }
                else
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
                if (GameManager.Instance.NowStage != Stage.Lobby && GameManager.Instance.NowFloor != floor.fBoss)
                {
                    if (!GameManager.Instance.IsSuccess()) return;
                    MoveCharacter(Vector3.left);
                }
                else
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


        //if (hitdata2)
        //{
        //}

        switch (_equipWeapon.weaponType)
        {
            case WeaponType.Dagger:
                RaycastHit2D hitdataTypeDagger = Physics2D.Raycast(Temp, vec, 0.5f, _weaponCheckLayerMask);
                if (hitdataTypeDagger)
                {
                    if (hitdataTypeDagger.collider.tag == "Monster")
                    {
                        hitdataTypeDagger.collider.GetComponent<Monster>().TakeDamage(_damage);
                        return;
                    }
                    break;
                }
                break;

            case WeaponType.Spear:
                RaycastHit2D hitdataTypeSpear = Physics2D.Raycast(Temp, vec, 2f, _weaponCheckLayerMask);
                if (hitdataTypeSpear)
                {
                    if (hitdataTypeSpear.collider.tag == "Monster")
                    {
                        Debug.Log(hitdataTypeSpear.collider.tag);
                        hitdataTypeSpear.collider.GetComponent<Monster>().TakeDamage(_damage);
                        return;
                    }
                }
                break;

            case WeaponType.GreatSword:
                Vector3 swordCenter = (Vector3)transform.position + vec; // 대검의 중심 위치 계산
                Vector3 boxSize;
                Collider2D[] colliders;
                if (vec == Vector3.down || vec == Vector3.up)
                {
                    boxSize = new Vector3(2f, 1f, 0);
                }
                else if (vec == Vector3.left || vec == Vector3.right)
                {
                    boxSize = new Vector3(1f, 2f, 0f);
                }
                else
                {
                    return;
                }

                colliders = Physics2D.OverlapBoxAll(swordCenter, boxSize, 0f, _weaponCheckLayerMask);
                bool IsMonster = false;
                foreach (Collider2D collider in colliders)
                {
                    if (collider.CompareTag("Monster"))
                    {
                        collider.GetComponent<Monster>().TakeDamage(_damage);
                        IsMonster = true;
                    }
                }

                if (IsMonster) return;
                break;
        }

        RaycastHit2D hitdata = Physics2D.Raycast(Temp, vec, 0.5f, _normalLayerMask);

        if (hitdata)
        {   
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
            else if (hitdata.collider.tag == "Item")
            {
                _childSpriteRenderer.sortingOrder = (int)(transform.position.y - 1) * -1; // 레이어 값변환
                Move(vec);

                DropItem dropItem = hitdata.collider.GetComponent<DropItem>();
                switch(dropItem.ItemType)
                {
                    case DropItemType.Drop:
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
                        }
                        break;
                    case DropItemType.Shop:                        
                        BuyGoldItem(dropItem);
                        break;
                    case DropItemType.UnlockShop:

                        switch (dropItem.Item._itemType)
                        {
                            case ItemType.Shovel:
                            case ItemType.Weapon:
                            case ItemType.Armor:
                            case ItemType.Potion:
                                UnlockEquipItem(dropItem);
                                break;
                            case ItemType.Unlock:
                                UnlockPassiveItem(dropItem);
                                break;
                        }                        
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
       

        // 대검 범위 내에 있는 모든 콜라이더들을 검출



    }


    void GetItem(DropItem dropItem)
    {
        switch (dropItem.Item._itemType)
        {
            case ItemType.Shovel:
                Shovel shovel = (Shovel)dropItem.Item;
                Debug.Log($"먹을 아이템 {dropItem.Item._ItemID}");

                if(_equipShovel != null)
                {
                    //장착중인 삽이 있으면
                    Shovel temp = new Shovel();
                    temp = _equipShovel;
                    _equipShovel = shovel;
                    dropItem.ChangeItem(temp);
                    Debug.Log($"착용한 아이템 {_equipShovel._ItemID}");
                    Debug.Log($"버려진 아이템 {dropItem.Item._ItemID}");
                    return;
                }
                else
                {
                    _equipShovel = shovel;
                }
                break;
            case ItemType.Weapon:
                Weapon weapon = (Weapon)dropItem.Item;
                Debug.Log($"먹을 아이템 {dropItem.Item._ItemID}");

                if(_equipWeapon != null)
                {
                    Weapon temp = new Weapon();
                    temp = _equipWeapon;
                    _equipWeapon = weapon;
                    dropItem.ChangeItem(temp);

                    Debug.Log($"착용한 아이템 {_equipWeapon._ItemID}");
                    Debug.Log($"버려진 아이템 {dropItem.Item._ItemID}");
                    return;
                }
                else
                {
                    _equipWeapon = weapon;
                }
                break;
            case ItemType.Armor:
                Armor armor = (Armor)dropItem.Item;
                Debug.Log($"먹을 아이템 {dropItem.Item._ItemID}");
                if (_equipArmor != null)
                {
                    Armor temp = new Armor();
                    temp = _equipArmor;
                    _equipArmor = armor;
                    dropItem.ChangeItem(temp);

                    Debug.Log($"착용한 아이템 {_equipArmor._ItemID}");
                    Debug.Log($"버려진 아이템 {dropItem.Item._ItemID}");
                    return;
                }
                else
                {
                    _equipArmor = armor;
                }
                break;
            case ItemType.Potion:
                Potion potion = (Potion)dropItem.Item;
                Debug.Log($"먹을 아이템 {dropItem.Item._ItemID}");
                if (_equipPotion != null)
                {
                    Potion temp = new Potion();
                    temp = _equipPotion;
                    _equipPotion = potion;
                    dropItem.ChangeItem(temp);

                    Debug.Log($"착용한 아이템 {_equipPotion._ItemID}");
                    Debug.Log($"버려진 아이템 {dropItem.Item._ItemID}");
                    return;
                }
                else
                {
                    _equipPotion = potion;
                }
                break;
        }
        dropItem.DeleteDropItem();
    }

    void BuyGoldItem(DropItem dropItem)
    {
        //골드체크를 하고 골드가충분하면, 골드를 차감하고 저장한다음에 습득 호출
        int needGold = -1;
        switch (dropItem.Item._itemType)
        {
            case ItemType.Shovel:
                Shovel sv = (Shovel)Data.Instance.GetItemInfo(dropItem.Item._ItemID);
                needGold = sv.Price;
                break;
            case ItemType.Weapon:
                Weapon wp = (Weapon)Data.Instance.GetItemInfo(dropItem.Item._ItemID);
                needGold = wp.Price;
                break;
            case ItemType.Armor:
                Armor ar = (Armor)Data.Instance.GetItemInfo(dropItem.Item._ItemID);
                needGold = ar.Price;
                break;
            case ItemType.Potion:
                Potion po = (Potion)Data.Instance.GetItemInfo(dropItem.Item._ItemID);
                needGold = po.Price;
                break;
        }

        if (needGold <= GameManager.Instance.Gold)
        {
            //자원이 충분하면 구매
            GameManager.Instance.Gold -= needGold;
            //Data.Instance.SavePlayerData();

            GetItem(dropItem);
            UpdateCharacterState();
            GameManager.Instance.GetEquipItem(); // 유아이 관련 호출
        }
        else
        {
            //골드부족!
        }
    }

    void UnlockEquipItem(DropItem dropItem)
    {
        int needDia = -1;
        switch (dropItem.Item._itemType)
        {
            case ItemType.Shovel:
                Shovel sv = (Shovel)Data.Instance.GetItemInfo(dropItem.Item._ItemID);
                needDia = sv.UnlockPrice;
                break;
            case ItemType.Weapon:
                Weapon wp = (Weapon)Data.Instance.GetItemInfo(dropItem.Item._ItemID);
                needDia = wp.UnlockPrice;
                break;
            case ItemType.Armor:
                Armor ar = (Armor)Data.Instance.GetItemInfo(dropItem.Item._ItemID);
                needDia = ar.UnlockPrice;
                break;
            case ItemType.Potion:
                Potion po = (Potion)Data.Instance.GetItemInfo(dropItem.Item._ItemID);
                needDia = po.UnlockPrice;
                break;
        }

        if (needDia <= GameManager.Instance.Dia)
        {
            GameManager.Instance.Dia -= needDia;            
            Data.Instance.CharacterSaveData._unlockItemId.Add(dropItem.Item._ItemID); //언락리스트에 아이템아이디 추가
            Data.Instance.SavePlayerData();

            GetItem(dropItem);
            UpdateCharacterState();
            GameManager.Instance.GetEquipItem(); // 유아이 관련 호출
        }
        else
        {
            //다이아 부족!!
        }
    }

    void UnlockPassiveItem(DropItem dropItem)
    {
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

    void UsePotion()
    {
        if (NowHP < MaxHP) NowHP += _equipPotion.Heal;
        _equipPotion = null;
        //유아이 업데이트
    }

    public void InitCharacterData()
    {
        //게임메니져에서 각종 데이터 로드가 끝나면, 그때 실행

        if (Data.Instance.CharacterSaveData._equipShovelID != 0)
        {
            _equipShovel = (Shovel)Data.Instance.GetItemInfo(Data.Instance.CharacterSaveData._equipShovelID);
        }
        else
        {
            Debug.Log("기본삽 장착");
            _equipShovel = (Shovel)Data.Instance.GetItemInfo(201);
        }


        if (Data.Instance.CharacterSaveData._equipWeaponID != 0)
        {
            _equipWeapon = (Weapon)Data.Instance.GetItemInfo(Data.Instance.CharacterSaveData._equipWeaponID);
        }
        else
        {
            Debug.Log("기본무기 장착");
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

        //캐릭터 스텟
        UpdateCharacterState();
    }

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

        //공격력
        if (_equipWeapon != null)
        {
            _damage += _equipWeapon.Attack;
        }
        Debug.Log($"현재 공격력" + _damage);

        //방어력
        if (_equipArmor != null)
        {
            _def += _equipArmor.Defence;
        }

        //삽 공격력
        if(_equipShovel != null)
        {
            _shovelPower += _equipShovel.ShovelPower;
        }
       
        //코인배수 - 콤보구현을 어디서 하냐에 따라서 거기에 적용
        //상자? 게임매니저에 넣어야될듯?
    }

    public void BaseItemEquip()
    {
        _equipShovel = (Shovel)Data.Instance.GetItemInfo(201);
        _equipWeapon = (Weapon)Data.Instance.GetItemInfo(301);
        _equipArmor = null;
        _equipPotion = null;
    }

    void Death()
    {
        GameManager.Instance.StageFail();
    }
}
