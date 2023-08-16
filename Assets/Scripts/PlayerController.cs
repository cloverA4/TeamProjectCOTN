using UnityEngine;
using System.Collections.Generic;

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
    }

    // Update is called once per frame
    void Update()
    {
        //if (Data.Instance.Player.State == CharacterState.Live && _isSuccess && _isDubbleClick)
        if (_isSuccess && _isDubbleClick)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow)) // 위 화살표를 입력 받았을때
            {
                if (GameManager.Instance.NowStage != Stage.Lobby)
                {
                    if (!GameManager.Instance.IsSuccess()) return;
                }
                //창 공격
                //창을 가지고있다면 창스크립트에 있는 범위체크를하고
                //공격
                //if (WeaponManager.Instance.NowEquip == WeaponType.ShortSword) // 수정필요!!!!!!! Data에 Enum타입으로 ItemType으로 weapon이
                //                                                              // 있어서 사용했지만 ShortSword,GreatSword,Spear로 나눠져야함
                //{
                //}
                //if (WeaponManager.Instance.NowEquip == WeaponType.GreatSword)
                //{
                //}
                //if (WeaponManager.Instance.NowEquip == WeaponType.Spear)
                //{
                //}

                //아니라면 캐릭터 무브
                MoveCharacter(Vector3.up);
                IsX = false;
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow)) // 아래 화살표를 입력 받았을때
            {
                if (GameManager.Instance.NowStage != Stage.Lobby)
                {
                    if (!GameManager.Instance.IsSuccess()) return;
                }
                MoveCharacter(Vector3.down);
                IsX = false;
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow)) // 오른쪽 화살표를 입력 받았을때
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
            else if (Input.GetKeyDown(KeyCode.LeftArrow)) // 왼쪽 화살표를 입력 받았을때
            {
                if (GameManager.Instance.NowStage != Stage.Lobby)
                {
                    if (!GameManager.Instance.IsSuccess()) return;
                }
                MoveCharacter(Vector3.left);
                _childSpriteRenderer.flipX = false;
                //_transform.position = Vector3.Lerp(transform.)
                IsX = true;
            }
        }
    }


    void MoveCharacter(Vector3 vec)
    {
        Vector3 Temp = transform.position;
        RaycastHit2D hitdata = Physics2D.Raycast(Temp, vec, 1f, _layerMask);

        // 왼쪽으로 빔을쏘는         

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
                TestmoveWay(vec);
            }
            else if (hitdata.collider.tag == "Monster")
            {
                hitdata.collider.GetComponent<Monster>().TakeDamage(_damage);
            }
        }
        else
        {
            TestmoveWay(vec);
        }
        
    }

    void Move(Vector3 vec)
    {
        transform.position += vec;
        _MakeFog2.UpdateFogOfWar();
        GetComponent<SpriteRenderer>().sortingOrder = (int)(transform.position.y - 1) * -1; // 레이어 값변환
    }

    void TestmoveWay(Vector3 vec)
    {
        //if (_fixanime == true)
        //{
        transform.position = Vector3.Lerp(transform.position, transform.position + vec, 1);
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
        //}
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

        //최대체력 
        _maxHP = 3;
        for (int i = 0; i < UnlockSaveData.unlockCount.Count; i++)
        {
            if (UnlockSaveData.unlockCount[i].name == "HP")
            {
                _maxHP += UnlockSaveData.unlockCount[i].count;
            }
        }
        //현재체력
        if (Data.Instance.CharacterSaveData._nowHP <= 0)
        {
            _nowHp = _maxHP;
        }
        else _nowHp = Data.Instance.CharacterSaveData._nowHP;


        if(Data.Instance.CharacterSaveData._equipItemId == null)
        {
            InitEquipItem();
        }
        else
        {
            PlayerEquipItemList.Clear();
            for(int i = 0; i < Data.Instance.CharacterSaveData._equipItemId.Count; i++)
            {
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

        //코인배수 - 콤보구현을 어디서 하냐에 따라서 거기에 적용
        //상자? 게임매니저에 넣어야될듯?
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
