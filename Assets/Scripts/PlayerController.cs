using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private static PlayerController instance;

    SpriteRenderer _spriter; // ���� ����� �ʱ�ȭ�ϱ�
    [SerializeField] 
    LayerMask _layerMask;
    [SerializeField]
    MakeFog2 _MakeFog2;

    int shovelPower = 1;
    
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
        _spriter = GetComponent<SpriteRenderer>(); // ���������� �������� �Լ�
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
                    if (!Judgement()) return;
                }
                MoveCharacter(Vector3.up);
                _MakeFog2.UpdateFogOfWar();
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow)) // �Ʒ� ȭ��ǥ�� �Է� �޾�����
            {
                if (GameManager.Instance.NowStage != Stage.Lobby)
                {
                    if (!Judgement()) return;
                }
                MoveCharacter(Vector3.down);
                _MakeFog2.UpdateFogOfWar();
            }
            else if(Input.GetKeyDown(KeyCode.RightArrow)) // ������ ȭ��ǥ�� �Է� �޾�����
            {
                if (GameManager.Instance.NowStage != Stage.Lobby)
                {
                    if (!Judgement()) return;
                }
                MoveCharacter(Vector3.right);
                _spriter.flipX = true;
                _MakeFog2.UpdateFogOfWar();
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow)) // ���� ȭ��ǥ�� �Է� �޾�����
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
        Debug.Log("�г�Ƽ ����!");
        _isSuccess = true;
        _isDubbleClick = true;
    }

    void MoveCharacter(Vector3 vec)
    {
        Vector3 Temp = transform.position + new Vector3(0, -0.5f, 0);
        RaycastHit2D hitdata = Physics2D.Raycast(Temp, vec, 1f, _layerMask);
        // �������� �������         
        if (hitdata)
        {
            Debug.Log(hitdata.collider.name);
            if (hitdata.collider.tag == "WeedWall") // weedwall�� ������Ÿ�� �±׷� ���Ӵٸ�
            {
                //Debug.Log(hitdata.collider.gameObject); // ������Ÿ�ݶ��̴����ӿ�����Ʈ�� ���� ������ ��µȴ�
                //Destroy(hitdata.collider.gameObject); // ������Ÿ�ݶ��̴����ӿ�����Ʈ�� �ı��Ѵ�
                //setActiveȰ���ؼ� ���μ��� ǥ���غ���
                hitdata.collider.GetComponent<Wall>().DamageWall(shovelPower);

            }
            else if (hitdata.collider.tag == "Door") // Door��(��) ������Ÿ�� �±׷� ���Ӵٸ�
            {

                hitdata.collider.GetComponent<Door>().OpenDoor();
                

            }
            else if (hitdata.collider.tag == "BadRock") // weedwall�� ������Ÿ�� �±׷� ���Ӵٸ�
            {
                //hitdata.collider.GetComponent<Door>().InvincibilityWall();
            }
            else if (hitdata.collider.tag == "Stair")
            {
                transform.position += vec;
            }
            //else if(hitdata.collider.tag == "���±��̸�")
            //{
            //    ���ݿ��ϸ��̼�
            //    ����
            //    ���� ü���� ������
            //    ���� ü���� 0�̉�����
            //    ���� ������Ʈ�� �μ���?
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
