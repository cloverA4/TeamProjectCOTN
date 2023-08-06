using TMPro;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private static PlayerController instance;

    //priteRenderer _spriter; // ���� ����� �ʱ�ȭ�ϱ�
    SpriteRenderer _childSpriteRenderer;
    [SerializeField] LayerMask _layerMask;
    [SerializeField] MakeFog2 _MakeFog2;

    
    
    bool _isSuccess = true;
    bool _isDubbleClick = true;

    //ĳ���� ������
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

    float _maxHP;
    public float MaxHP
    {
        get { return _maxHP; }
        set { _maxHP = value; }
    }

    int _shovelPower = 1;

    public int ShovelPower
    {
        get { return _shovelPower; }
        set { _shovelPower = value; }
    }

    int _damage = 1;

    public int Damage
    {
        get { return _damage; }
        set { _damage = value; }
    }

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
        _childSpriteRenderer = GetComponentsInChildren<SpriteRenderer>()[1];
    }

    // Update is called once per frame
    void Update()
    {
        //if (Data.Instance.Player.State == CharacterState.Live && _isSuccess && _isDubbleClick)
        if (_isSuccess && _isDubbleClick)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow)) // �� ȭ��ǥ�� �Է� �޾�����
            {
                if(GameManager.Instance.NowStage != Stage.Lobby)
                {
                    if(!GameManager.Instance.IsSuccess()) return;
                }
                MoveCharacter(Vector3.up);
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow)) // �Ʒ� ȭ��ǥ�� �Է� �޾�����
            {
                if (GameManager.Instance.NowStage != Stage.Lobby)
                {
                    if(!GameManager.Instance.IsSuccess()) return;
                }
                MoveCharacter(Vector3.down);
            }
            else if(Input.GetKeyDown(KeyCode.RightArrow)) // ������ ȭ��ǥ�� �Է� �޾�����
            {
                if (GameManager.Instance.NowStage != Stage.Lobby)
                {
                    if(!GameManager.Instance.IsSuccess()) return;
                }
                MoveCharacter(Vector3.right);
                _childSpriteRenderer.flipX = true;
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow)) // ���� ȭ��ǥ�� �Է� �޾�����
            {
                if (GameManager.Instance.NowStage != Stage.Lobby)
                {
                    if(!GameManager.Instance.IsSuccess()) return;
                }
                MoveCharacter(Vector3.left);
                _childSpriteRenderer.flipX = false;
            }
        }
    }


    void MoveCharacter(Vector3 vec)
    {
        Vector3 Temp = transform.position;
        RaycastHit2D hitdata = Physics2D.Raycast(Temp, vec, 1f, _layerMask);
        

        // �������� �������         
        if (hitdata)
        {            
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
        GetComponent<SpriteRenderer>().sortingOrder = (int)(transform.position.y - 1) * -1; // ���̾� ����ȯ
    }

    void TestmoveWay(Vector3 vec)
    {
        transform.position = Vector3.Lerp(transform.position, transform.position + vec, 1);
        _MakeFog2.UpdateFogOfWar();
        GetComponent<SpriteRenderer>().sortingOrder = (int)(transform.position.y - 1) * -1; // ���̾� ����ȯ
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
