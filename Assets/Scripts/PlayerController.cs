using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    private static PlayerController instance;

    //priteRenderer _spriter; // 변수 선언과 초기화하기
    [SerializeField] GameObject[] _weaponEffect;
    [SerializeField] MakeFog2 _MakeFog2;
    [SerializeField] SaveInfoData _unlockSaveData;

    SpriteRenderer _childSpriteRenderer;
    Animator _animator;
    AudioSource _audio;
    LayerMask _normalLayerMask;
    LayerMask _weaponCheckLayerMask;
    LayerMask _itemCheckLayerMask;

    float _lobbyMoveDelay = 0f;
    private float moveSpeed = 10f;
    private bool isMoving = false;
    bool _fixanime = false;
    bool _isLive = true;

    float[] _baseCoinMultiple = new float[3] { 1f, 1.5f, 2f };
    int _coinMultipleIndex = 0;

    public bool IsX { get; private set; }
    public SaveInfoData UnlockSaveData { get { return _unlockSaveData; } }

    //캐릭터 데이터
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
            else
            {
                _nowHp = _maxHP;
            }
            UIManeger.Instance.setHP();

            if (_nowHp <= 0)
            {
                _audio.clip = Data.Instance.SoundEffect[(int)SoundEffect.PlayerDeath];
                _audio.Play();
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
    public Shovel EquipShovel
    {
        get { return _equipShovel; }
        set
        {
            _equipShovel = value;
            UIManeger.Instance.UpdataShovel();
        }
    }

    Weapon _equipWeapon;
    public Weapon EquipWeapon
    {
        get { return _equipWeapon; }
        set
        {
            _equipWeapon = value;
            UIManeger.Instance.UpdataWeapon();
        }
    }

    Armor _equipArmor;
    public Armor EquipArmor
    {
        get { return _equipArmor; }
        set
        {
            _equipArmor = value;
            UIManeger.Instance.UpdateArmor();
        }
    }

    Potion _equipPotion;
    public Potion EquipPotion
    {
        get { return _equipPotion; }
        set
        {
            _equipPotion = value;
            UIManeger.Instance.UpdatePotion();
        }
    }

    

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
        _audio = GetComponent<AudioSource>();
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

        _itemCheckLayerMask =
                  (1 << LayerMask.NameToLayer("DropItem")) |
                  (1 << LayerMask.NameToLayer("ExitItemCheck"));

        
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.NowStage == Stage.Lobby || GameManager.Instance.NowFloor == floor.fBoss)
        {
            _lobbyMoveDelay += Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.LeftShift))
        {
            //특수기능들 구현

            //물약
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                if (EquipPotion == null) return;
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
        
        switch (EquipWeapon.weaponType)
        {
            case WeaponType.Dagger:
                RaycastHit2D hitdataTypeDagger = Physics2D.Raycast(Temp, vec, 0.5f, _weaponCheckLayerMask);
                if (hitdataTypeDagger)
                {
                    if (hitdataTypeDagger.collider.tag == "Monster")
                    {
                        switch (EquipWeapon.WeaponEffectType) 
                        {
                            case WeaponEffectType.Normal :
                                NDWeaponEffectPos(vec);
                                break;
                            case WeaponEffectType.Titanium :
                                TDWeaponEffectPos(vec);
                                break;
                        }
                        hitdataTypeDagger.collider.GetComponent<Monster>().TakeDamage(_damage);
                        return;
                    }
                    break;
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
                        switch (EquipWeapon.WeaponEffectType)
                        {
                            case WeaponEffectType.Normal:
                                NGWeaponEffectPos(vec);
                                break;
                            case WeaponEffectType.Titanium:
                                TGWeaponEffectPos(vec);
                                break;
                        }
                        collider.GetComponent<Monster>().TakeDamage(_damage);
                        IsMonster = true;
                    }
                }

                if (IsMonster) return;
                break;

            case WeaponType.Spear:
                RaycastHit2D hitdataTypeSpear = Physics2D.Raycast(Temp, vec, 2f, _weaponCheckLayerMask);
                if (hitdataTypeSpear)
                {
                    if (hitdataTypeSpear.collider.tag == "Monster")
                    {
                        if (Vector2.Distance(hitdataTypeSpear.collider.transform.position, transform.position) <= 1f)
                        {
                            switch (EquipWeapon.WeaponEffectType)
                            {
                                case WeaponEffectType.Normal:
                                    NDWeaponEffectPos(vec);
                                    break;
                                case WeaponEffectType.Titanium:
                                    TDWeaponEffectPos(vec);
                                    break;
                            }
                            hitdataTypeSpear.collider.GetComponent<Monster>().TakeDamage(_damage);
                            return;
                        }

                        switch (EquipWeapon.WeaponEffectType)
                        {
                            case WeaponEffectType.Normal:
                                NSWeaponEffectPos(vec);
                                break;
                            case WeaponEffectType.Titanium:
                                TSWeaponEffectPos(vec);
                                break;
                        }
                        Debug.Log(hitdataTypeSpear.collider.tag);
                        hitdataTypeSpear.collider.GetComponent<Monster>().TakeDamage(_damage);
                        return;
                    }
                }
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
                GameObject shovelImage = Instantiate(new GameObject());
                shovelImage.transform.position = hitdata.collider.transform.position + new Vector3(0, 0.2f, 0);
                shovelImage.AddComponent<SpriteRenderer>();
                shovelImage.GetComponent<SpriteRenderer>().sprite = EquipShovel._ItemIcon;
                Destroy( shovelImage , 0.2f);
                _audio.clip = Data.Instance.SoundEffect[(int)SoundEffect.Dig];
                _audio.Play();
            }
            else if (hitdata.collider.tag == "Box")
            {
                //상자 아이템 기능구현
                hitdata.collider.GetComponent<Box>().OpenBox();
                _audio.clip = Data.Instance.SoundEffect[(int)SoundEffect.BoxOpen];
                _audio.Play();
            }
            else if (hitdata.collider.tag == "Door") // Door이(가) 힛데이타에 태그로 들어왓다면
            {
                hitdata.collider.GetComponent<Door>().OpenDoor();
                _audio.clip = Data.Instance.SoundEffect[(int)SoundEffect.OpenDoor];
                _audio.Play();
            }
            else if (hitdata.collider.tag == "BadRock") // BadRock이 힛데이타에 태그로 들어왓다면
            {
                //hitdata.collider.GetComponent<Door>().InvincibilityWall();
                PlayerController.Instance.ResetCoinMultiple();
                GameObject shovelImage = Instantiate(new GameObject());
                shovelImage.transform.position = hitdata.collider.transform.position + new Vector3(0, 0.2f, 0);
                shovelImage.AddComponent<SpriteRenderer>();
                shovelImage.GetComponent<SpriteRenderer>().sprite = EquipShovel._ItemIcon;
                Destroy(shovelImage, 0.2f);
            }
            else if (hitdata.collider.tag == "ShopWall") // ShopWall이 힛데이타에 태그로 들어왓다면
            {
                //hitdata.collider.GetComponent<Door>().InvincibilityWall();
                PlayerController.Instance.ResetCoinMultiple();
                GameObject shovelImage = Instantiate(new GameObject());
                shovelImage.transform.position = hitdata.collider.transform.position + new Vector3(0, 0.2f, 0);
                shovelImage.AddComponent<SpriteRenderer>();
                shovelImage.GetComponent<SpriteRenderer>().sprite = EquipShovel._ItemIcon;
                Destroy(shovelImage, 0.2f);
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
                switch (dropItem.DropItemType)
                {
                    case DropItemType.Drop:
                        switch (dropItem.Item._itemType)
                        {
                            case ItemType.Currency:
                                //해당하는 재화를 상승 시키고 드랍아이템 삭제
                                Currency cr = (Currency)dropItem.Item;
                                if (cr._ItemID == 101)
                                {
                                    GameManager.Instance.Dia += cr.Count;
                                    UIManeger.Instance.IconMove(cr);
                                }
                                else if (cr._ItemID == 102)
                                {
                                    //코인배수계산
                                    float level = 0;
                                    if (PlayerPrefs.HasKey("ComboUpgradeLevel")) level = (float)PlayerPrefs.GetInt("ComboUpgradeLevel");
                                    float multiple = _baseCoinMultiple[_coinMultipleIndex] + (level * 0.5f);

                                    GameManager.Instance.Gold += Mathf.FloorToInt(cr.Count * multiple);
                                }
                                dropItem.DeleteDropItem();
                                break;
                            case ItemType.Shovel:
                            case ItemType.Weapon:
                            case ItemType.Armor:
                            case ItemType.Potion:
                                GetItem(dropItem);
                                UpdateCharacterState();
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
        //_MakeFog2.UpdateFogOfWar();
    }

    #region 무기이펙트관리
    void NDWeaponEffectPos(Vector3 vec)
    {

        if (_weaponEffect[0] != null)
        {
            // 이펙트를 생성하면서 위치 잡아주기
            GameObject NDWeaponEffect = null;

            // 위키를 입력 받았을떄 이펙트가 위 방향으로 이동
            if (vec == Vector3.up)
            {
                NDWeaponEffect = Instantiate(_weaponEffect[0], transform.position + new Vector3(0, 0.8f, 0), Quaternion.Euler(0f, 0f, 90f));
            }
            else if (vec == Vector3.down) // 아래 키를 입력 받았을떄 이펙트가 아래 방향으로 이동
            {
                NDWeaponEffect = Instantiate(_weaponEffect[0], transform.position + new Vector3(0, -0.8f, 0), Quaternion.Euler(0f, 0f, -90f));
            }
            else if (vec == Vector3.left) // 왼쪽 키를 입력 받았을떄 이펙트가 왼쪽 방향으로 이동
            {
                NDWeaponEffect = Instantiate(_weaponEffect[0], transform.position + new Vector3(-0.8f, 0.2f, 0), Quaternion.identity);
                NDWeaponEffect.GetComponent<SpriteRenderer>().flipX = true;
            }
            else if (vec == Vector3.right) // 오른쪽 키를 입력 받았을떄 이펙트가 오른쪽 방향으로 이동
            {
                NDWeaponEffect = Instantiate(_weaponEffect[0], transform.position + new Vector3(0.8f, 0.2f, 0), Quaternion.identity);
            }

            Destroy(NDWeaponEffect, 0.2f);
        }
    }
    void TDWeaponEffectPos(Vector3 vec)
    {
        if (_weaponEffect[1] != null)
        {
            // 이펙트를 생성하면서 위치 잡아주기
            GameObject NDWeaponEffect = null;

            // 위키를 입력 받았을떄 이펙트가 위 방향으로 이동
            if (vec == Vector3.up)
            {
                NDWeaponEffect = Instantiate(_weaponEffect[1], transform.position + new Vector3(0, 0.8f, 0), Quaternion.Euler(0f, 0f, 90f));
            }
            else if (vec == Vector3.down) // 아래 키를 입력 받았을떄 이펙트가 아래 방향으로 이동
            {
                NDWeaponEffect = Instantiate(_weaponEffect[1], transform.position + new Vector3(0, -0.8f, 0), Quaternion.Euler(0f, 0f, -90f));
            }
            else if (vec == Vector3.left) // 왼쪽 키를 입력 받았을떄 이펙트가 왼쪽 방향으로 이동
            {
                NDWeaponEffect = Instantiate(_weaponEffect[1], transform.position + new Vector3(-0.8f, 0.2f, 0), Quaternion.identity);
                NDWeaponEffect.GetComponent<SpriteRenderer>().flipX = true;
            }
            else if (vec == Vector3.right) // 오른쪽 키를 입력 받았을떄 이펙트가 오른쪽 방향으로 이동
            {
                NDWeaponEffect = Instantiate(_weaponEffect[1], transform.position + new Vector3(0.8f, 0.2f, 0), Quaternion.identity);
            }

            Destroy(NDWeaponEffect, 0.2f);
        }
    }
    void NGWeaponEffectPos(Vector3 vec)
    {
        if (_weaponEffect[2] != null)
        {
            // 이펙트를 생성하면서 위치 잡아주기
            GameObject NDWeaponEffect = null;

            // 위키를 입력 받았을떄 이펙트가 위 방향으로 이동
            if (vec == Vector3.up)
            {
                NDWeaponEffect = Instantiate(_weaponEffect[2], transform.position + new Vector3(0, 0.8f, 0), Quaternion.Euler(0f, 0f, 90f));
            }
            else if (vec == Vector3.down) // 아래 키를 입력 받았을떄 이펙트가 아래 방향으로 이동
            {
                NDWeaponEffect = Instantiate(_weaponEffect[2], transform.position + new Vector3(0, -0.8f, 0), Quaternion.Euler(0f, 0f, -90f));
            }
            else if (vec == Vector3.left) // 왼쪽 키를 입력 받았을떄 이펙트가 왼쪽 방향으로 이동
            {
                NDWeaponEffect = Instantiate(_weaponEffect[2], transform.position + new Vector3(-0.8f, 0.2f, 0), Quaternion.identity);
                NDWeaponEffect.GetComponent<SpriteRenderer>().flipX = true;
            }
            else if (vec == Vector3.right) // 오른쪽 키를 입력 받았을떄 이펙트가 오른쪽 방향으로 이동
            {
                NDWeaponEffect = Instantiate(_weaponEffect[2], transform.position + new Vector3(0.8f, 0.2f, 0), Quaternion.identity);
            }

            Destroy(NDWeaponEffect, 0.2f);
        }
    }
    void TGWeaponEffectPos(Vector3 vec)
    {
        if (_weaponEffect[3] != null) // 예외처리 만약 3번쨰 인덱스에 게임오브젝트가 있다면 실행 없다면 실행 x
        {
            // 이펙트를 생성하면서 위치 잡아주기
            GameObject NDWeaponEffect = null;

            // 위키를 입력 받았을떄 이펙트가 위 방향으로 이동
            if (vec == Vector3.up)
            {
                NDWeaponEffect = Instantiate(_weaponEffect[3], transform.position + new Vector3(0, 0.8f, 0), Quaternion.Euler(0f, 0f, 90f));
            }
            else if (vec == Vector3.down) // 아래 키를 입력 받았을떄 이펙트가 아래 방향으로 이동
            {
                NDWeaponEffect = Instantiate(_weaponEffect[3], transform.position + new Vector3(0, -0.8f, 0), Quaternion.Euler(0f, 0f, -90f));
            }
            else if (vec == Vector3.left) // 왼쪽 키를 입력 받았을떄 이펙트가 왼쪽 방향으로 이동
            {
                NDWeaponEffect = Instantiate(_weaponEffect[3], transform.position + new Vector3(-0.8f, 0.2f, 0), Quaternion.identity);
                NDWeaponEffect.GetComponent<SpriteRenderer>().flipX = true;
            }
            else if (vec == Vector3.right) // 오른쪽 키를 입력 받았을떄 이펙트가 오른쪽 방향으로 이동
            {
                NDWeaponEffect = Instantiate(_weaponEffect[3], transform.position + new Vector3(0.8f, 0.2f, 0), Quaternion.identity);
            }

            Destroy(NDWeaponEffect, 0.2f);
        }
    }
    void NSWeaponEffectPos(Vector3 vec)
    {
        if (_weaponEffect[4] != null)
        {
            // 이펙트를 생성하면서 위치 잡아주기
            GameObject NDWeaponEffect = null;

            // 위키를 입력 받았을떄 이펙트가 위 방향으로 이동
            if (vec == Vector3.up)
            {
                NDWeaponEffect = Instantiate(_weaponEffect[4], transform.position + new Vector3(0, 0.8f, 0), Quaternion.Euler(0f, 0f, 90f));
            }
            else if (vec == Vector3.down) // 아래 키를 입력 받았을떄 이펙트가 아래 방향으로 이동
            {
                NDWeaponEffect = Instantiate(_weaponEffect[4], transform.position + new Vector3(0, -0.8f, 0), Quaternion.Euler(0f, 0f, -90f));
            }
            else if (vec == Vector3.left) // 왼쪽 키를 입력 받았을떄 이펙트가 왼쪽 방향으로 이동
            {
                NDWeaponEffect = Instantiate(_weaponEffect[4], transform.position + new Vector3(-0.8f, 0.2f, 0), Quaternion.identity);
                NDWeaponEffect.GetComponent<SpriteRenderer>().flipX = true;
            }
            else if (vec == Vector3.right) // 오른쪽 키를 입력 받았을떄 이펙트가 오른쪽 방향으로 이동
            {
                NDWeaponEffect = Instantiate(_weaponEffect[4], transform.position + new Vector3(0.8f, 0.2f, 0), Quaternion.identity);
            }

            Destroy(NDWeaponEffect, 0.2f);
        }
    }
    void TSWeaponEffectPos(Vector3 vec)
    {
        if (_weaponEffect[5] != null)
        {
            // 이펙트를 생성하면서 위치 잡아주기
            GameObject NDWeaponEffect = null;

            // 위키를 입력 받았을떄 이펙트가 위 방향으로 이동
            if (vec == Vector3.up)
            {
                NDWeaponEffect = Instantiate(_weaponEffect[5], transform.position + new Vector3(0, 0.8f, 0), Quaternion.Euler(0f, 0f, 90f));
            }
            else if (vec == Vector3.down) // 아래 키를 입력 받았을떄 이펙트가 아래 방향으로 이동
            {
                NDWeaponEffect = Instantiate(_weaponEffect[5], transform.position + new Vector3(0, -0.8f, 0), Quaternion.Euler(0f, 0f, -90f));
            }
            else if (vec == Vector3.left) // 왼쪽 키를 입력 받았을떄 이펙트가 왼쪽 방향으로 이동
            {
                NDWeaponEffect = Instantiate(_weaponEffect[5], transform.position + new Vector3(-0.8f, 0.2f, 0), Quaternion.identity);
                NDWeaponEffect.GetComponent<SpriteRenderer>().flipX = true;
            }
            else if (vec == Vector3.right) // 오른쪽 키를 입력 받았을떄 이펙트가 오른쪽 방향으로 이동
            {
                NDWeaponEffect = Instantiate(_weaponEffect[5], transform.position + new Vector3(0.8f, 0.2f, 0), Quaternion.identity);
            }

            Destroy(NDWeaponEffect, 0.2f);
        }
    }
    #endregion

    void GetItem(DropItem dropItem)
    {
        _audio.clip = Data.Instance.SoundEffect[(int)SoundEffect.GetItem];
        _audio.Play();
        switch (dropItem.Item._itemType)
        {
            case ItemType.Shovel:
                Shovel shovel = (Shovel)dropItem.Item;
                Debug.Log($"먹을 아이템 {dropItem.Item._ItemID}");

                if (EquipShovel != null)
                {
                    //장착중인 삽이 있으면
                    Shovel temp = new Shovel();
                    temp = EquipShovel;
                    EquipShovel = shovel;
                    dropItem.ChangeItem(temp);
                    UIManeger.Instance.IconMove(EquipShovel);
                    Debug.Log($"착용한 아이템 {EquipShovel._ItemID}");
                    Debug.Log($"버려진 아이템 {dropItem.Item._ItemID}");
                    return;
                }
                else
                {
                    EquipShovel = shovel;
                    UIManeger.Instance.IconMove(EquipShovel);
                }
                break;
            case ItemType.Weapon:
                Weapon weapon = (Weapon)dropItem.Item;
                Debug.Log($"먹을 아이템 {dropItem.Item._ItemID}");

                if (EquipWeapon != null)
                {
                    Weapon temp = new Weapon();
                    temp = EquipWeapon;
                    EquipWeapon = weapon;
                    dropItem.ChangeItem(temp);
                    UIManeger.Instance.IconMove(EquipWeapon);
                    Debug.Log($"착용한 아이템 {EquipWeapon._ItemID}");
                    Debug.Log($"버려진 아이템 {dropItem.Item._ItemID}");
                    return;
                }
                else
                {
                    EquipWeapon = weapon;
                    UIManeger.Instance.IconMove(EquipWeapon);
                }
                
                break;
            case ItemType.Armor:
                Armor armor = (Armor)dropItem.Item;
                Debug.Log($"먹을 아이템 {dropItem.Item._ItemID}");
                if (EquipArmor != null)
                {
                    Armor temp = new Armor();
                    temp = EquipArmor;
                    EquipArmor = armor;
                    dropItem.ChangeItem(temp);
                    UIManeger.Instance.IconMove(EquipArmor);
                    Debug.Log($"착용한 아이템 {EquipArmor._ItemID}");
                    Debug.Log($"버려진 아이템 {dropItem.Item._ItemID}");
                    return;
                }
                else
                {
                    EquipArmor = armor;
                    UIManeger.Instance.IconMove(EquipArmor);
                }
                break;
            case ItemType.Potion:
                Potion potion = (Potion)dropItem.Item;
                Debug.Log($"먹을 아이템 {dropItem.Item._ItemID}");
                if (EquipPotion != null)
                {
                    Potion temp = new Potion();
                    temp = EquipPotion;
                    EquipPotion = potion;
                    dropItem.ChangeItem(temp);
                    UIManeger.Instance.IconMove(EquipPotion);
                    Debug.Log($"착용한 아이템 {EquipPotion._ItemID}");
                    Debug.Log($"버려진 아이템 {dropItem.Item._ItemID}");
                    return;
                }
                else
                {
                    EquipPotion = potion;
                    UIManeger.Instance.IconMove(EquipPotion);
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
            dropItem.DeleteDropItem();
        }
        else
        {
            //골드부족!
            dropItem.NotEnoughCurreny();
            ResetCoinMultiple();
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

            UpdateCharacterState();
            dropItem.DeleteDropItem();
            _audio.clip = Data.Instance.SoundEffect[(int)SoundEffect.UnLock];
            _audio.Play();
        }
        else
        {
            //다이아 부족!!
            dropItem.NotEnoughCurreny();
            ResetCoinMultiple();
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
        for (int j = 0; j < _unlockSaveData.unlockNeedDias.Count; j++)
        {
            if (_unlockSaveData.unlockNeedDias[j].level == level)
            {
                needDia = _unlockSaveData.unlockNeedDias[j].NeedDia;
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
            dropItem.DeleteDropItem();
            _audio.clip = Data.Instance.SoundEffect[(int)SoundEffect.UnLock];
            _audio.Play();
        }
        else if(needDia != -1 && GameManager.Instance.Dia < needDia)
        {
            dropItem.NotEnoughCurreny();
            Debug.Log("해금불가");
        }
    }

    private void OnDrawGizmos() //확인용
    {
        Vector2[] UDLR = { Vector2.up, Vector2.down, Vector2.left, Vector2.right };

        foreach (Vector3 urdr in UDLR)
        {
            Vector3 temp = transform.position + urdr / 2;
            RaycastHit2D hit = Physics2D.Raycast(transform.position, temp, 0.5f, _itemCheckLayerMask);
            if (hit)
            {
                Debug.DrawLine(transform.position, hit.point, Color.green);
                // 충돌한 물체가 "Item" 태그를 가진 경우
            }
        }
    }

    void Move(Vector3 vec)
    {
        transform.position += vec;


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

        if (GameManager.Instance.NowStage != Stage.Lobby && GameManager.Instance.NowFloor != floor.fBoss)
        {
            _MakeFog2.UpdateFogOfWar();
        }

        CheckItmeInfo(transform);
    


        //PlayerMoveEvent?.Invoke(this, EventArgs.Empty);
    }
    //public event EventHandler PlayerMoveEvent;


    public void CheckItmeInfo(Transform Player)
    {
        Vector2[] directions = { Vector2.up, Vector2.down, Vector2.left, Vector2.right };
        foreach (Vector2 direction in directions)
        {
            Vector2 temp = (Vector2)Player.position + direction / 2f;
            RaycastHit2D[] NearItemCheck = Physics2D.RaycastAll(temp, direction, 0.5f, _itemCheckLayerMask);

            foreach (RaycastHit2D hit in NearItemCheck)
            {
                if (hit.collider.CompareTag("Item"))
                {
                    Debug.Log("레이가 'Item' 태그를 가진 물체와 충돌했습니다: " + hit.collider.gameObject.name);
                    // UI 기능 호출
                    hit.collider.GetComponent<DropItem>().OpenItemInfo();
                }
                else if (hit.collider.CompareTag("ExitItemCheck"))
                {
                    Debug.Log("'Item' 태그를 가진 물체와 멀어졌습니다");
                    hit.collider.GetComponentInParent<DropItem>().CloseItemInfo();
                }
            }
        }
    }



    //PlayerMoveEvent?.Invoke(this, EventArgs.Empty);

    //public event EventHandler PlayerMoveEvent;
    public void transfromUpdate(Vector3 vec)
    {
        transform.position = vec;
        //PlayerMoveEvent?.Invoke(this, EventArgs.Empty);
    }

    void UsePotion()
    {
        NowHP += EquipPotion.Heal;
        EquipPotion = null;
        _audio.clip = Data.Instance.SoundEffect[(int)SoundEffect.UsePotion];
        _audio.Play();
    }

    public void InitCharacterData()
    {
        //게임메니져에서 각종 데이터 로드가 끝나면, 그때 실행

        if (Data.Instance.CharacterSaveData._equipShovelID != 0)
        {
            EquipShovel = (Shovel)Data.Instance.GetItemInfo(Data.Instance.CharacterSaveData._equipShovelID);
        }
        else
        {
            Debug.Log("기본삽 장착");
            EquipShovel = (Shovel)Data.Instance.GetItemInfo(201);
        }


        if (Data.Instance.CharacterSaveData._equipWeaponID != 0)
        {
            EquipWeapon = (Weapon)Data.Instance.GetItemInfo(Data.Instance.CharacterSaveData._equipWeaponID);
        }
        else
        {
            Debug.Log("기본무기 장착");
            EquipWeapon = (Weapon)Data.Instance.GetItemInfo(301);
        }


        if (Data.Instance.CharacterSaveData._equipArmorID != 0)
        {
            EquipArmor = (Armor)Data.Instance.GetItemInfo(Data.Instance.CharacterSaveData._equipArmorID);
        }
        else
        {
            EquipArmor = null;
        }

        if (Data.Instance.CharacterSaveData._equipPotionID != 0)
        {
            EquipPotion = (Potion)Data.Instance.GetItemInfo(Data.Instance.CharacterSaveData._equipPotionID);
        }
        else
        {
            EquipArmor = null;
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
        if (EquipWeapon != null)
        {
            _damage += EquipWeapon.Attack;
        }
        Debug.Log($"현재 공격력" + _damage);

        //방어력
        if (EquipArmor != null)
        {
            _def += EquipArmor.Defence;
        }

        //삽 공격력
        if(EquipShovel != null)
        {
            _shovelPower += EquipShovel.ShovelPower;
        }
       
        //코인배수 - 콤보구현을 어디서 하냐에 따라서 거기에 적용
        //상자? 게임매니저에 넣어야될듯?
    }

    public void UpCoinMultiple()
    {
        if (_coinMultipleIndex < _baseCoinMultiple.Length - 1)
        {
            _coinMultipleIndex++;

            float level = 0;
            if (PlayerPrefs.HasKey("ComboUpgradeLevel")) level = (float)PlayerPrefs.GetInt("ComboUpgradeLevel");
            float multiple = _baseCoinMultiple[_coinMultipleIndex] + (level * 0.5f);

            UIManeger.Instance.CoinMultipleUI(multiple, _coinMultipleIndex);
        }
    }

    public void ResetCoinMultiple()
    {
        //코인배수 초기화
        _coinMultipleIndex = 0;

        float level = 0;
        if (PlayerPrefs.HasKey("ComboUpgradeLevel")) level = (float)PlayerPrefs.GetInt("ComboUpgradeLevel");
        float multiple = _baseCoinMultiple[_coinMultipleIndex] + (level * 0.5f);

        UIManeger.Instance.CoinMultipleUI(multiple, _coinMultipleIndex);
    }

    public void BaseItemEquip()
    {
        EquipShovel = (Shovel)Data.Instance.GetItemInfo(201);
        EquipWeapon = (Weapon)Data.Instance.GetItemInfo(301);
        EquipArmor = null;
        EquipPotion = null;
    }

    public void TakeDamage(int dmg)
    {
        int damage = dmg - Def;
        _audio.clip = Data.Instance.SoundEffect[(int)SoundEffect.PlayerHit];
        _audio.Play();
        ShakeCamera.Instance.ShakeAndFlashCamera();
        if (damage > 1)
        {
            NowHP -= damage;
        }
        else
        {
            NowHP -= 1;
        }
        ResetCoinMultiple();
    }

    void Death()
    {
        GameManager.Instance.StageFail();
    }
}
