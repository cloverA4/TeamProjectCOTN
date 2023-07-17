using System.Collections;
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
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            StartGoLobbyUI();
            //SelectToggle();
        }
    }




    private void Start()
    {
        endGoLobbyUI();
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
        _diamond= 0;
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
                _shovelImage.sprite= _changeImage;
                break;
            case ItemType.Potion:
                break;
            default:
                break;
        }
            
    }
   
    public void PotionCount(bool _potion)
    {
        if(_potion == true ) _emptyPotion.SetActive(false);
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
        while(true)
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

    enum LobbyAndRetry
    {
        Lobby,
        Retry,
    }
    
    LobbyAndRetry lar = new LobbyAndRetry();

    void GoLobby(LobbyAndRetry lar)
    {
        if (lar == LobbyAndRetry.Lobby)
        {

        }
        else if (lar == LobbyAndRetry.Retry)
        {

        }
        else return;
    }

    public void SelectToggle()
    {
        if(_lobbyToggle.GetComponent<Toggle>().isOn == true)
        {
            _lobbyToggle.transform.Find("LobbyCheck").gameObject.SetActive(true);
            _lobbyToggle.transform.Find("LobbyText").gameObject.SetActive(false);
        }
        else
        {
            _retryToggle.transform.Find("RetryCheck").gameObject.SetActive(true);
            _retryToggle.transform.Find("RetryText").gameObject.SetActive(false);
        }
    }



    #endregion
}