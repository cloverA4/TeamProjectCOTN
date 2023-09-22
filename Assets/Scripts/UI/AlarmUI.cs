using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class AlarmUI : MonoBehaviour
{
    [SerializeField] Toggle _toggle1;
    [SerializeField] Toggle _toggle2;

    [SerializeField] Text _alarmToggle1;
    [SerializeField] Text _alarmText1;
    [SerializeField] Text _alarmToggle2;
    [SerializeField] Text _alarmText2;

    [SerializeField] Text _mainText;

    int _alarmIndex = 0;

    private void Update()
    {
        if (gameObject.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.UpArrow))
            {
                if (_alarmIndex == 0) _alarmIndex = 1;
                else _alarmIndex = 0;

                switch (_alarmIndex)
                {
                    case 0:
                        _toggle1.isOn = true;
                        break;
                    case 1:
                        _toggle2.isOn = true;
                        break;
                }
            }

        }
    }

    public void Init()
    {
        gameObject.SetActive(false);
        _alarmIndex = 0;
    }


    void ActionEvent(string mainText, string _toggle1, string _toggle2, UnityAction _action1, UnityAction _action2)
    {
        _mainText.text = mainText;

        _alarmToggle1.GetComponent<Text>().text = "-" + _toggle1 + "-";
        _alarmText1.GetComponent<Text>().text = _toggle1;

        _alarmToggle2.GetComponent<Text>().text = "-" + _toggle2 + "-";
        _alarmText2.GetComponent<Text>().text = _toggle2;

        if (Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            switch (_alarmIndex)
            {
                case 0:
                    _action1();
                    break;
                case 1:
                    _action2();
                    break;
            }
        }
    }

    public void GoLobby()
    {
        GameManager.Instance.NowStage = Stage.Lobby;
        GameManager.Instance.NowFloor = floor.f1;
        EndAlarmUI();
        //StartCoroutine(FadeIn());   ui매니저 싱글턴으로 만들고 호츌
    }
    public void Retry()
    {
        GameManager.Instance.NowFloor = floor.f1;
        GameManager.Instance.Gold = 0;
        PlayerController.Instance.BaseItemEquip();
        EndAlarmUI();
        //StartCoroutine(FadeIn());
    }

    public void StartAlarmUI()
    {
        _alarmIndex = 0;
        gameObject.SetActive(true);
        _alarmText2.gameObject.SetActive(true);
    }
    public void EndAlarmUI()
    {
        GameManager.Instance.PlayerHpReset();
        gameObject.SetActive(false);
        _alarmIndex = 0;
    }

    public void AlarmToggle1(bool _bool)
    {
        if (_bool) _alarmText1.gameObject.SetActive(false);
        else _alarmText1.gameObject.SetActive(true);
    }
    public void AlarmToggle2(bool _bool)
    {
        if (_bool) _alarmText2.gameObject.SetActive(false);
        else _alarmText2.gameObject.SetActive(true);
    }

}
