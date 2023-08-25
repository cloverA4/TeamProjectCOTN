using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.Pool;
using UnityEngine.UI;

public class UIManeger : MonoBehaviour
{
    [SerializeField] GameObject _heart;
    [SerializeField] Transform _HeartBase;

    [SerializeField] Text _goldCount;
    [SerializeField] Text _diamondCount;

    int _goLobbyIndex = 0;

    [SerializeField] Image _shovelImage;
    [SerializeField] Image _armorImage;
    [SerializeField] Image _weaponImage;
    [SerializeField] GameObject _emptyPotion;

    [SerializeField] Image _fade;

    [SerializeField] GameObject _goLobbyUI;
    [SerializeField] GameObject _lobbyToggle;
    [SerializeField] GameObject _retryToggle;
    [SerializeField] GameObject _replayToggle;

    [SerializeField] GameObject _alarmUI;
    [SerializeField] GameObject _useDiamondToggle;
    [SerializeField] GameObject _retryToggle2;

    [SerializeField] Canvas _infoCanvas;
    [SerializeField] GameInfoMassege _gameInfoMassege;
    [SerializeField] Transform _gameInfoBase;

    [SerializeField] GameObject Judgement;
    [SerializeField] MissInfoMassege _missInfo;
    [SerializeField] Transform _InfoBase;
    int _maxInfoCount = 10;



    private void Update()
    {
        if (_goLobbyUI.activeSelf) 
        {
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                if(_goLobbyIndex == 0)
                {
                    _goLobbyIndex = 1;
                }
                else if(_goLobbyIndex == 1)
                {
                    _goLobbyIndex = 2;
                }
                else if(_goLobbyIndex == 2)
                {
                    _goLobbyIndex = 0;
                }
                SelectToggle();
            }
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                if (_goLobbyIndex == 0)
                {
                    _goLobbyIndex = 2;
                }
                else if (_goLobbyIndex == 1)
                {
                    _goLobbyIndex = 0;
                }
                else if (_goLobbyIndex == 2)
                {
                    _goLobbyIndex = 1;
                }
                SelectToggle();
            }
            if (_lobbyToggle.GetComponent<Toggle>().isOn)
            {
                _goLobbyIndex = 0;
            }
            else if (_retryToggle.GetComponent<Toggle>().isOn)
            {
                _goLobbyIndex = 1;
            }
            else if (_replayToggle.GetComponent<Toggle>().isOn)
            {
                _goLobbyIndex = 2;
            }
            
        }
        if (_alarmUI.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.UpArrow))
            {
                if (_alarmIndex == 0) _alarmIndex = 1;
                else _alarmIndex = 0;
                SelectToggle();
            }
            if (_useDiamondToggle.GetComponent<Toggle>().isOn)
            {
                _alarmIndex = 0;
            }
            else if (_retryToggle2.GetComponent<Toggle>().isOn)
            {
                _alarmIndex = 1;
            }
            
        }
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (_alarmUI.activeSelf)
            {
                switch (_alarmIndex)
                {
                    case 0:
                        GameManager.Instance.NowStage = Stage.Lobby;
                        GameManager.Instance.NowFloor = floor.f1;
                        EndAlarmUI();
                        StartCoroutine(FadeIn());
                        break;
                    case 1:
                        GameManager.Instance.NowFloor = floor.f1;
                        GameManager.Instance.Gold = 0;
                        PlayerController.Instance.InitEquipItem();
                        EndAlarmUI();
                        StartCoroutine(FadeIn());
                        break;
                }

            }
            if (_goLobbyUI.activeSelf)
            {
                switch (_goLobbyIndex)
                {
                    case 0:
                        GameManager.Instance.NowStage = Stage.Lobby;
                        GameManager.Instance.NowFloor = floor.f1;
                        GameManager.Instance.Gold = 0;
                        StartCoroutine(FadeIn());
                        endGoLobbyUI();
                        break;
                    case 1:
                        StartAlarmUI();
                        endGoLobbyUI();
                        break;
                    case 2:  // replay 기능 추후 구현
                        break;
                }
            }
        }
    }

    private void Start()
    {
        UIInit();
        InfoPool = new ObjectPool<MissInfoMassege>(CreatePool, OnGet, OnReleaseInfo, DestroyInfo, maxSize: 15);
        GameInfoPool = new ObjectPool<GameInfoMassege>(CreateGameInfoPool, OnGet, OnReleaseInfo, DestroyInfo, maxSize: 15);
        SpawnInfo();
        SpawnGameInfo();
    }

    public void UIInit()
    {
        gameObject.SetActive(true);
        _fade.gameObject.SetActive(true);
        _alarmUI.gameObject.SetActive(false);
        _goLobbyUI.SetActive(false);
    }
    #region HP
    List<GameObject> hearts = new List<GameObject>();
    public void setHP()
    {
        ResetHP();

        for (int i = 0; i < PlayerController.Instance.MaxHP / 2; i++) // 맥스 HP값
        {
            var temp =Instantiate(_heart, _HeartBase);
            hearts.Add(temp);
        }
        for(int i = 0; i< PlayerController.Instance.NowHP / 2; i++)  // 현재 HP값
        {
            hearts[i].GetComponent<HeartPrefeb>().FullHeartActive();
            if(i > PlayerController.Instance.MaxHP/2)
            {
                return;
            }
        }
        if(PlayerController.Instance.NowHP / 2 == 1)  // 반칸 하트 구현
        {
            //hearts[PlayerController.Instance.NowHP/2].GetComponent<HeartPrefeb>().HalfHeartActive();
        }
    }

    void ResetHP()
    {
        int index = hearts.Count;
        if (hearts != null)
        {
            for (int i = 0; i < index; i++)
            {
                Destroy(hearts[0]);
                hearts.RemoveAt(0);
            }
        }
        else return;
    }

    #endregion

    #region Wealth
    public void UpdateGold(int _count)
    {
        _goldCount.text = "X " + _count;
    }

    public void UpdataDiamond(int _count)
    {
        _diamondCount.text = "X " + _count;
    }

    #endregion

    #region Equipment

    public void Equipment()  // 아이템을 회득했을때나 아이템이 바뀔때 선언
    {
        // 처음에 모든 아이콘을 꺼주면서 초기화
        // 아이템 장착 리스트를 전부 검사하면서 어떤 아이템 유무 확인
        // 장착이 돼어있는 아이템은 아이템 이미지를 켜주고 장착 아이템에 맞게 이미지 교체
        EquipmentInit();
        var temp = PlayerController.Instance.PlayerEquipItemList;
        for (int i = 0; i < temp.Count; i++)
        {
            if (temp[i]._itemType == ItemType.Weapon)
            {
                _weaponImage.GetComponent<Image>().sprite = temp[i]._ItemIcon;
                _weaponImage.gameObject.SetActive(true);
            }
            else if (temp[i]._itemType == ItemType.Armor)
            {
                _armorImage.GetComponent<Image>().sprite = temp[i]._ItemIcon;
                _armorImage.gameObject.SetActive(true);
            }
            else if (temp[i]._itemType == ItemType.Shovel)
            {
                _shovelImage.GetComponent<Image>().sprite = temp[i]._ItemIcon;
                _shovelImage.gameObject.SetActive(true);
            }
        }

    }

    public void EquipmentInit()
    {
        _shovelImage.gameObject.SetActive(false);
        _weaponImage.gameObject.SetActive(false);
        _armorImage.gameObject.SetActive(false);
    }

    public void PotionCount(bool _potion)
    {
        if (_potion == true) _emptyPotion.SetActive(false);
        else _emptyPotion.SetActive(true);
    }

    #endregion

    #region NoteCreate

    void createNote()
    {

    }

    #endregion

    #region FadeIn , FadeOut
    public IEnumerator FadeIn()
    {
        Color color = _fade.color;
        while (true)
        {
            color.a += Time.deltaTime;
            _fade.color = color;
            yield return null;
            if (color.a >= 1) break;
        }
        GameManager.Instance.StageLoad();
    }

    public IEnumerator FadeOut()
    {
        Color color = _fade.color;
        while (true)
        {
            color.a -= Time.deltaTime;
            _fade.color = color;
            yield return null;
            if (color.a <= 0) break;
        }
        GameManager.Instance.StageStart();
    }


    #endregion

    #region Lobby and Retry UI

    public void endGoLobbyUI()
    {
        _goLobbyUI.SetActive(false);
    }

    public void StartGoLobbyUI()
    {
        _goLobbyIndex = 0;
        _goLobbyUI.SetActive(true);
        SelectToggle();
    }

    void SelectToggle()
    {
        if (_goLobbyUI.activeSelf)
        {
            switch (_goLobbyIndex)
            {
                case 0:
                    _lobbyToggle.GetComponent<Toggle>().isOn = true;
                    break;
                case 1:
                    _retryToggle.GetComponent<Toggle>().isOn = true;
                    break;
                case 2:  // replay 기능 추후 구현
                    _replayToggle.GetComponent<Toggle>().isOn = true;
                    break;
            }
           
        }
        if(_alarmUI.activeSelf)
        {
            switch (_alarmIndex)
            {
                case 0:
                    _useDiamondToggle.GetComponent<Toggle>().isOn = true;
                    break; 
                case 1:
                    _retryToggle2.GetComponent<Toggle>().isOn = true;
                    break;
            }
        }
       
    }

    [SerializeField] GameObject toggle1S;

    [SerializeField] GameObject toggle2S;

    [SerializeField] GameObject toggle3S;
    
    public void GoLobbySelect(bool _bool)
    {
        if (_bool) toggle1S.gameObject.SetActive(false);
        else toggle1S.gameObject.SetActive(true);
    }
    public void RetrySelect(bool _bool)
    {
        if (_bool) toggle2S.gameObject.SetActive(false);
        else toggle2S.gameObject.SetActive(true);
    }
    public void ReplaySelect(bool _bool)
    {
        if (_bool) toggle3S.gameObject.SetActive(false);
        else toggle3S.gameObject.SetActive(true);
    }

    #endregion

    #region Alarm UI
    [SerializeField] GameObject alarmText1;
    [SerializeField] GameObject alarmText2;
    int _alarmIndex = 0;
    public void StartAlarmUI()
    {
        _alarmIndex = 0;
        _alarmUI.SetActive(true);
        alarmText2.gameObject.SetActive(true);
        SelectToggle();
    }
    public void EndAlarmUI()
    {
        _alarmUI.SetActive(false);
        _alarmIndex = 0;
    }

    public void UseDiamondSelect(bool _bool)
    {
        if (_bool) alarmText1.gameObject.SetActive(false);
        else alarmText1.gameObject.SetActive(true);
    }
    public void RetrySelect2(bool _bool)
    {
        if (_bool) alarmText2.gameObject.SetActive(false);
        else alarmText2.gameObject.SetActive(true);
    }



    #endregion

    #region JudgementInfo

    private IObjectPool<MissInfoMassege> InfoPool;

    public List<MissInfoMassege> SpawnInfo()
    {
        List<MissInfoMassege> _infoList = new List<MissInfoMassege> (_maxInfoCount);
        for(int i = 0; i < _maxInfoCount; i++)
        {
            MissInfoMassege info = InfoPool.Get();
            info.GetComponent<MissInfoMassege>().Init();
            _infoList.Add(info);
        }
        return _infoList;
    }

    private MissInfoMassege CreatePool()
    {
        MissInfoMassege _info = Instantiate(_missInfo, _InfoBase);
        _info.SetPool(InfoPool);
        return _info;
    }

    private void OnGet(MissInfoMassege _info) //  풀 활성화
    {
        _info.gameObject.SetActive(true);
    }
    private void OnReleaseInfo(MissInfoMassege _info) // 풀 비활성화
    {
        _info.gameObject.SetActive(false);
    }
    private void DestroyInfo(MissInfoMassege _info) // 풀 삭제
    {
        Destroy(_info);
    }

    public void MissInfo()  // "빗나감!" 텍스트 출력
    {
        if(_goLobbyUI.activeSelf == false && _alarmUI.activeSelf == false)
        {
            MissInfoMassege info = InfoPool.Get();
            info.GetComponent<Text>().text = "빗나감!";
        }
    }
    public void MissBeatInfo()  // "박자 놓침!" 텍스트 출력
    {
        MissInfoMassege info = InfoPool.Get();
        info.GetComponent<Text>().text = "박자 놓침!";
    }


    #endregion

    #region PlayingGameInfo
    private IObjectPool<GameInfoMassege> GameInfoPool;

    public List<GameInfoMassege> SpawnGameInfo()
    {
        List<GameInfoMassege> _infoList = new List<GameInfoMassege>(_maxInfoCount);
        for (int i = 0; i < _maxInfoCount; i++)
        {
            GameInfoMassege info = GameInfoPool.Get();
            info.GetComponent<GameInfoMassege>().Init();
            _infoList.Add(info);
        }
        return _infoList;
    }

    private GameInfoMassege CreateGameInfoPool()
    {
        GameInfoMassege _info = Instantiate(_gameInfoMassege, _gameInfoBase);
        _info.SetPool(GameInfoPool);
        return _info;
    }

    private void OnGet(GameInfoMassege _info) //  풀 활성화
    {
        _info.gameObject.SetActive(true);
    }
    private void OnReleaseInfo(GameInfoMassege _info) // 풀 비활성화
    {
        _info.gameObject.SetActive(false);
    }
    private void DestroyInfo(GameInfoMassege _info) // 풀 삭제
    {
        Destroy(_info);
    }

    public void GamePlayeInfo(string str) // 아이템 이름과 같은 텍스트를 넣으면 그에 맞게 출력
    {
        GameInfoMassege _info = GameInfoPool.Get();
        _info.GetComponent<Text>().text = str;
        Vector3 pos = PlayerController.Instance.GetComponent<Transform>().localPosition;
        Vector3 screenPos = Camera.main.WorldToScreenPoint(PlayerController.Instance.GetComponent<Transform>().position); 
        _info.GetComponent<RectTransform>().position = screenPos;
    }

    #endregion



}