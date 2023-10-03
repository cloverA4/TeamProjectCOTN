using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.EventSystems.EventTrigger;

public class GoLobbyUI : MonoBehaviour
{
    [SerializeField] GameObject _goLobbyUI;
    [SerializeField] GameObject _lobbyToggle;
    [SerializeField] GameObject _retryToggle;
    [SerializeField] GameObject _replayToggle;

    [SerializeField] GameObject _DieMasseage;
    int _index = 0;

    private void Start()
    {
        _DieMasseage.SetActive(false);
        _goLobbyUI.SetActive(false);
    }
    private void Update()
    {
        if (_goLobbyUI.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                if (_index == 0)
                {
                    _index = 1;
                }
                else if (_index == 1)
                {
                    _index = 2;
                }
                else if (_index == 2)
                {
                    _index = 0;
                }
                SelectToggle();
            }
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                if (_index == 0)
                {
                    _index = 2;
                }
                else if (_index == 1)
                {
                    _index = 0;
                }
                else if (_index == 2)
                {
                    _index = 1;
                }
                SelectToggle();
            }
            if (_lobbyToggle.GetComponent<Toggle>().isOn)
            {
                _index = 0;
            }
            else if (_retryToggle.GetComponent<Toggle>().isOn)
            {
                _index = 1;
            }
            else if (_replayToggle.GetComponent<Toggle>().isOn)
            {
                _index = 2;
            }

        }
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (_goLobbyUI.activeSelf)
            {
                switch (_index)
                {
                    case 0:
                        GameManager.Instance.NowStage = Stage.Lobby;
                        GameManager.Instance.NowFloor = floor.f1;
                        GameManager.Instance.Gold = 0;
                        UIManeger.Instance.StartFadin();
                        endGoLobbyUI();
                        break;
                    case 1:
                        string main = "사용하지 않은 다이아몬드는\r\n재시작시 모두 사라집니다.\r\n로비로 되돌아가 사용하겠습니까? ";
                        string Toggle1 = "로비에서 다이아몬드 사용";
                        string toggle2 = "빠른재시작";
                        UIManeger.Instance.Alarm(main, Toggle1, toggle2, GoLobby, Retry);
                        endGoLobbyUI();
                        break;
                    case 2:  // replay 기능 추후 구현
                        break;
                }
            }
        }
        if (_DieMasseage.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.DownArrow) && Input.GetKeyDown(KeyCode.UpArrow))
            {
                _DieMasseage.SetActive(false);
                _index = 0;
                _goLobbyUI.SetActive(true);
                SelectToggle();
            }
        }
    }

    public void endGoLobbyUI()
    {
        GameManager.Instance.PlayerHpReset();
        _goLobbyUI.SetActive(false);
    }

    public void StartGoLobbyUI()
    {
        _DieMasseage.gameObject.SetActive(true);
    }

    void SelectToggle()
    {
        if (_goLobbyUI.activeSelf)
        {
            switch (_index)
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


    public void GoLobby()
    {
        GameManager.Instance.NowStage = Stage.Lobby;
        GameManager.Instance.NowFloor = floor.f1;
        PlayerController.Instance.BaseItemEquip();
        UIManeger.Instance.StartFadin();
    }
    public void Retry()
    {
        GameManager.Instance.NowFloor = floor.f1;
        GameManager.Instance.Gold = 0;
        PlayerController.Instance.BaseItemEquip();
        UIManeger.Instance.StartFadin();
    }
}
