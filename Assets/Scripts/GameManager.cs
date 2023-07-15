using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance = null;
    [SerializeField] GameObject _dropItem;

    

    //스테이지 데이터
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

    //스테이지 관련 지역변수들
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
        //임시 강제 수정 코드
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

    #region 비트

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
                //죽음처리
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

        //첫 노트가 판정선에 도착할때에 맞춰서 노래를 시작하게끔 어떻게 만들것인가?
        //판정시간과의 싱크는 어떻게 맞출것인가?
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
        _activeNoteList.Clear();
    }

    public void ActiveNoteRemove(GameObject note) //활성노트 리스트에서 제거
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
            Debug.Log("실패");
            return isSuccess;
        }

        RectTransform rec = _activeNoteList[0].GetComponent<RectTransform>();
        if (rec != null && rec.anchoredPosition.x < 200)
        {
            Debug.Log("성공");
            isSuccess = true;
        }

        Debug.Log("실패");
        DeleteJudgementNote();
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

        //플레이어의 상태 초기화
        if (PlayerController.Instance.IsLive == false)
        {
            PlayerController.Instance.IsLive = true;
            PlayerController.Instance.NowHP = PlayerController.Instance.MaxHP;
        }

        //초기화 및 로드
        resetNote();
        //스테이지 배경음 설정
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
        
        //플레이어 위치 변경
        PlayerController.Instance.transfromUpdate(_StartPoint);
        
        //벽 로드(부서진 벽 등 전부 리셋)
        GameObject.Find("WallAndDoorManager").GetComponent<WallAndDoorManager>().ResetWallAndDoor();

        //안개초기화 및 위치 변경 ,스테이지 이동 후 플레이어 주변으로 안개 걷음
        //GameObject.Find("Fog/FogArea").GetComponent<MakeFog2>().ResetFog();
        //GameObject.Find("Fog/FogArea").GetComponent<MakeFog2>().Stage1F1UpdateFogOfWar();
        



        //몬스터 로드(몬스터 풀 만들고, 현재 생성된 몬스터 다 초기화 후 새로 스폰)
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
        //실패시 캐릭터를 죽음
        PlayerController.Instance.IsLive = false;
        PlayerController.Instance.NowHP = 0;

        //노래와 비트 중지
        resetNote();
        StopCoroutine(Metronom());
        StopCoroutine(StartMusic());

        //UI호출 - 스테이지 재시작, 로비이동, 다시보기 선택할수있게끔.
    }

    #endregion

    public void PlayerHPUpdate()
    {
        //유아이호출
        //체력변경사항 저장 -> 데이터 세이브데이터 쪽 수정
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
    { // for smooth pulse movement of the object (선형 보간)
        transform.localScale = Vector3.Lerp(transform.localScale, _startSize, Time.deltaTime * _returnSpeed);
    }

    public void PulseinSize()
    {  //changing size of the object
        transform.localScale = _startSize * _pulseSize;
    }
}
*/