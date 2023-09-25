using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Pool;
using UnityEngine.UI;

public class UIManeger : MonoBehaviour
{
    [SerializeField]
    GameObject[] Hearts;

    [SerializeField] Text _goldCount;
    [SerializeField] Text _diamondCount;

    int _goLobbyIndex = 0;
    [SerializeField] Image _fade;

    [SerializeField] GameObject _goLobbyUI;
    [SerializeField] GameObject _lobbyToggle;
    [SerializeField] GameObject _retryToggle;
    [SerializeField] GameObject _replayToggle;

    [SerializeField] Canvas _infoCanvas;
    [SerializeField] GameInfoMassege _gameInfoMassege;
    [SerializeField] Transform _gameInfoBase;

    [SerializeField] GameObject Judgement;
    [SerializeField] MissInfoMassege _missInfo;
    [SerializeField] Transform _InfoBase;
    int _maxInfoCount = 10;

    [SerializeField] AlarmUI _alarmUI;
    [SerializeField] EquipmentControll _equipmentControll;
    [SerializeField] StageClearUI _stageClearUI;
    [SerializeField] CoinMultipleUI _coinMultipleUI;

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
        if (Input.GetKeyDown(KeyCode.Return))
        {
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
                        string main = "사용하지 않은 다이아몬드는\r\n재시작시 모두 사라집니다.\r\n로비로 되돌아가 사용하겠습니까? ";
                        string Toggle1 = "로비에서 다이아몬드 사용";
                        string toggle2 = "빠른재시작";
                        _alarmUI.StartAlarmUI(main, Toggle1, toggle2, GoLobby, Retry);
                        endGoLobbyUI();
                        break;
                    case 2:  // replay 기능 추후 구현
                        break;
                }
            }
        }
    }

    public void GoLobby()
    {
        GameManager.Instance.NowStage = Stage.Lobby;
        GameManager.Instance.NowFloor = floor.f1;
        //StartCoroutine(FadeIn());   ui매니저 싱글턴으로 만들고 호츌
    }
    public void Retry()
    {
        GameManager.Instance.NowFloor = floor.f1;
        GameManager.Instance.Gold = 0;
        PlayerController.Instance.BaseItemEquip();
        //StartCoroutine(FadeIn());
    }

    private void Start()
    {
        UIInit();
        InfoPool = new ObjectPool<MissInfoMassege>(CreatePool, OnGet, OnReleaseInfo, DestroyInfo, maxSize: 15);
        GameInfoPool = new ObjectPool<GameInfoMassege>(CreateGameInfoPool, OnGet, OnReleaseInfo, DestroyInfo, maxSize: 15);
        SpawnInfo();
        SpawnGameInfo();
        _alarmUI.Init();
        _equipmentControll.EquipmentAllDisabel();
    }

    public void UIInit()
    {
        gameObject.SetActive(true);
        _fade.gameObject.SetActive(true);
        _goLobbyUI.SetActive(false);
    }
    #region HP
    public void setHP()
    {
        PlayerController pc = PlayerController.Instance;
        ResetHP();

        int damge = pc.MaxHP - pc.NowHP;

        for(int i = 0; i< pc.MaxHP/2; i++)
        {
            Hearts[i].gameObject.SetActive(true);
            Hearts[i].transform.Find("FullHeart").gameObject.SetActive(true);
            Hearts[i].transform.Find("HalfHeart").gameObject.SetActive(false);
        }
        for (int i = 0;i< damge/2; i++)
        {
            Hearts[i].transform.Find("FullHeart").gameObject.SetActive(false);
            Hearts[i].transform.Find("HalfHeart").gameObject.SetActive(false);

        }
        if (damge %2 == 1)
        {
            Hearts[damge/2].transform.Find("HalfHeart").gameObject.SetActive(true);
            Hearts[damge/2].transform.Find("FullHeart").gameObject.SetActive(false);
        }
    }

    void ResetHP()
    {
        for (int i = 0; i < Hearts.Length; i++) 
        {
            Hearts[i].gameObject.SetActive(false);
        } 
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
        GameManager.Instance.PlayerHpReset();
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

    #region Equipment

    public void UpdataShovel()
    {
        _equipmentControll.UpdataShovel();
    }

    public void UpdataWeapon()
    {
        _equipmentControll.UpdataWeapon();
    }

    public void UpdateArmor()
    {
        _equipmentControll.UpdateArmor();
    }

    public void UpdatePotion()
    {
        _equipmentControll.UpdatePotion();
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
        if(_goLobbyUI.activeSelf == false)
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

    public void Alarm(string str1, string str2, string str3, UnityAction action1, UnityAction action2 = null)
    {
        _alarmUI.StartAlarmUI(str1, str2, str3, action1, action2);
    }
    public void IconMove(Item _item)  // 아이템 아이콘 애니메이션 함수 따로 어떤 무기인지 구분할 필요 없이 아이템만 넣으면됌
    {
        _equipmentControll.ItemIconMove(_item);
    }
    public void StageClear() // 마지막 스테이지의 상자를 열때 호출
    {
        _stageClearUI.OnClearEffect();
    }
    public void CoinMultipleUI(float multiple , int multipleIndex) // 코인 배수 텍스트로 출력
    {
        _coinMultipleUI.OnCoinMultiple(multiple, multipleIndex);
    }
}