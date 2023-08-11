using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEditor.Rendering;

public class GameManager : MonoBehaviour
{
    private static GameManager instance = null;
    [SerializeField] GameObject _dropItem;
    [SerializeField] MakeFog2 _MakeFog2;


    //�������� ������
    Stage _nowStage = Stage.Lobby;
    StageStartPosition _stageStartPosition = new StageStartPosition();
    bool _stageClear = false;

    public bool StageClear
    {
        get { return _stageClear; }
        set { _stageClear = value; }
    }

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

    List<GameObject> _rightNoteList = new List<GameObject>();
    List<GameObject> _leftNoteList = new List<GameObject>();
    List<GameObject> _pools = new List<GameObject>();

    IEnumerator mt;
    IEnumerator sm;

    IEnumerator StartMusic()
    {
        yield return new WaitForSeconds(3);
        _audio.Play();
        while(true)
        {
            yield return new WaitForSeconds(1);
            if (!_audio.isPlaying)
            {
                //���������� �Ѿ�� �ڵ� ����
                //�������������� ������?
                break;
            }
        }
        yield return null;
    }
    IEnumerator Metronom()
    {
        yield return new WaitForSeconds(1);
        float beatTime = 60 / _bpm;
        while (_audio.time <= _audio.clip.length - 1)
        {
            foreach (GameObject prefab in _pools)
            {
                if (!prefab.activeSelf)
                {
                    prefab.GetComponent<Note>().PlayNote(-1, 1000);
                    _rightNoteList.Add(prefab);
                    break;
                }
            }
            foreach (GameObject prefab in _pools)
            {
                if (!prefab.activeSelf)
                {
                    prefab.GetComponent<Note>().PlayNote(1, -1000);
                    _leftNoteList.Add(prefab);
                    break;
                }
            }
            //���� �̵�            
            yield return new WaitForSeconds(beatTime);
        }
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
        _rightNoteList.Clear();
        _leftNoteList.Clear();
    }

    public void RightNoteRemove(GameObject note) //Ȱ����Ʈ ����Ʈ���� ����
    {
        _rightNoteList.Remove(note);
        //���� �ൿ�κ� - �̺�Ʈ��Ŀ��� ����������� ����
        //MosterMoveEnvent?.Invoke(this, EventArgs.Empty);
        monsterActionStart();
    }
    public void LeftNoteRemove(GameObject note) //Ȱ����Ʈ ����Ʈ���� ����
    {
        _leftNoteList.Remove(note);
    }

    void DeleteJudgementNote()
    {    
        _rightNoteList[0].SetActive(false);
        _leftNoteList[0].SetActive(false);
        RightNoteRemove(_rightNoteList[0]);
        LeftNoteRemove(_leftNoteList[0]);
    }

    public bool IsSuccess()
    {
        bool isSuccess = false;

        if (_rightNoteList.Count <= 0)
        {
            //����
            return isSuccess;
        }

        RectTransform rec = _rightNoteList[0].GetComponent<RectTransform>();
        if (rec != null && rec.anchoredPosition.x < 200)
        {
            //����
            DeleteJudgementNote();
            isSuccess = true;
        }

        //����
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
        _MakeFog2.gameObject.SetActive(true); // �Ȱ� ������Ʈ�� ���ִ� ����

        //�÷��̾��� ���� �ʱ�ȭ
        if (PlayerController.Instance.IsLive == false)
        {
            PlayerController.Instance.IsLive = true;
            PlayerController.Instance.NowHP = PlayerController.Instance.MaxHP;
        }

        //�ʱ�ȭ �� �ε�
        resetNote();
        if(mt != null) StopCoroutine(mt);
        if(sm != null) StopCoroutine(sm);
        //�������� ����� ����

        //���� ������ �����Ϳ� ���缭 �ش� �������� �ε�
        switch (_nowStage)
        {
            case Stage.Lobby:
                _audio.clip = Resources.Load<AudioClip>("SoundsUpdate/Stage/StageLobby");
                _audio.loop = true;
                _stageClear = true;
                switch (_nowFloor)
                {
                    case floor.f1:
                        PlayerController.Instance.transfromUpdate(_stageStartPosition.LobbyPosition);                        
                        break;
                    case floor.f2:
                    case floor.f3:
                    case floor.fBoss:
                        break;
                }
                break;
            case Stage.Stage1:
                _stageClear = false;
                _audio.loop = false;
                switch (_nowFloor)
                {
                    case floor.f1:                        
                        PlayerController.Instance.transfromUpdate(_stageStartPosition.Stage1F1);
                        _MakeFog2.FogOfWarStageMove();
                        _audio.clip = Resources.Load<AudioClip>("SoundsUpdate/Stage/Stage1-1");
                        break;
                    case floor.f2:
                        PlayerController.Instance.transfromUpdate(_stageStartPosition.Stage1F2);
                        _MakeFog2.FogOfWarStageMove();
                        _audio.clip = Resources.Load<AudioClip>("SoundsUpdate/Stage/Stage1-2");
                        break;
                    case floor.f3:
                        PlayerController.Instance.transfromUpdate(_stageStartPosition.Stage1F3);
                        _MakeFog2.FogOfWarStageMove();
                        _audio.clip = Resources.Load<AudioClip>("SoundsUpdate/Stage/Stage1-3");
                        break;
                    case floor.fBoss:
                        break;
                }
                break;
            case Stage.Stage2:
                _stageClear = false;
                _audio.loop = false;
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
        
        //�� �ε�(�μ��� �� �� ���� ����)
        GameObject.Find("WallAndDoorManager").GetComponent<WallAndDoorManager>().ResetWallAndDoor();

        //�Ȱ��ʱ�ȭ �� ��ġ ���� ,�������� �̵� �� �÷��̾� �ֺ����� �Ȱ� ����
        //GameObject.Find("Fog/FogArea").GetComponent<MakeFog2>().ResetFog();
        //GameObject.Find("Fog/FogArea").GetComponent<MakeFog2>().Stage1F1UpdateFogOfWar();

        //���� �ε�(���� Ǯ �����, ���� ������ ���� �� �ʱ�ȭ �� ���� ����)
        ResetMonster();
        LoadingMonster();

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
                if (mt != null)
                {
                    StopCoroutine(mt);
                }
                mt = Metronom();
                StartCoroutine(mt);
                break;
        }

        if (sm != null)
        {
            StopCoroutine(sm);
        }
        sm = StartMusic();
        StartCoroutine(sm);
    }


    public void StageFail()
    {
        //�뷡�� ��Ʈ ����
        resetNote();
        if (mt != null) StopCoroutine(mt);
        if (sm != null) StopCoroutine(sm);

        //UIȣ�� - �������� �����, �κ��̵�, �ٽú��� �����Ҽ��ְԲ�.
        _uiManeger.StartGoLobbyUI();
    }

    #endregion

    #region ���� ����
    public event EventHandler EventEliteMonsterDie;

    [SerializeField] Transform[] _spawnPoint1s1f; // �渶�� �ϳ���
    [SerializeField] Transform[] _spawnPoint1s2f;
    [SerializeField] Transform[] _spawnPoint1s3f;

    [SerializeField] Transform _eliteSpawnPoint1s1f;
    [SerializeField] Transform _eliteSpawnPoint1s2f;
    [SerializeField] Transform _eliteSpawnPoint1s3f;

    [SerializeField] GameObject[] _monster;
    [SerializeField] GameObject _eliteMonster;
    [SerializeField] GameObject _monsterPool;


    List<Vector3> randomSpawnList = new List<Vector3>();
    //���� Ǯ��/���� ����
    void LoadingMonster()
    {
        switch (NowStage)
        {
            case Stage.Stage1:
                switch (NowFloor)
                {
                    case floor.f1:
                        CreateSpawnList(_spawnPoint1s1f);
                        EliteMonsterSpawn(_eliteSpawnPoint1s1f);
                        break;
                    case floor.f2:
                        CreateSpawnList(_spawnPoint1s2f);
                        EliteMonsterSpawn(_eliteSpawnPoint1s2f);
                        break;
                    case floor.f3:
                        CreateSpawnList(_spawnPoint1s2f);
                        EliteMonsterSpawn(_eliteSpawnPoint1s2f);
                        break;
                }
                break;
            case Stage.Stage2:
                break;
        }
    }
    void CreateSpawnList(Transform[] vecs)
    {
        for (int i = 0; i < vecs.Length; i++)
        {
            vecs[i].GetComponent<Room>().CalculateRoomSize();
        }

        for (int i = 0; i < vecs.Length; i++)
        {
            for(int j = 0; j < 4; j++)
            {
                int r = UnityEngine.Random.Range(0, vecs[i].GetComponent<Room>().Roomindex.Count);
                randomSpawnList.Add(vecs[i].GetComponent<Room>().Roomindex[r]);
                vecs[i].GetComponent<Room>().Roomindex.RemoveAt(r);
            }
        }
        SpawnMonster();
    }
    void ResetMonster()
    {
        randomSpawnList.Clear();
        for (int i = 0; i < _monsterPool.transform.childCount; i++)
        {
            _monsterPool.transform.GetChild(i).gameObject.SetActive(false);
        }
    }
    void SpawnMonster()
    {
        int index = 0;

        while (randomSpawnList.Count > 0)
        {
            MonsterPooling(index);
            index++;

            if(index >= _monster.Length)
            {
                index = 0;
            }
        }
    }
    void MonsterPooling(int index)
    {
        int r = UnityEngine.Random.Range(0, randomSpawnList.Count);
        MonsterType CurrentType = (MonsterType)index;
        GameObject go;

        for (int i = 0; i < _monsterPool.transform.childCount; i++)
        {
            if (_monsterPool.transform.GetChild(i).GetComponent<Monster>().Type == CurrentType)
            {
                go = _monsterPool.transform.GetChild(i).gameObject;
            }
        }

        go = Instantiate(_monster[index], _monsterPool.transform);

        if (go != null)
        {
            go.GetComponent<Monster>().Init(CurrentType);
            go.transform.position = randomSpawnList[r];
            randomSpawnList.RemoveAt(r);
        }
    }

    void EliteMonsterSpawn(Transform transform)
    {
        GameObject go;
        for (int i = 0; i < _monsterPool.transform.childCount; i++)
        {
            if (_monsterPool.transform.GetChild(i).GetComponent<Monster>().Type == MonsterType.EliteMonster)
            {
                go = _monsterPool.transform.GetChild(i).gameObject;
                break;
            }
        }
        //����Ʈ���� ����
        go = Instantiate(_eliteMonster, _monsterPool.transform);

        if (go != null)
        {
            go.GetComponent<Monster>().Init(MonsterType.EliteMonster);
            go.transform.position = transform.position;
        }
    }

    void monsterActionStart()
    {
        for(int i = 0; i < _monsterPool.transform.childCount; i++)
        {
            if(_monsterPool.transform.GetChild(i).gameObject.activeSelf)
            {
                Monster mo = _monsterPool.transform.GetChild(i).GetComponent<Monster>();
                if(mo != null)
                {
                    mo.MonsterMove();
                }
            }
        }
    }

    public void EliteMonsterDie()
    {
        EventEliteMonsterDie?.Invoke(this, EventArgs.Empty);
        _stageClear = true;
    }

    //���� ai ����(�������� �ϳ��� �߰�)
    #endregion

    public void PlayerHPUpdate()
    {
        //������ȣ��
        _uiManeger.setHP();
        //ü�º������ ���� -> ������ ���̺굥���� �� ����
        SaveData saveData = new SaveData(PlayerController.Instance.NowHP, PlayerController.Instance.MaxHP, PlayerController.Instance.transform.position);

    }
}




/* //���� Ŀ���� �۾����� �ϰ� ����� �ڵ�
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