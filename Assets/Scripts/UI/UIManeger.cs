using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class UIManeger : MonoBehaviour
{
    [SerializeField] int _maxHP;

    [SerializeField] int _hpCount;

    [SerializeField] GameObject _heart;

    [SerializeField] Transform _HPPanel;

    [SerializeField] Text _goldCount;
    [SerializeField] Text _diamondCount;
    public int _gold;
    public int _diamond;

    [SerializeField] Image _shovelImage;
    [SerializeField] Image _armorImage;
    [SerializeField] Image _weaponImage;
    [SerializeField] GameObject _emptyPotion;

    [SerializeField] GameObject NotePrefab;
    [SerializeField] GameObject NotesPool;

    [SerializeField] Image _fade;

    [SerializeField] GameObject _goLobbyUI;
    [SerializeField] GameObject _lobbyToggle;
    [SerializeField] GameObject _retryToggle;
    [SerializeField] GameObject _replayToggle;

    [SerializeField] GameObject _alarmUI;
    [SerializeField] GameObject _useDiamondToggle;
    [SerializeField] GameObject _retryToggle2;

    [SerializeField] GameObject _failMessage;

    private void Update()
    {
        if(_goLobbyUI.activeSelf == true) GoLobbyArrow();
        if (_alarmUI.activeSelf == true) AlarmArrow();
    }




    private void Start()
    {
        setMaxHP();
    }
    #region HP
    void setMaxHP()
    {
        for (int i = 0; i < _maxHP; i++)
        {
            GameObject obj = Instantiate(_heart, _HPPanel);
            obj.gameObject.name = "emptyHP" + i;
        }
    }

    void setHP(float hp)
    {

        for (int i = 0; i < _maxHP; i++)
        {
            string str = "HP" + i;

            if (i < hp)
            {

            }
        }
    }

    #endregion

    #region Wealth
    void UpdateGold(int _count)
    {
        _gold += _count;

        _goldCount.text = "X " + _gold;
    }

    void UpdataDiamond(int _count)
    {
        _diamond += _count;

        _diamondCount.text = "X " + _diamond;
    }

    void ResetGold()
    {
        _gold = 0;
        _goldCount.text = "X " + _gold;
    }
    void RestDiamond()
    {
        _diamond = 0;
        _diamondCount.text = "X " + _diamond;
    }

    #endregion

    #region Equipment

    public void Equipment(Item _item)
    {
        Sprite _changeImage;
        switch (_item._itemType)
        {
            case ItemType.Weapon:
                _changeImage = Resources.Load("UI/Item" + _item.ItemID) as Sprite;
                _weaponImage.sprite = _changeImage;
                break;
            case ItemType.Armor:
                _changeImage = Resources.Load("UI/Item" + _item.ItemID) as Sprite;
                _armorImage.sprite = _changeImage;
                break;
            case ItemType.Shovel:
                _changeImage = Resources.Load("UI/Item" + _item.ItemID) as Sprite;
                _shovelImage.sprite = _changeImage;
                break;
            case ItemType.Potion:
                break;
            default:
                break;
        }

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
        _goLobbyUI.SetActive(true);
    }    

    int goLobbycount = 1;
    public void GoLobbyArrow()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            goLobbycount++;
            if (goLobbycount > 3)
            {
                goLobbycount = 1;
            }
            if (goLobbycount < 1)
            {
                goLobbycount = 3;
            }

        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            goLobbycount--;
            if (goLobbycount > 3)
            {
                goLobbycount = 1;
            }
            if (goLobbycount < 1)
            {
                goLobbycount = 3;
            }
        }

        switch (goLobbycount)
        {
            case 1:
                _lobbyToggle.GetComponent<Toggle>().isOn = true;
                if (Input.GetKeyDown(KeyCode.Return))
                {
                    StartCoroutine(FadeIn());
                    Invoke("GoLobby", 1f);
                }
                break;
            case 2:
                _retryToggle.GetComponent<Toggle>().isOn = true;
                if (Input.GetKeyDown(KeyCode.Return))
                {
                    StartCoroutine(FadeIn());
                    Invoke("GoRetry", 1f);
                }
                break;
            case 3:
                _replayToggle.GetComponent<Toggle>().isOn = true;
                break;
            default:
                 break;
        }
    }
    public void GoLobby()
    {
        GameManager.Instance.NowStage = Stage.Lobby;
        GameManager.Instance.NowFloor = floor.f1;
        StageStartPosition tempPos = new StageStartPosition();
        PlayerController.Instance.transfromUpdate(tempPos.LobbyPosition);
        StartCoroutine(FadeOut());
    }
    public void GoRetry(Stage _nowStage , floor _nowFloor)
    {
        // 현재 스테이지와 
    }
    public void SelectLobby()
    {
        _lobbyToggle.GetComponent<LobbyToggle>().OnCheck();
        _retryToggle.GetComponent<RetryToggle>().OffCheck();
        _replayToggle.GetComponent<RePlayToggle>().OffCheck();   
    }

    public void SelectRetry()
    {
        _lobbyToggle.GetComponent<LobbyToggle>().OffCheck();
        _retryToggle.GetComponent<RetryToggle>().OnCheck();
        _replayToggle.GetComponent<RePlayToggle>().OffCheck();

        if (_useDiamondToggle.GetComponent<Toggle>().isOn == true)
        {
            _useDiamondToggle.GetComponent<UseDiamondToggle>().OffCheck();
        }
    }

    public void SelectReplay()
    {
        _lobbyToggle.GetComponent<LobbyToggle>().OffCheck();
        _retryToggle.GetComponent<RetryToggle>().OffCheck();
        _replayToggle.GetComponent<RePlayToggle>().OnCheck();
    }




    #endregion

    #region Alarm UI
    public void StartAlarmUI()
    {
        _alarmUI.SetActive(true);
    }
    public void EndAlarmUI()
    {
        _alarmUI.SetActive(false);
    }

    public void SelectUseDiamond()
    {
        _useDiamondToggle.GetComponent<UseDiamondToggle>().OnCheck();
        _retryToggle2.GetComponent<RetryToggle2>().OffCheck();
    }
    public void SelectRetry2()
    {
        _useDiamondToggle.GetComponent<UseDiamondToggle>().OffCheck();
        _retryToggle2.GetComponent<RetryToggle2>().OnCheck();
    }

    int alarmCount = 1;
    public void AlarmArrow()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            alarmCount--;
            if (alarmCount < 1)
            {
                alarmCount = 2;
            }
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            alarmCount++;
            if (alarmCount > 2)
            {
                alarmCount = 1;
            }
        }

        switch (alarmCount)
        {
            case 1:
                _retryToggle2.GetComponent<Toggle>().isOn = true;
                break;
            case 2:
                _useDiamondToggle.GetComponent<Toggle>().isOn = true;
                break;
            default:
                break;
        }
    }

    #endregion

    public void FailMassge()
    { // 실패 메세지 
        Instantiate(_failMessage, transform);
    }
}