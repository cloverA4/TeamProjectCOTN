using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using UnityEditor.ShaderGraph.Internal;

public class GameManager : MonoBehaviour
{
    private static GameManager instance = null;
    [SerializeField] GameObject _dropItem;
    [SerializeField] MakeFog2 _MakeFog2;
    [SerializeField] GameObject _itemPool;

    //�ڽ� ������ ������
    [SerializeField] GameObject _clearBox;
    [SerializeField] GameObject _ClearBoxPos;

    [SerializeField] GameObject[] _boxSpawnPoint;

    [SerializeField] GameObject _normalBox;
    [SerializeField] GameObject _boxPool;

    public GameObject ItemPool { get { return _itemPool; } }

    //�������� ������
    Stage _nowStage = Stage.Lobby;
    StageStartPosition _stageStartPosition = new StageStartPosition();
    bool _stageClear = false;

    int _gold = 0;
    int _dia = 0;

    public int Gold
    {
        get { return _gold; }
        set
        {
            _gold = value;
            _uiManeger.UpdateGold(_gold);
        }
    }

    public int Dia
    {
        get { return _dia; }
        set 
        {
            _dia = value; 
            _uiManeger.UpdataDiamond(_dia);
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
        //���� - ������ ������ (��ġ����, ���������(�κ�� �⺻), ���, ���̾�, ����ü��(�κ��Ǯ))
        //1. ����� �����Ϳ��� ��ġ���� �ҷ�����
        //2. ��Ұ� �κ� �ƴϸ� �������� ������ �� �����ص� ������ �ҷ����� -> ��������Ŭ���� �� ������������ �����Ҷ�, ���� �����ؾߵ�.
        //3. ��Ұ� �κ�� �⺻���������� �����Ű�� ���̾Ƹ� ����Ȱ� �ҷ����� ����ü���� �׳� Ǯ�� ä���ְ�
        Gold = Data.Instance.CharacterSaveData._gold;
        Dia = Data.Instance.CharacterSaveData._dia;
        _nowStage = Data.Instance.CharacterSaveData._nowStage;            
        _nowFloor = Data.Instance.CharacterSaveData._nowFloor;
        PlayerController.Instance.InitCharacterData();
        _uiManeger.setHP();
        GetEquipItem();
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

    void ResetNote() // �뷡����, ��Ʈ���� ����
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
        //monsterActionStart();
        StartCoroutine(monsterActionStartTset());
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
            _uiManeger.MissInfo();
            return isSuccess;
        }

        RectTransform rec = _rightNoteList[0].GetComponent<RectTransform>();
        if (rec != null && rec.anchoredPosition.x < 200)
        {
            //����
            DeleteJudgementNote();
            isSuccess = true;
            return isSuccess;
        }

        //����
        _uiManeger.MissInfo();
        return isSuccess;
    }

    public void MissBeat()
    {
        _uiManeger.MissBeatInfo();
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
        ItemClear();
        ResetNote();
        ResetBox();
        if (mt != null) StopCoroutine(mt);
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
                        GameObject go = Instantiate(_dropItem, _itemPool.transform);
                        go.transform.position = new Vector3(-25, 100, 0);
                        go.GetComponent<DropItem>().Init(Data.Instance.GetItemInfo(303));
                        go = Instantiate(_dropItem, _itemPool.transform);
                        go.transform.position = new Vector3(-26, 100, 0);
                        go.GetComponent<DropItem>().Init(Data.Instance.GetItemInfo(501));
                        go = Instantiate(_dropItem, _itemPool.transform);
                        go.transform.position = new Vector3(-24, 100, 0);
                        go.GetComponent<DropItem>().Init(Data.Instance.GetItemInfo(401));
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
        GameObject.Find("MapManager").GetComponent<MapManager>().ResetMapObject();

        //���� �ε�(���� Ǯ �����, ���� ������ ���� �� �ʱ�ȭ �� ���� ����)
        ResetMonster();
        LoadingMonster();

        //������ �ε�
        StageShopItemSpawn();

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
                _gold = 0;
                PlayerController.Instance.BaseItemEquip();
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
        //�뷡�� ��Ʈ ����
        ResetNote();
        if (mt != null) StopCoroutine(mt);
        if (sm != null) StopCoroutine(sm);

        //UIȣ�� - �������� �����, �κ��̵�, �ٽú��� �����Ҽ��ְԲ�.
        _uiManeger.StartGoLobbyUI();
    }

    #endregion

    #region ������ ����

    //�κ� ���� ����Ʈ
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
        //���
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
        //�Ҹ�ǰ��
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

        //�нú꼥
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
                        //������ ����
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

    #region ���� ����
    public event EventHandler EventEliteMonsterDie;

    [SerializeField] Transform[] _spawnPoint1s1f; // �渶�� �ϳ���
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
        //����Ʈ���� ����
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
        switch (_nowStage)
        {
            case Stage.Stage1:
                for (int i = 0; i < _boxSpawnPoint[(int)_nowFloor].transform.childCount; i++)
                {
                    GameObject go = Instantiate(_normalBox, _boxPool.transform);
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
            Destroy(_boxPool.transform.GetChild(i));
        }
    }


    //���� ai ����(�������� �ϳ��� �߰�)
    #endregion

    public void GetEquipItem()
    {

    }

    public void PlayerHPUpdate()
    {
        //������ȣ��
        _uiManeger.setHP();
        //ü�º������ ���� -> ������ ���̺굥���� �� ����
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