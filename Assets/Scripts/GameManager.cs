using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance = null;
    [SerializeField] GameObject _dropItem;

    

    //�������� ������
    Stage _nowStage = Stage.Lobby;

    public Stage NowStage
    {
        get { return _nowStage; }
        set { _nowStage = value; }
    }

    floor _nowFloor = floor.f1;

    public floor NowFloor
    {
        get { return _nowFloor; }
        set { _nowFloor = value; }
    }

    //�������� ���� ����������
    Vector3 _StartPoint;

    public Vector3 StartPoint
    {
        get { return _StartPoint; }
        set { _StartPoint = value; }
    }

    [SerializeField] UIManeger _uiManeger;

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

    void CreateItem(Transform tf)
    {
        GameObject creatrItem = Instantiate(_dropItem, tf);
        //creatrItem.GetComponent<DropItem>().Init();
    }

    //���� �Ŵ��� �ν��Ͻ��� ������ �� �ִ� ������Ƽ. static�̹Ƿ� �ٸ� Ŭ�������� ���� ȣ���� �� �ִ�.
    public static GameManager Instance
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


    // Start is called before the first frame update
    void Start()
    {
        //��ƮǮ ����
        CreateNote();
        //���� Ǯ ����
        //������ Ǯ ����

        // �ε����� �ʱ�ȭ
        InitGameData();
        // �ε����
        StageLoad();
    }

    void InitGameData()
    {
        //�ӽ� ���� ���� �ڵ�
        _nowStage = Stage.Lobby;            
        _nowFloor = floor.f1;
        _StartPoint = new Vector3(-28, 0, 0);

        PlayerController.Instance.MaxHP = 4;
        PlayerController.Instance.NowHP = PlayerController.Instance.MaxHP;
    }

    // Update is called once per frame
    void Update()
    {

    }

    #region ��Ʈ

    [SerializeField] float _bpm;
    public float BPM
    {
        get { return _bpm; }
    }

    [SerializeField] AudioSource _audio;
    [SerializeField] GameObject _note;
    [SerializeField] GameObject _notePool;
    List<GameObject> _activeNoteList = new List<GameObject>();
    List<GameObject> _pools = new List<GameObject>();

    IEnumerator StartMusic()
    {
        yield return new WaitForSeconds(3);
        _audio.Play();
        while(true)
        {
            yield return new WaitForSeconds(1);
            
            if(!_audio.isPlaying)
            {
                resetNote();
                //����ó��
                yield return null;
            }
        }
    }
    IEnumerator Metronom()
    {
        yield return new WaitForSeconds(2);
        float beatTime = 60 / _bpm;
        while (_audio.time <= _audio.clip.length - 1)
        {
            foreach (GameObject prefab in _pools)
            {
                if (!prefab.activeSelf)
                {
                    prefab.GetComponent<Note>().PlayNote();
                    _activeNoteList.Add(prefab);
                    break;
                }
            }
            yield return new WaitForSeconds(beatTime);
        }

        //ù ��Ʈ�� �������� �����Ҷ��� ���缭 �뷡�� �����ϰԲ� ��� ������ΰ�?
        //�����ð����� ��ũ�� ��� ������ΰ�?
    }

    void CreateNote() // Ǯ���� ��Ʈ �̸������
    {
        for(int i = 0; i < 20; i++)
        {
            GameObject go = Instantiate(_note, _notePool.transform);
            go.GetComponent<Note>().Init();
            go.name = "Note" + i;
            _pools.Add(go);
            
        }
    }

    void resetNote() // �뷡����, ��Ʈ���� ����
    {
        _audio.Stop();
        for (int i = 0; i < _pools.Count; i++)
        {
            _pools[i].SetActive(false);
        }
        _activeNoteList.Clear();
    }

    public void ActiveNoteRemove(GameObject note) //Ȱ����Ʈ ����Ʈ���� ����
    {
        _activeNoteList.Remove(note);
    }

    void DeleteJudgementNote()
    {
        _activeNoteList[0].SetActive(false);
        ActiveNoteRemove(_activeNoteList[0]);
    }

    public bool IsSuccess()
    {
        bool isSuccess = false;

        if (_activeNoteList.Count <= 0)
        {
            Debug.Log("����");
            return isSuccess;
        }

        RectTransform rec = _activeNoteList[0].GetComponent<RectTransform>();
        if (rec != null && rec.anchoredPosition.x < 200)
        {
            Debug.Log("����");
            isSuccess = true;
        }

        Debug.Log("����");
        DeleteJudgementNote();
        return isSuccess;
    }

    #endregion

    #region ������������

    //������ �������������� ������ ���������� ����� �Է� ��  ���̵� �Ŀ� �ش罺�������� �ε��Ҽ� �ְԲ�
    //�ش� �������� �Ա����� �Լ� ���� ����?
    public void FaidIn()
    {
        //���̵���ȿ�� ȣ��(��ο�����)
        StartCoroutine(_uiManeger.FadeIn());
    }

    public void FaidOut()
    {
        //���̵�ƿ� ȿ�� ȣ��(�������)
        StartCoroutine(_uiManeger.FadeOut());
    }


    public void StageLoad()
    {
        //���̵�ȿ���� ���� �� �ε����

        //�÷��̾��� ���� �ʱ�ȭ
        if (PlayerController.Instance.IsLive == false)
        {
            PlayerController.Instance.IsLive = true;
            PlayerController.Instance.NowHP = PlayerController.Instance.MaxHP;
        }

        //�ʱ�ȭ �� �ε�
        resetNote();
        //�������� ����� ����
        switch (_nowStage)
        {
            case Stage.Lobby:
                switch (_nowFloor)
                {
                    case floor.f1:
                        break;
                    case floor.f2:
                    case floor.f3:
                    case floor.fBoss:
                        break;
                }
                break;
            case Stage.Stage1:
                switch (_nowFloor)
                {
                    case floor.f1:
                        GameObject.Find("Fog/FogArea").GetComponent<MakeFog2>().ResetFog();
                        PlayerController.Instance.transfromUpdate(_StartPoint);
                        GameObject.Find("Fog/FogArea").GetComponent<MakeFog2>().Stage1F1UpdateFogOfWar();
                        break;
                    case floor.f2:
                    case floor.f3:
                    case floor.fBoss:
                        break;
                }
                break;
            case Stage.Stage2:
                switch (_nowFloor)
                {
                    case floor.f1:
                        break;
                    case floor.f2:
                    case floor.f3:
                    case floor.fBoss:
                        break;
                }
                break;
        }
        
        //�÷��̾� ��ġ ����
        PlayerController.Instance.transfromUpdate(_StartPoint);
        
        //�� �ε�(�μ��� �� �� ���� ����)
        GameObject.Find("WallAndDoorManager").GetComponent<WallAndDoorManager>().ResetWallAndDoor();

        //�Ȱ��ʱ�ȭ �� ��ġ ���� ,�������� �̵� �� �÷��̾� �ֺ����� �Ȱ� ����
        //GameObject.Find("Fog/FogArea").GetComponent<MakeFog2>().ResetFog();
        //GameObject.Find("Fog/FogArea").GetComponent<MakeFog2>().Stage1F1UpdateFogOfWar();
        



        //���� �ε�(���� Ǯ �����, ���� ������ ���� �� �ʱ�ȭ �� ���� ����)
        //������ �ε�(��ȹ �� �۾�)
        //��ȭ �ʱ�ȭ

        //�ε尡 ������ ���̵�ƿ� ȣ��        
        FaidOut();
    }

    public void StageStart()
    {
        //���̵�ƿ��� ���� �� �뷡,��Ʈ ����
        //���������� �´� bpm����
        switch (_nowStage)
        {
            case Stage.Lobby:
                break;
                case Stage.Stage1:
                case Stage.Stage2:
                StartCoroutine(Metronom());
                break;
        }
        StartCoroutine(StartMusic());
    }


    public void StageFail()
    {
        //���н� ĳ���͸� ����
        PlayerController.Instance.IsLive = false;
        PlayerController.Instance.NowHP = 0;

        //�뷡�� ��Ʈ ����
        resetNote();
        StopCoroutine(Metronom());
        StopCoroutine(StartMusic());

        //UIȣ�� - �������� �����, �κ��̵�, �ٽú��� �����Ҽ��ְԲ�.
    }

    #endregion

    public void PlayerHPUpdate()
    {
        //������ȣ��
        //ü�º������ ���� -> ������ ���̺굥���� �� ����
    }
}




/*
public class Pulse : MonoBehaviour
{
    [SerializeField] float _pulseSize = 1.5f;
    [SerializeField] float _returnSpeed = 5f;
    private Vector3 _startSize;

    private void Start()
    {
        _startSize = transform.localScale;
    }

    void Update()
    { // for smooth pulse movement of the object (���� ����)
        transform.localScale = Vector3.Lerp(transform.localScale, _startSize, Time.deltaTime * _returnSpeed);
    }

    public void PulseinSize()
    {  //changing size of the object
        transform.localScale = _startSize * _pulseSize;
    }
}
*/