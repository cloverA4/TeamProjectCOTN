using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance = null;
    [SerializeField] GameObject _dropItem;

    //�������� ���� ����������
    [SerializeField] Transform _lobbyStartPoint;
    [SerializeField] Transform _stage1StartPoint;

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
        CreateNote();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            //��������1 ������ �����ϰ� - ���⼭ ������� �� ��Ʈ���� �׽�Ʈ
            StartCoroutine(Metronom());
            StartCoroutine(StartMusic());
        }
        if (Input.GetMouseButtonDown(1))
        {
            //��������1 ������ �����ϰ� - ���⼭ ������� �� ��Ʈ���� �׽�Ʈ
            resetNote();
            StopCoroutine(Metronom());
            StopCoroutine(StartMusic());
        }
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
    void FaidIn()
    {
        //���̵���ȿ�� ȣ��(��ο�����)
    }

    void FaidOut()
    {
        //���̵�ƿ� ȿ�� ȣ��(�������)
    }
    

    void StageLoad()
    {
        //���̵�ȿ���� ���� �� �ε����
        //�̴ϼȶ����� �����ε� �̿�

        //�÷��̾� ��ġ������ ������ ���� ������ó��.
        if (Data.Instance.Player.PlayerTransform == null)
        {
            Data.Instance.Player.PlayerTransform = GameObject.Find("Player").transform;
        }

        //�÷��̾��� ���� �ʱ�ȭ?
        Data.Instance.Player.HP = Data.Instance.Player.MaxHP;
        Data.Instance.Player.State = CharacterState.Live;

        switch (Data.Instance.NowStage)
        {
            case Stage.Lobby:
                //�÷��̾� ��ġ ����
                Data.Instance.Player.PlayerTransform = _lobbyStartPoint.transform;
                //�������� ����� ����
                break;
            case Stage.Stage1:
                //�÷��̾� ��ġ ����
                Data.Instance.Player.PlayerTransform = _stage1StartPoint.transform;
                //�������� ����� ����
                break;
            case Stage.Stage2:
                break;
            case Stage.Stage3:
                break;
            case Stage.Stage4:
                break;
            case Stage.Stage5:
                break;
            default:
                break;
        }
        
        //�� �ε�(�μ��� �� �� ���� ����)
        //���� �ε�(���� Ǯ �����, ���� ������ ���� �� �ʱ�ȭ �� ���� ����)
        //������ �ε�(��ȹ �� �۾�)
        //��ȭ �ʱ�ȭ
        
        //�ε尡 ������ ���̵�ƿ� ȣ��        
    }

    void StageStart()
    {
        //���̵�ƿ��� ���� �� �뷡,��Ʈ ����
        //���������� �´� bpm����
        
        StartCoroutine(Metronom());
        StartCoroutine(StartMusic());
    }


    public void StageFail()
    {
        //���н� ĳ���͸� ����
        Data.Instance.Player.State = CharacterState.Death;

        //�뷡�� ��Ʈ ����
        resetNote();
        StopCoroutine(Metronom());
        StopCoroutine(StartMusic());

        //UIȣ�� - �������� �����, �κ��̵�, ??? �����Ҽ��ְԲ�.
    }

    #endregion
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