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


    public void StartAlarmUI(string mainText, string _toggle1, string _toggle2, UnityAction _action1, UnityAction _action2 = null)
    {
        _alarmIndex = 0;
        gameObject.SetActive(true);
        _alarmToggle1.gameObject.SetActive(true);
        _alarmToggle2.gameObject.SetActive(false);
        _alarmText1.gameObject.SetActive(false);
        _alarmText2.gameObject.SetActive(true);

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
                    if(_action2 != null) _action2();
                    gameObject.SetActive(false);
                    break;
            }
        }
    }

    

    public void EndAlarmUI()
    {
        GameManager.Instance.PlayerHpReset();
        gameObject.SetActive(false);
        _alarmIndex = 0;
    }

    public void AlarmToggle1(bool _bool)
    {
        _alarmText1.gameObject.SetActive(!_bool);
    }
    public void AlarmToggle2(bool _bool)
    {
        _alarmText2.gameObject.SetActive(!_bool);
    }

}
