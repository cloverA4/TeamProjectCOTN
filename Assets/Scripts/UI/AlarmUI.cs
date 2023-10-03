using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SocialPlatforms;
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
    UnityAction _action1;
    UnityAction _action2;

    private void Update()
    {
        if (gameObject.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.UpArrow))
            {
                UIManeger.Instance.PlayEffectSound(SoundEffect.UIChange);
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

        if (Input.GetKeyDown(KeyCode.Return))
        {
            UIManeger.Instance.PlayEffectSound(SoundEffect.UISelect);
            switch (_alarmIndex)
            {
                case 0:
                    _action1();
                    gameObject.SetActive(false);
                    Time.timeScale = 1;
                    PlayerController.Instance.IsTimeStop = false;
                    GameManager.Instance.SoundPerse(true);
                    break;
                case 1:
                    if (_action2 != null) _action2();
                    gameObject.SetActive(false);
                    Time.timeScale = 1;
                    PlayerController.Instance.IsTimeStop = false;
                    GameManager.Instance.SoundPerse(true);
                    break;
            }
        }
    }

    public void Init()
    {
        gameObject.SetActive(false);
        _alarmIndex = 0;
    }


    public void StartAlarmUI(string mainText, string toggle1Text, string toggle2Text, UnityAction action1, UnityAction action2 = null)
    {
        Time.timeScale = 0;
        GameManager.Instance.SoundPerse(false);
        PlayerController.Instance.IsTimeStop = true;
        _action1 = action1;
        _action2 = action2;
        _alarmIndex = 0;
        gameObject.SetActive(true);
        _toggle1.isOn = true;
        _alarmText1.gameObject.SetActive(false);
        _alarmText2.gameObject.SetActive(true);

        _mainText.text = mainText;

        _alarmToggle1.text = "-" + toggle1Text + "-";
        _alarmText1.text = toggle1Text;

        _alarmToggle2.text = "-" + toggle2Text + "-";
        _alarmText2.text = toggle2Text;
    }

    public void EndAlarmUI()
    {
        GameManager.Instance.PlayerHpReset();
        gameObject.SetActive(false);
        _alarmIndex = 0;
    }

    public void AlarmToggle1(bool _bool)
    {
        _alarmIndex = 0;
        _alarmText1.gameObject.SetActive(!_bool);
    }
    public void AlarmToggle2(bool _bool)
    {
        _alarmIndex = 1;
        _alarmText2.gameObject.SetActive(!_bool);
    }

}
