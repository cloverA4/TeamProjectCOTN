using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private static PlayerController instance;

    SpriteRenderer _spriter; // 변수 선언과 초기화하기
    [SerializeField] 
    LayerMask _layerMask;
    [SerializeField]
    MakeFog2 _MakeFog2;

    int shovelPower = 1;
    
    bool _isSuccess = true;
    bool _isDubbleClick = true;

    //캐릭터 데이터
    bool _isLive = true;
    public bool IsLive
    {
        get { return _isLive; }
        set { _isLive = value; }
    }

    float _nowHp;
    public float NowHP
    {
        get { return _nowHp; }
        set 
        {
            _nowHp = value;
            GameManager.Instance.PlayerHPUpdate();
            if (_nowHp <= 0)
            {
                _isLive = false;
                GameManager.Instance.StageFail();
            }
        }      
    }

    float _maxHP;
    public float MaxHP
    {
        get { return _maxHP; }
        set { _maxHP = value; }
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
        _spriter = GetComponent<SpriteRenderer>(); // 마찬가지로 가져오는 함수
    }

    // Update is called once per frame
    void Update()
    {
        //if (Data.Instance.Player.State == CharacterState.Live && _isSuccess && _isDubbleClick)
        if (_isSuccess && _isDubbleClick)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow)) // 위 화살표를 입력 받았을때
            {
                if(GameManager.Instance.NowStage != Stage.Lobby)
                {
                    if (!Judgement()) return;
                }
                MoveCharacter(Vector3.up);
                _MakeFog2.UpdateFogOfWar();
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow)) // 아래 화살표를 입력 받았을때
            {
                if (GameManager.Instance.NowStage != Stage.Lobby)
                {
                    if (!Judgement()) return;
                }
                MoveCharacter(Vector3.down);
                _MakeFog2.UpdateFogOfWar();
            }
            else if(Input.GetKeyDown(KeyCode.RightArrow)) // 오른쪽 화살표를 입력 받았을때
            {
                if (GameManager.Instance.NowStage != Stage.Lobby)
                {
                    if (!Judgement()) return;
                }
                MoveCharacter(Vector3.right);
                _spriter.flipX = true;
                _MakeFog2.UpdateFogOfWar();
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow)) // 왼쪽 화살표를 입력 받았을때
            {
                if (GameManager.Instance.NowStage != Stage.Lobby)
                {
                    if (!Judgement()) return;
                }
                MoveCharacter(Vector3.left);
                _spriter.flipX = false;
                _MakeFog2.UpdateFogOfWar();
            }
        }
    }

    bool Judgement()
    {
        _isDubbleClick = false;
        Invoke("DubbleLock", 0.1f);

        if (!GameManager.Instance.IsSuccess())
        {
            _isSuccess = false;
            Invoke("penalty", 60 / GameManager.Instance.BPM);
            return false;
        }
        return true;
    }

    void DubbleLock()
    {
        _isDubbleClick = true;
    }
    void penalty()
    {
        Debug.Log("패널티 해제!");
        _isSuccess = true;
        _isDubbleClick = true;
    }

    void MoveCharacter(Vector3 vec)
    {
        Vector3 Temp = transform.position + new Vector3(0, -0.5f, 0);
        RaycastHit2D hitdata = Physics2D.Raycast(Temp, vec, 1f, _layerMask);
        // 왼쪽으로 빔을쏘는         
        if (hitdata)
        {
            Debug.Log(hitdata.collider.name);
            if (hitdata.collider.tag == "WeedWall") // weedwall이 힛데이타에 태그로 들어왓다면
            {
                //Debug.Log(hitdata.collider.gameObject); // 힛데이타콜라이더게임오브젝트에 대한 정보가 출력된다
                //Destroy(hitdata.collider.gameObject); // 힛데이타콜라이더게임오브젝트를 파괴한다
                //setActive활용해서 벽부수는 표현해보기
                hitdata.collider.GetComponent<Wall>().DamageWall(shovelPower);

            }
            else if (hitdata.collider.tag == "Door") // Door이(가) 힛데이타에 태그로 들어왓다면
            {

                hitdata.collider.GetComponent<Door>().OpenDoor();
                

            }
            else if (hitdata.collider.tag == "BadRock") // weedwall이 힛데이타에 태그로 들어왓다면
            {
                //hitdata.collider.GetComponent<Door>().InvincibilityWall();
            }
            else if (hitdata.collider.tag == "Stair")
            {
                transform.position += vec;
            }
            //else if(hitdata.collider.tag == "적태그이름")
            //{
            //    공격에니메이션
            //    공격
            //    적의 체력이 낮아짐
            //    적의 체력이 0이됬을때
            //    적의 오브젝트가 부서짐?
            //}
        }
        else
        {
            transform.position += vec;
        }
    }

    public void transfromUpdate(Vector3 vec)
    {
        transform.position = vec;
    }
    void Death()
    {        
        GameManager.Instance.StageFail();
    }
}
