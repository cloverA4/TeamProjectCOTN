using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManeger : MonoBehaviour
{
    [SerializeField] GameObject _heart;
    [SerializeField] Transform _HeartBase;

    [SerializeField] Text _goldCount;
    [SerializeField] Text _diamondCount;
    public int _gold;
    public int _diamond;
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

    [SerializeField] GameObject _failMessage;
    [SerializeField] GameObject _failMessageBase;
    [SerializeField] int _speed;


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
    }

    public void UIInit()
    {
        gameObject.SetActive(true);
        _fade.gameObject.SetActive(true);
        setHP();
        _alarmUI.gameObject.SetActive(false);
        _goLobbyUI.SetActive(false);
    }
    #region HP
    List<GameObject> hearts = new List<GameObject>();
    public void setHP()
    {
        ResetHP();

        for (int i = 0; i < PlayerController.Instance.MaxHP; i++) // 맥스 HP값
        {
            var temp =Instantiate(_heart, _HeartBase);
            hearts.Add(temp);
        }
        for(int i = 0; i< (int)PlayerController.Instance.NowHP; i++)  // 현재 HP값
        {
            var temp = hearts[i].gameObject.transform.GetChild(0);
            temp.gameObject.SetActive(true);
            if(i > PlayerController.Instance.MaxHP)
            {
                return;
            }
        }
        if((PlayerController.Instance.NowHP - (int)PlayerController.Instance.NowHP) > 0)  // 반칸 하트 구현
        {
            var temp = hearts[(int)PlayerController.Instance.NowHP].gameObject.transform.GetChild(1);
            var temp1 = hearts[(int)PlayerController.Instance.NowHP].gameObject.transform.GetChild(0);

            temp1.gameObject.SetActive(false);
            temp.gameObject.SetActive(true);
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

}