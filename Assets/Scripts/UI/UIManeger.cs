using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
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
        //MouseSelect();
    }

    private void Start()
    {
        _fade.gameObject.SetActive(true);
        setHP();
        ToggleTextAllEnable();
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
        _goLobbyUI.SetActive(true);
        ToggleTextAllEnable();
    }    

    int goLobbyIndex = 1;
    public void GoLobbyArrow()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            goLobbyIndex++;
            if (goLobbyIndex > 3)
            {
                goLobbyIndex = 1;
            }
            if (goLobbyIndex < 1)
            {
                goLobbyIndex = 3;
            }

        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            goLobbyIndex--;
            if (goLobbyIndex > 3)
            {
                goLobbyIndex = 1;
            }
            if (goLobbyIndex < 1)
            {
                goLobbyIndex = 3;
            }
        }
        SelectToggle();
    }
    public Camera mainCamera;
    void MouseSelect()
    {
    }
    void SelectToggle()
    {
        switch (goLobbyIndex)
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

    
    void ToggleTextAllEnable()
    {
        _lobbyToggle.transform.GetChild(1).gameObject.SetActive(true);
        _retryToggle.transform.GetChild(1).gameObject.SetActive(true);
        _replayToggle.transform.GetChild(1).gameObject.SetActive(true);
    }
    public void GoLobbySelect(bool _bool)
    {
        if (_bool == true) _lobbyToggle.transform.GetChild(1).gameObject.SetActive(false);
        else if (_bool == false) _lobbyToggle.transform.GetChild(1).gameObject.SetActive(true);
    }
    public void RetrySelect(bool _bool)
    {
        if (_bool == true) _retryToggle.transform.GetChild(1).gameObject.SetActive(false);
        else if (_bool == false) _retryToggle.transform.GetChild(1).gameObject.SetActive(true);
    }
    public void ReplaySelect(bool _bool)
    {
        if (_bool == true) _replayToggle.transform.GetChild(1).gameObject.SetActive(false);
        else if (_bool == false) _replayToggle.transform.GetChild(1).gameObject.SetActive(true);
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