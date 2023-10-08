using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class GameManager : MonoBehaviour
{
    private static GameManager instance = null;

    [SerializeField] GameObject _dropItem;
    [SerializeField] MakeFog2 _MakeFog2;
    [SerializeField] GameObject _itemPool;
    [SerializeField] AudioSource _shopF2;
    [SerializeField] AudioSource _shopF3;
    //박스 리젠용 변수들
    [SerializeField] GameObject _clearBox;
    [SerializeField] GameObject _ClearBoxPos;
    [SerializeField] GameObject[] _boxSpawnPoint;
    [SerializeField] GameObject _normalBox;
    [SerializeField] GameObject _boxPool;

    public GameObject ItemPool { get { return _itemPool; } }

    //스테이지 데이터
    Stage _nowStage = Stage.Lobby;
    StageStartPosition _stageStartPosition = new StageStartPosition();
    bool _stageClear = false;

    int _gold = 0;
    public int Gold
    {
        get { return _gold; }
        set
        {
            _gold = value;
            UIManeger.Instance.UpdateGold(_gold);
        }
    }

    int _dia = 0;
    public int Dia
    {
        get { return _dia; }
        set 
        {
            _dia = value;
            UIManeger.Instance.UpdataDiamond(_dia);
        }
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

    private void Awake()
    {
        if (null == instance)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

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

    void Start()
    {        
        CreateNote();
        InitGameData();
        StageLoad(); 
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            string str = null;
            if (GameManager.instance.NowStage == Stage.Lobby)
            {
                string str2 = null;
                str = "메뉴";
                UIManeger.Instance.Option(str, "계속하기", "사운드", "조작법", "게임 종료", str2, () => UIManeger.Instance.AllCloseUI(), () => UIManeger.Instance.StartSoundOption(), UIManeger.Instance.OnControllManual, () => Application.Quit(), null);
            }
            else if (GameManager.instance.NowStage != Stage.Lobby)
            {
                str = "일시정지";
                UIManeger.Instance.Option(str, "계속하기", "사운드", "조작법", "로비로 나가기", "게임 종료", () => UIManeger.Instance.AllCloseUI(), () => UIManeger.Instance.StartSoundOption(), UIManeger.Instance.OnControllManual, LobbyAlarm, () => Application.Quit());
            }
        }
    }

    void InitGameData()
    {
        Gold = Data.Instance.CharacterSaveData._gold;
        Dia = Data.Instance.CharacterSaveData._dia;
        _nowStage = Data.Instance.CharacterSaveData._nowStage;            
        _nowFloor = Data.Instance.CharacterSaveData._nowFloor;
        PlayerController.Instance.InitCharacterData();
        UIManeger.Instance.setHP();
    }

    #region 비트

    [SerializeField] float _bpm;
    [SerializeField] AudioSource _audio;
    [SerializeField] GameObject _note;
    [SerializeField] GameObject _notePool;

    List<GameObject> _rightNoteList = new List<GameObject>();
    List<GameObject> _leftNoteList = new List<GameObject>();
    List<GameObject> _pools = new List<GameObject>();

    IEnumerator mt;
    IEnumerator sm;
    public AudioSource Audio { get { return _audio; } }
    public float BPM
    {
        get { return _bpm; }
    }


    IEnumerator StartMusic()
    {
        yield return new WaitForSeconds(3);
        _audio.Play();
        _isLastNote = false;
        if (_nowStage == Stage.Stage1 && _nowFloor == floor.f2)
        {
            _shopF2.Play();
            _shopF3.Stop();
        }
        else if (_nowStage == Stage.Stage1 && _nowFloor == floor.f3)
        {
            _shopF3.Play();
            _shopF2.Stop();
        }
        else
        {
            _shopF3.Stop();
            _shopF2.Stop();
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

    void ResetNote() // 노래중지, 노트전부 끄기
    {
        _audio.Stop();
        _shopF3.Stop();
        _shopF2.Stop();
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
        //마지막노트인지를 검사해서 마지막노트면 특정 불값을 바꿔주면 그뒤로 더이상 노트가 없다
        if (_leftNoteList.Count == 0)
        {
            _isLastNote = true;
        }
        else
        {
            _isLastNote = false;
        }
    }
    bool _isLastNote = false;
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
            if(PlayerController.Instance.NowHP > 0)
            {
                UIManeger.Instance.MissInfo();
            }
            PlayerController.Instance.ResetCoinMultiple();
            return isSuccess;
        }

        RectTransform rec = _rightNoteList[0].GetComponent<RectTransform>();
        if (rec != null && rec.anchoredPosition.x < 200)
        {
            //성공
            DeleteJudgementNote();
            isSuccess = true;
            return isSuccess;
        }

        //실패
        UIManeger.Instance.MissInfo();
        PlayerController.Instance.ResetCoinMultiple();
        return isSuccess;
    }

    public void MissBeat()
    {
        UIManeger.Instance.MissBeatInfo();
        PlayerController.Instance.ResetCoinMultiple();
    }

    public void AudioVolumControll(float value)
    {
        _audio.volume = value;
        _shopF2.volume = value;
        _shopF3.volume = value;
    }

    public void EndMusic()
    {
        if (_isLastNote)
        {
            if (_stageClear)
            {
                _nowFloor++;
                FaidIn();
            }
            else
            {
                PlayerController.Instance.NowHP = 0;
            }
        }
    }

    #endregion

    #region 스테이지관리

    public void FaidIn()
    {
        ResetNote();
        if (mt != null) StopCoroutine(mt);
        if (sm != null) StopCoroutine(sm);
        StartCoroutine(UIManeger.Instance.FadeIn());
    }

    public void FaidOut()
    {
        StartCoroutine(UIManeger.Instance.FadeOut());
    }

    public void StageLoad()
    {
        _MakeFog2.gameObject.SetActive(true); // 안개 오브젝트를 켜주는 구문

        //초기화 및 로드
        ItemClear();
        ResetNote();        
        NormalBoxSpawn();
        if (mt != null) StopCoroutine(mt);
        if(sm != null) StopCoroutine(sm);

        //현재 스테이지 데이터에 맞춰서 해당 스테이지 로딩
        switch (_nowStage)
        {
            case Stage.Lobby:
                _audio.clip = Resources.Load<AudioClip>("SoundsUpdate/Stage/StageLobby");
                _audio.loop = true;
                _stageClear = true;
                Gold = 0;
                PlayerController.Instance.BaseItemEquip();
                PlayerController.Instance.ResetCoinMultiple();
                PlayerController.Instance.transfromUpdate(_stageStartPosition.LobbyPosition);
                PlayerHpReset();

                //관리자방에서 아이템 생성
                //무기 타입들
                GameObject Item = Instantiate(_dropItem, _itemPool.transform);
                Item.transform.position = new Vector3(-48, 98, 0);
                Item.GetComponent<DropItem>().Init(Data.Instance.GetItemInfo(301));
                Item = Instantiate(_dropItem, _itemPool.transform);
                Item.transform.position = new Vector3(-46, 98, 0);
                Item.GetComponent<DropItem>().Init(Data.Instance.GetItemInfo(302));
                Item = Instantiate(_dropItem, _itemPool.transform);
                Item.transform.position = new Vector3(-44, 98, 0);
                Item.GetComponent<DropItem>().Init(Data.Instance.GetItemInfo(303));
                Item = Instantiate(_dropItem, _itemPool.transform);
                Item.transform.position = new Vector3(-42, 98, 0);
                Item.GetComponent<DropItem>().Init(Data.Instance.GetItemInfo(304));
                Item = Instantiate(_dropItem, _itemPool.transform);
                Item.transform.position = new Vector3(-40, 98, 0);
                Item.GetComponent<DropItem>().Init(Data.Instance.GetItemInfo(305));
                Item = Instantiate(_dropItem, _itemPool.transform);
                Item.transform.position = new Vector3(-38, 98, 0);
                Item.GetComponent<DropItem>().Init(Data.Instance.GetItemInfo(306));
                Item = Instantiate(_dropItem, _itemPool.transform);
                Item.transform.position = new Vector3(-36, 98, 0);
                Item.GetComponent<DropItem>().Init(Data.Instance.GetItemInfo(307));
                Item = Instantiate(_dropItem, _itemPool.transform);
                //갑옷 타입들
                Item.transform.position = new Vector3(-48, 96, 0);
                Item.GetComponent<DropItem>().Init(Data.Instance.GetItemInfo(501));
                Item = Instantiate(_dropItem, _itemPool.transform);
                Item.transform.position = new Vector3(-46, 96, 0);
                Item.GetComponent<DropItem>().Init(Data.Instance.GetItemInfo(502));
                Item = Instantiate(_dropItem, _itemPool.transform);
                //포션 타입들
                Item.transform.position = new Vector3(-44, 96, 0);
                Item.GetComponent<DropItem>().Init(Data.Instance.GetItemInfo(401));
                Item = Instantiate(_dropItem, _itemPool.transform);
                Item.transform.position = new Vector3(-42, 96, 0);
                Item.GetComponent<DropItem>().Init(Data.Instance.GetItemInfo(402));
                Item = Instantiate(_dropItem, _itemPool.transform);
                Item.transform.position = new Vector3(-40, 96, 0);
                Item.GetComponent<DropItem>().Init(Data.Instance.GetItemInfo(403));
                Item = Instantiate(_dropItem, _itemPool.transform);
                //삽 타입들
                Item.transform.position = new Vector3(-38, 96, 0);
                Item.GetComponent<DropItem>().Init(Data.Instance.GetItemInfo(201));
                Item = Instantiate(_dropItem, _itemPool.transform);
                Item.transform.position = new Vector3(-36, 96, 0);
                Item.GetComponent<DropItem>().Init(Data.Instance.GetItemInfo(202));
                Item = Instantiate(_dropItem, _itemPool.transform);

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
                        PlayerController.Instance.ResetCoinMultiple();
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

        //몬스터 로드(몬스터 풀 만들고, 현재 생성된 몬스터 다 초기화 후 새로 스폰)
        ResetMonster();
        LoadingMonster();

        //아이템 로드
        StageShopItemSpawn();

        //로드가 끝나면 페이드아웃 호출        
        FaidOut();
    }

    public void StageStart()
    {
        switch (_nowStage)
        {
            case Stage.Lobby:
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
        ResetNote();
        if (mt != null) StopCoroutine(mt);
        if (sm != null) StopCoroutine(sm);

        //UI호출 - 스테이지 재시작, 로비이동, 다시보기 선택할수있게끔.
        UIManeger.Instance.StartGoLobby();
    }

    #endregion

    #region 아이템 스폰

    //로비 스폰 포인트
    [SerializeField] GameObject _equipShop;
    [SerializeField] GameObject _potionShop;
    [SerializeField] GameObject _passivesShop;
    [SerializeField] GameObject _1stage2floorShop;
    [SerializeField] GameObject _1stage3floorShop;

    void ItemClear()
    {
        diaSpawnList.Clear();
        for (int i = 0; i < _itemPool.transform.childCount; i++)
        {
            Destroy(_itemPool.transform.GetChild(i).gameObject);
        }
    }

    void LobbyShopItemSpawn()
    {
        //장비샵
        List<int> EquipTemp = Data.Instance.LockEquipItemIDList.ToList();

        for(int i = 0; i < _equipShop.transform.childCount; i++)
        {
            if(i < Data.Instance.LockEquipItemIDList.Count)
            {
                int random = UnityEngine.Random.Range(0, EquipTemp.Count);
                GameObject SpawnItem = Instantiate(Data.Instance.ItemPrefab, _itemPool.transform);
                SpawnItem.GetComponent<DropItem>().Init(Data.Instance.GetItemInfo(EquipTemp[random]), DropItemType.UnlockShop);
                SpawnItem.transform.position = _equipShop.transform.GetChild(i).position;
                EquipTemp.RemoveAt(random);
            }
        }
        //소모품샵
        List<int> PotionTemp = Data.Instance.LockPotionList.ToList();

        for (int i = 0; i < _potionShop.transform.childCount; i++)
        {
            if (i < Data.Instance.LockPotionList.Count)
            {
                int random = UnityEngine.Random.Range(0, PotionTemp.Count);
                GameObject SpawnItem = Instantiate(Data.Instance.ItemPrefab, _itemPool.transform);
                SpawnItem.GetComponent<DropItem>().Init(Data.Instance.GetItemInfo(PotionTemp[random]), DropItemType.UnlockShop);
                SpawnItem.transform.position = _potionShop.transform.GetChild(i).position;
                PotionTemp.RemoveAt(random);
            }
        }

        //패시브샵
        List<int> PassivesTemp = Data.Instance.LockPassivesList.ToList();

        for (int i = 0; i < _passivesShop.transform.childCount; i++)
        {
            if (i < Data.Instance.LockPassivesList.Count)
            {
                int random = UnityEngine.Random.Range(0, PassivesTemp.Count);
                GameObject SpawnItem = Instantiate(Data.Instance.ItemPrefab, _itemPool.transform);
                SpawnItem.GetComponent<DropItem>().Init(Data.Instance.GetItemInfo(PassivesTemp[random]), DropItemType.UnlockShop);
                SpawnItem.transform.position = _passivesShop.transform.GetChild(i).position;
                PassivesTemp.RemoveAt(random);
            }
        }
    }

    void StageShopItemSpawn()
    {
        switch(_nowStage)
        {
            case Stage.Lobby:
                LobbyShopItemSpawn();
                break;
            case Stage.Stage1:
                switch(_nowFloor)
                {
                    case floor.f1:
                        //상점이 없음
                        break;
                    case floor.f2:
                        stageShop(_1stage2floorShop);
                        break;
                    case floor.f3:
                        stageShop(_1stage3floorShop);
                        break;
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
                        break;
                    case floor.f3:
                        break;
                    case floor.fBoss:
                        break;
                }
                break;
        }
    }

    void stageShop(GameObject stageShopPosition)
    {
        List<int> GoldShopTemp = Data.Instance.CharacterSaveData._unlockItemId.ToList();

        for (int i = 0; i < stageShopPosition.transform.childCount; i++)
        {
            if (i < Data.Instance.CharacterSaveData._unlockItemId.Count)
            {
                int random = UnityEngine.Random.Range(0, GoldShopTemp.Count);
                GameObject SpawnItem = Instantiate(Data.Instance.ItemPrefab, _itemPool.transform);
                SpawnItem.GetComponent<DropItem>().Init(Data.Instance.GetItemInfo(GoldShopTemp[random]), DropItemType.Shop);
                SpawnItem.transform.position = stageShopPosition.transform.GetChild(i).position;
                GoldShopTemp.RemoveAt(random);
            }
        }
    }
    #endregion

    #region 몬스터 제어
    public event EventHandler EventEliteMonsterDie;

    [SerializeField] Transform[] _spawnPoint1s1f;
    [SerializeField] Transform[] _spawnPoint1s2f;
    [SerializeField] Transform[] _spawnPoint1s3f;
    [SerializeField] Transform _eliteSpawnPoint1s1f;
    [SerializeField] Transform _eliteSpawnPoint1s2f;
    [SerializeField] Transform _eliteSpawnPoint1s3f;
    [SerializeField] GameObject[] _monsterPreFabs;
    [SerializeField] GameObject _eliteMonsterPrefab;
    [SerializeField] GameObject _monsterPool;

    List<Vector3> randomSpawnList = new List<Vector3>();
    List<Vector3> diaSpawnList = new List<Vector3>();

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
                    case floor.fBoss:
                        ClearBoxSpawn();
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
            for(int j = 0; j < vecs[i].GetComponent<Room>().Roomindex.Count; j++)
            {
                if(j < 4)
                {
                    int r = UnityEngine.Random.Range(0, vecs[i].GetComponent<Room>().Roomindex.Count);
                    randomSpawnList.Add(vecs[i].GetComponent<Room>().Roomindex[r]);
                    vecs[i].GetComponent<Room>().Roomindex.RemoveAt(r);
                }
                else
                {
                    diaSpawnList.Add(vecs[i].GetComponent<Room>().Roomindex[j-4]);
                }
            }
        }
        SpawnMonster();
        SpawnDropDia();
    }

    void SpawnDropDia()
    {
        GameObject go = Instantiate(Data.Instance.ItemPrefab, GameManager.Instance.ItemPool.transform);
        int randomIndex = UnityEngine.Random.Range(0, diaSpawnList.Count);
        go.transform.position = diaSpawnList[randomIndex];

        Currency cr = (Currency)Data.Instance.GetItemInfo(101);
        cr.Count = 1;
        go.GetComponent<DropItem>().Init(cr);
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

            if(index >= _monsterPreFabs.Length)
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

        go = Instantiate(_monsterPreFabs[index], _monsterPool.transform);

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
        go = Instantiate(_eliteMonsterPrefab, _monsterPool.transform);

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
        UIManeger.Instance.CenterMessage("탈출계단 잠금해제");
        EventEliteMonsterDie?.Invoke(this, EventArgs.Empty);
    }

    void ClearBoxSpawn()
    {
        GameObject ClearBox = Instantiate(_clearBox, _boxPool.transform);
        ClearBox.transform.position = _ClearBoxPos.transform.position;
        ClearBox.GetComponent<Box>().InitBox(BoxType.Clear);
    }

    void NormalBoxSpawn()
    {
        ResetBox();
        int BoxCount = 0;
        if (PlayerPrefs.HasKey("TreasureBoxUpgradeLevel"))
        {
            BoxCount = PlayerPrefs.GetInt("TreasureBoxUpgradeLevel");
        }

        switch (_nowStage)
        {
            case Stage.Stage1:
                if (_nowFloor == floor.fBoss) return;
                for (int i = 0; i < BoxCount; i++)
                {
                    GameObject go = Instantiate(_normalBox, _boxPool.transform);
                    go.transform.position = _boxSpawnPoint[(int)_nowFloor].transform.GetChild(i).transform.position;
                    go.GetComponent<Box>().InitBox(BoxType.Normal);
                }
                break;
            case Stage.Stage2:
                break;
        }
    }

    private void ResetBox()
    {
        for (int i = 0; i < _boxPool.transform.childCount; i++)
        {
            Destroy(_boxPool.transform.GetChild(i).gameObject);
        }
    }
    #endregion

    public void SoundPerse(bool isplay)
    {
        if(isplay)
        {
            _audio.UnPause();
            _shopF2.UnPause();
            _shopF3.UnPause();
        }
        else
        {
            _audio.Pause();
            _shopF2.Pause();
            _shopF3.Pause();
        }
    }

    public void PlayerHpReset()
    {
        PlayerController.Instance.IsLive = true;
        PlayerController.Instance.NowHP = PlayerController.Instance.MaxHP;
    }
    public void LobbyAlarm()
    {
        string main = "정말 로비로\r\n돌아가시겠습니까";
        string Toggle1 = "네";
        string toggle2 = "아니요";
        UIManeger.Instance.Alarm(main, Toggle1, toggle2, UIManeger.Instance.GoLobby, UIManeger.Instance.ReturnOption);
    }
}