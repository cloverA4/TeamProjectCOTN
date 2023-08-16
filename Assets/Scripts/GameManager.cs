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


    //스테이지 데이터
    Stage _nowStage = Stage.Lobby;
    StageStartPosition _stageStartPosition = new StageStartPosition();
    bool _stageClear = false;

    int _gold = 0;
    int _dia = 0;

    public int Gold
    {
        get { return _gold; }
        set { _gold = value; }
    }

    public int Dia
    {
        get { return _dia; }
        set { _dia = value; }
    }

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

    void CreateItem(Transform tf)
    {
        GameObject creatrItem = Instantiate(_dropItem, tf);
        //creatrItem.GetComponent<DropItem>().Init();
    }

    //게임 매니저 인스턴스에 접근할 수 있는 프로퍼티. static이므로 다른 클래스에서 맘껏 호출할 수 있다.
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
        //노트풀 생성
        CreateNote();
        //몬스터 풀 생성
        //아이템 풀 생성

        // 로드전에 초기화
        InitGameData();
        // 로드시작
        StageLoad();
    }

    void InitGameData()
    {
        //정리 - 저장할 데이터 (위치정보, 착용아이템(로비면 기본), 골드, 다이아, 현재체력(로비면풀))
        //1. 저장된 데이터에서 위치정보 불러오기
        //2. 장소가 로비가 아니면 스테이지 시작할 때 저장해둔 데이터 불러오기 -> 스테이지클리어 후 다음스테이지 입장할때, 정보 저장해야됨.
        //3. 장소가 로비면 기본장착아이템 착용시키고 다이아만 저장된거 불러오고 현재체력은 그냥 풀로 채워주고
        _gold = Data.Instance.CharacterSaveData._gold;
        _dia = Data.Instance.CharacterSaveData._dia;
        _nowStage = Data.Instance.CharacterSaveData._nowStage;            
        _nowFloor = Data.Instance.CharacterSaveData._nowFloor;
        PlayerController.Instance.InitCharacterData();
    }

    #region 비트

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
                if(_stageClear)
                {
                    _nowFloor++;
                    FaidIn();
                }
                else
                {
                    PlayerController.Instance.NowHP = 0;
                }
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
            //몬스터 이동            
            yield return new WaitForSeconds(beatTime);
        }
    }

    void CreateNote() // 풀링용 노트 미리만들기
    {
        for(int i = 0; i < 20; i++)
        {
            GameObject go = Instantiate(_note, _notePool.transform);
            go.GetComponent<Note>().Init();
            go.name = "Note" + i;
            _pools.Add(go);
        }
    }

    void resetNote() // 노래중지, 노트전부 끄기
    {
        _audio.Stop();
        for (int i = 0; i < _pools.Count; i++)
        {
            _pools[i].SetActive(false);
        }
        _rightNoteList.Clear();
        _leftNoteList.Clear();
    }

    public void RightNoteRemove(GameObject note) //활성노트 리스트에서 제거
    {
        _rightNoteList.Remove(note);
        //몬스터 행동부분 - 이벤트방식에서 순차방식으로 변경
        //MosterMoveEnvent?.Invoke(this, EventArgs.Empty);
        //monsterActionStart();
        StartCoroutine(monsterActionStartTset());
    }
    public void LeftNoteRemove(GameObject note) //활성노트 리스트에서 제거
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
            //실패
            return isSuccess;
        }

        RectTransform rec = _rightNoteList[0].GetComponent<RectTransform>();
        if (rec != null && rec.anchoredPosition.x < 200)
        {
            //성공
            DeleteJudgementNote();
            isSuccess = true;
        }

        //실패
        return isSuccess;
    }

    #endregion

    #region 스테이지관리

    //데이터 스테이지변수에 변경할 스테이지가 어딘지 입력 후  페이드 후에 해당스테이지를 로드할수 있게끔
    //해당 스테이지 입구별로 함수 개별 적용?
    public void FaidIn()
    {
        //페이드인효과 호출(어두워지게)
        StartCoroutine(_uiManeger.FadeIn());
    }

    public void FaidOut()
    {
        //페이드아웃 효과 호출(밝아지게)
        StartCoroutine(_uiManeger.FadeOut());
    }

    
    public void StageLoad()
    {
        //페이드효과가 끝난 후 로드시작
        _MakeFog2.gameObject.SetActive(true); // 안개 오브젝트를 켜주는 구문

        //플레이어의 상태 초기화
        if (PlayerController.Instance.IsLive == false)
        {
            PlayerController.Instance.IsLive = true;
            PlayerController.Instance.NowHP = PlayerController.Instance.MaxHP;
        }

        //초기화 및 로드
        resetNote();
        if(mt != null) StopCoroutine(mt);
        if(sm != null) StopCoroutine(sm);
        //스테이지 배경음 설정

        //현재 스테이 데이터에 맞춰서 해당 스테이지 로딩
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
                        PlayerController.Instance.transfromUpdate(_stageStartPosition.Stage1FBoss);
                        _audio.clip = Resources.Load<AudioClip>("SoundsUpdate/Stage/StageLobby");
                        _audio.loop = true;
                        _stageClear = true;
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
        
        //벽 로드(부서진 벽 등 전부 리셋)
        GameObject.Find("MapManager").GetComponent<MapManager>().ResetMapObject();

        //안개초기화 및 위치 변경 ,스테이지 이동 후 플레이어 주변으로 안개 걷음
        //GameObject.Find("Fog/FogArea").GetComponent<MakeFog2>().ResetFog();
        //GameObject.Find("Fog/FogArea").GetComponent<MakeFog2>().Stage1F1UpdateFogOfWar();

        //몬스터 로드(몬스터 풀 만들고, 현재 생성된 몬스터 다 초기화 후 새로 스폰)
        ResetMonster();
        LoadingMonster();

        //아이템 로드(기획 후 작업)
        //재화 초기화

        //로드가 끝나면 페이드아웃 호출        
        FaidOut();
    }

    public void StageStart()
    {
        //페이드아웃이 끝난 후 노래,비트 시작
        //스테이지에 맞는 bpm설정

        switch (_nowStage)
        {
            case Stage.Lobby:
                _gold = 0;
                PlayerController.Instance.InitEquipItem();
                break;
            case Stage.Stage1:
            case Stage.Stage2:
                
                switch (_nowFloor)
                {
                    case floor.f1:
                    case floor.f2:
                    case floor.f3:
                        if (mt != null)
                        {
                            StopCoroutine(mt);
                        }
                        mt = Metronom();
                        StartCoroutine(mt);
                        break;

                    case floor.fBoss:
                        break;
                }                
                break;
        }

        if (sm != null)
        {
            StopCoroutine(sm);
        }
        sm = StartMusic();
        StartCoroutine(sm);

        Data.Instance.SavePlayerData();
    }


    public void StageFail()
    {
        //노래와 비트 중지
        resetNote();
        if (mt != null) StopCoroutine(mt);
        if (sm != null) StopCoroutine(sm);

        //UI호출 - 스테이지 재시작, 로비이동, 다시보기 선택할수있게끔.
        _uiManeger.StartGoLobbyUI();
    }

    #endregion

    #region 몬스터 제어
    public event EventHandler EventEliteMonsterDie;

    [SerializeField] Transform[] _spawnPoint1s1f; // 방마다 하나씩
    [SerializeField] Transform[] _spawnPoint1s2f;
    [SerializeField] Transform[] _spawnPoint1s3f;

    [SerializeField] Transform _eliteSpawnPoint1s1f;
    [SerializeField] Transform _eliteSpawnPoint1s2f;
    [SerializeField] Transform _eliteSpawnPoint1s3f;

    [SerializeField] GameObject[] _monster;
    [SerializeField] GameObject _eliteMonster;
    [SerializeField] GameObject _monsterPool;


    List<Vector3> randomSpawnList = new List<Vector3>();
    //몬스터 풀링/스폰 구현
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
                        CreateSpawnList(_spawnPoint1s3f);
                        EliteMonsterSpawn(_eliteSpawnPoint1s3f);
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
        //엘리트몬스터 스폰
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

    IEnumerator monsterActionStartTset()
    {
        for (int i = 0; i < _monsterPool.transform.childCount; i++)
        {
            if (_monsterPool.transform.GetChild(i).gameObject.activeSelf)
            {
                Monster mo = _monsterPool.transform.GetChild(i).GetComponent<Monster>();
                if (mo != null)
                {
                    mo.MonsterMove();
                    yield return null;
                }
            }
        }
    }

    public void EliteMonsterDie()
    {
        _stageClear = true;
        EventEliteMonsterDie?.Invoke(this, EventArgs.Empty);
    }

    //몬스터 ai 구현(종류별로 하나씩 추가)
    #endregion

    public void PlayerHPUpdate()
    {
        //유아이호출
        _uiManeger.setHP();
        //체력변경사항 저장 -> 데이터 세이브데이터 쪽 수정
    }
}




/* //심장 커졌다 작아졌다 하게 만드는 코드
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
    { // for smooth pulse movement of the object (선형 보간)
        transform.localScale = Vector3.Lerp(transform.localScale, _startSize, Time.deltaTime * _returnSpeed);
    }

    public void PulseinSize()
    {  //changing size of the object
        transform.localScale = _startSize * _pulseSize;
    }
}
*/