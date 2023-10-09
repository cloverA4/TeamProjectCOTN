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

    [SerializeField] Image _fade;
    [SerializeField] Canvas _infoCanvas;
    [SerializeField] GameInfoMassege _gameInfoMassege;
    [SerializeField] Transform _gameInfoBase;
    [SerializeField] GameObject Judgement;
    [SerializeField] MissInfoMassege _missInfo;
    [SerializeField] Transform _InfoBase;
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
    [SerializeField] WealthUI _wealthUI;

    UIMenu _status = UIMenu.Null;
    public UIMenu Status { get { return _status; } }

    int _maxInfoCount = 10;
    AudioSource _audio;

    float _effectVolume = 1;
    bool _isOption = false;
    bool _optionActive = true;
    public float EffectVolume { get { return _effectVolume; } }

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
        _audio = GetComponent<AudioSource>();        
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
        gameObject.SetActive(true);
        _fade.gameObject.SetActive(true);
        InfoPool = new ObjectPool<MissInfoMassege>(CreatePool, OnGet, OnReleaseInfo, DestroyInfo, maxSize: 15);
        SpawnInfo();
        _alarmUI.Init();
        _equipmentControll.EquipmentAllDisabel();

        PlayerPrefs.SetInt("TutorialManual", PlayerPrefs.GetInt("TutorialManual", 0));
        if (PlayerPrefs.GetInt("TutorialManual") == 0)
        {
            //최초진입
            ActiveMenuChange(UIMenu.Menual);
            _manual.OnMenual();
            PlayerController.Instance.IsTimeStop = true;
            PlayerPrefs.SetInt("TutorialManual", 1);
            PlayerPrefs.Save();
        }
    }

   public void setHP()
    {
        _hpUI.setHP();
    }

    public void UpdateGold(int count)
    {
        _wealthUI.UpdateGold(count);
    }

    public void UpdataDiamond(int count)
    {
        _wealthUI.UpdataDiamond(count);
    }

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
    public void IconMove(Item _item)  // 아이템 아이콘 애니메이션 함수 따로 어떤 무기인지 구분할 필요 없이 아이템만 넣으면됌
    {
        _equipmentControll.ItemIconMove(_item);
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

    public void Alarm(string main, string str1, string str2, UnityAction action1, UnityAction action2 = null)
    {
        ActiveMenuChange(UIMenu.AlarmUI);
        _alarmUI.StartAlarmUI(main, str1, str2, action1, action2);
        _isOption = false;
        _optionActive = false;
    }
    public void ReturnOption()
    {
        if (_optionUI.transform.Find("Image").gameObject.activeSelf)
        {
            _optionActive = true;
            _isOption = true;
            _alarmUI.EndAlarmUI();
            UIManeger.Instance.ActiveMenuChange(UIMenu.Option);
        }
    }

    public void Option(string main, string str1, string str2, string str3, string str4, string str5, UnityAction action1, UnityAction action2, UnityAction action3, UnityAction action4, UnityAction action5)
    {
        if (_optionActive)
        {
            if (_isOption) AllCloseUI();
            else
            {
                _isOption = true;
                _optionUI.StartOptionUI(main, str1, str2, str3, str4, str5, action1, action2, action3, action4, action5);
                UIManeger.Instance.ActiveMenuChange(UIMenu.Option);
            }
        }
    }

    public void StartSoundOption()
    {
        _optionUI.SoundOption();
        _isOption = false;
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
        _isOption = false;
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
    public void GoLobby()
    {
        _goLobby.GoLobby();
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

        _status = type;

        switch (type)
        {
            case UIMenu.AlarmUI:
                _alarmUI.IsActive = true;
                _optionActive = false;
                break;
            case UIMenu.GoLobby:
                _goLobby.IsDieMessage = true;
                _optionActive = false;
                break;
            case UIMenu.Option:
                _optionUI.IsActive = true;
                break;
            case UIMenu.Menual:
                _manual.IsActive = true;
                break;
        }
    }
    public void AllCloseUI()
    {
        _goLobby.endGoLobbyUI();
        _optionUI.EndOptionUI();
        _alarmUI.EndAlarmUI();
        _isOption = false;
        _optionActive = true;

        Time.timeScale = 1;
        GameManager.Instance.SoundPerse(true);
        PlayerController.Instance.IsTimeStop = false;
        ActiveMenuChange(UIMenu.Null);
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