using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class UIManeger : MonoBehaviour
{

    [SerializeField] GameObject _heart;
    [SerializeField] Transform _HeartBase;

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
    [SerializeField] GameObject _failMessageBase;
    [SerializeField] int _speed;


    private void Update()
    {
        if(_goLobbyUI.activeSelf == true) GoLobbyArrow();
        if (_alarmUI.activeSelf == true) AlarmArrow();
        //if (Input.GetMouseButtonDown(0))
        //{
        //    StartCoroutine(FailMessge());
        //}
        
    }

    private void Start()
    {
        _fade.gameObject.SetActive(true);
        setHP(5, 5);
    }
    #region HP

    void setHP(float nowHP , int maxHP)
    {
        for (int i = 0; i < maxHP; i++)
        {
            Instantiate(_heart, _HeartBase);
            for (int j = 0; j < nowHP; j++)
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
                    GameManager.Instance.NowStage = Stage.Lobby;
                    GameManager.Instance.NowFloor = floor.f1;
                    StartCoroutine(FadeIn());
                    endGoLobbyUI();
                }
                break;
            case 2:
                _retryToggle.GetComponent<Toggle>().isOn = true;
                if (Input.GetKeyDown(KeyCode.Return))
                {
                    GameManager.Instance.NowFloor = floor.f1;
                    StartCoroutine(FadeIn());
                    endGoLobbyUI();
                }
                break;
            case 3:  // replay 기능 추후 구현
                _replayToggle.GetComponent<Toggle>().isOn = true;
                break;
            default:
                 break;
        }
    }
    public void GoRetry()
    {
        // 현재 스테이지 값은 그대로
        // 플로어 값만 1로 변환
        // 현재가 몇 스테이지인지 검사 후 스테이지 스타트 포인트를 받아옴
        // 플레이어 위치 정보 업데이트
        // 페이드 아웃
        StageStartPosition tempPos = new StageStartPosition();

        GameManager.Instance.NowFloor = floor.f1;
        if(GameManager.Instance.NowStage == Stage.Stage1)
        {
            PlayerController.Instance.transfromUpdate(tempPos.Stage1F1);
        }
        else if (GameManager.Instance.NowStage == Stage.Stage2)
        {
            // 나중에 스테이지2가 나왔을경우 추가
        }
        StartCoroutine(FadeOut());
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

    public IEnumerator FailMessge()
    { 
      // 실패 메세지 뜨면 3초동안 올라가고
      // 점점 희미해지면서 사라짐
        GameObject temp = Instantiate(_failMessage, _failMessageBase.transform);
        Color tempColor = temp.GetComponent<Image>().color;
        Vector3 tempPos = temp.GetComponent<Transform>().position;
        Debug.Log("aaa");
        while(true)
        {
            tempColor.a -= Time.deltaTime;
            temp.GetComponent<Image>().color = tempColor;

            tempPos.y += _speed;
            temp.GetComponent<Transform>().position = tempPos;
            yield return null;

            if(tempColor.a <= 0)
            {
                break;
            }
        }
        Destroy(temp);
    }
}