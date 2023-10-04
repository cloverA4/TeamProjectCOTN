using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Pool;
using UnityEngine.UI;

public class UIManeger : MonoBehaviour
{
    private static UIManeger instance;

    public event EventHandler EventVolumeChange;

    AudioSource _audio;
    float _effectVolume = 1;
    public float EffectVolume { get { return _effectVolume; } }


    [SerializeField] Text _goldCount;
    [SerializeField] Text _diamondCount;

    int _goLobbyIndex = 0;
    [SerializeField] Image _fade;

   

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
    [SerializeField] PlayerHitUI _playerHit;
    [SerializeField] OptionUI _optionUI;
    [SerializeField] ControllManualPageManager _manual;
    [SerializeField] GoLobbyUI _goLobby;
    [SerializeField] HPUI _hpUI;
    [SerializeField] CenterMessage _centerMessage;

    UIMenu _activeMenu;

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
        _audio = GetComponent<AudioSource>();

        PlayerPrefs.SetInt("TutorialManual", PlayerPrefs.GetInt("TutorialManual", 0));
        if(PlayerPrefs.GetInt("TutorialManual") == 0)
        {
            //최초진입
            ActiveMenuChange(UIMenu.Menual);
            _manual.OnMenual();
            PlayerPrefs.SetInt("TutorialManual", 1);
            PlayerPrefs.Save();
        }
    }

    public static UIManeger Instance
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
    }
    #region HP
   public void setHP()
    {
        _hpUI.setHP();
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
    public void StartFadin()
    {
        StartCoroutine(FadeIn());
    }

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
        MissInfoMassege info = InfoPool.Get();
        info.GetComponent<Text>().text = "빗나감!";
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

    public void Alarm(string main, string str1, string str2, UnityAction action1, UnityAction action2 = null)
    {
        ActiveMenuChange(UIMenu.AlarmUI);
        _alarmUI.StartAlarmUI(main, str1, str2, action1, action2);
    }

    public void Option(string main, string str1, string str2,string str3,string str4, UnityAction action1, UnityAction action2, UnityAction action3,UnityAction action4)
    {
        ActiveMenuChange(UIMenu.Option);
        _optionUI.StartOptionUI(main, str1, str2, str3, str4, action1, action2, action3, action4);
    }
    public void EndOption()
    {
        _optionUI.EndOptionUI();
    }
    public void StartSoundOption()
    {
        string main = "오디오 옵션";
        _optionUI.SoundOption(main);
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
    public void DisableCoinMultiple() // 코인 배수 꺼주는 함수
    {
        _coinMultipleUI.DisableCoinMUltiple();
    }
    public void ActiveCoinMultiple()
    {
        _coinMultipleUI.ActiveCoinMultiple();
    }
    public void PlayerHitUI() // 플레이어가 맞았을경우 UI화면 빨개지는 함수
    {
        _playerHit.HitUI();
    }

    public void OnControllManual()  // 조작법 켜기
    {
        ActiveMenuChange(UIMenu.Menual);
        _manual.IsOption = true;
        _manual.OnMenual();
    }

    public void OffControllManual()  // 조작법 끄기
    {
        _manual.OffMenual();
    }

    public void StartGoLobby()
    {
        ActiveMenuChange(UIMenu.GoLobby);
        _goLobby.StartGoLobbyUI();
    }

    public void VolumeChange(float Volume)
    {
        _effectVolume = Volume;
        _audio.volume = Volume;
        EventVolumeChange?.Invoke(this, EventArgs.Empty);
    }

    public void PlayEffectSound(SoundEffect sound)
    {
        _audio.clip = Data.Instance.SoundEffect[(int)sound];
        _audio.Play();
    }

    public void CenterMessage(string message)
    {
        _centerMessage.SetCenterMessage(message);
    }

    public void ActiveMenuChange(UIMenu type)
    {
        _goLobby.IsActive = false;
        _alarmUI.IsActive = false;
        _optionUI.IsActive = false;
        _manual.IsActive = false;
        _goLobby.IsDieMessage = false;

        switch (type)
        {
            case UIMenu.AlarmUI:
                _alarmUI.IsActive = true;
                break;
            case UIMenu.GoLobby:
                _goLobby.IsDieMessage = true;
                break;
            case UIMenu.Option:
                _optionUI.IsActive = true;
                break;
            case UIMenu.Menual:
                _manual.IsActive = true;
                break;
        }
    }
}

public enum UIMenu
{
    Null,
    AlarmUI,
    GoLobby,
    Option,
    Menual,
}