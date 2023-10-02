using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class OptionUI : MonoBehaviour
{
    [SerializeField] GameObject _toggleGroup;
    [SerializeField] GameObject _soundOption;

    [SerializeField] Toggle _toggle1;
    [SerializeField] Toggle _toggle2;
    [SerializeField] Toggle _toggle3;

    [SerializeField] Text _check1;
    [SerializeField] Text _check2;
    [SerializeField] Text _check3;

    [SerializeField] Text _text1;
    [SerializeField] Text _text2;
    [SerializeField] Text _text3;

    [SerializeField] Text _main;

    int _index = 0;
    UnityAction _action1;
    UnityAction _action2;
    UnityAction _action3;

    [SerializeField] Toggle _soundToggle1;
    [SerializeField] Toggle _soundToggle2;
    [SerializeField] Toggle _soundToggle3;

    [SerializeField] Text _soundText1;
    [SerializeField] Text _soundText2;
    [SerializeField] Text _soundText3;
    [SerializeField] Text _soundCheck1;
    [SerializeField] Text _soundCheck2;

    [SerializeField] Button _volumAddBtn1;
    [SerializeField] Button _volumAddBtn2;
    [SerializeField] Button _volumSubBtn1;
    [SerializeField] Button _volumSubBtn2;

    [SerializeField] Slider _soundBar1;
    [SerializeField] Slider _soundBar2;


    [SerializeField] GameObject _gameObject;

    AudioSource _audio;

    // »ç¿îµå Á¶ÀÛÀº ¸¶¿ì½º

    private void Start()
    {
        Init();
    }

    private void Update()
    {
        if (_toggleGroup.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                _index++;
                if (_index > 2) _index = 0;
                CheckToggle();
            }
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                _index--;
                if (_index < 0) _index = 2;
                CheckToggle();
            }
            

            if (Input.GetKeyDown(KeyCode.Return))
            {
                switch (_index)
                {
                    case 0:
                        _action1();
                        _toggleGroup.gameObject.SetActive(false);
                        break;
                    case 1:
                        if (_action2 != null) _action2();
                        _toggleGroup.gameObject.SetActive(false);
                        break;
                    case 2:
                        if (_action3 != null) _action3();
                        _toggleGroup.gameObject.SetActive(false);
                        break;
                }
                if (_gameObject.activeSelf == false)
                {
                    Time.timeScale = 1;
                    PlayerController.Instance.IsTimeStop = false;
                    GameManager.Instance.SoundPerse(true);
                }
            }
        }
        if (_soundOption.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                _index++;
                if (_index > 2) _index = 0;
                CheckSoundToggle();
            }
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                _index--;
                if (_index < 0) _index = 2;
                CheckSoundToggle();
            }
            if(Input.GetKeyDown(KeyCode.LeftArrow))
            {
                if (_soundToggle1.isOn) _soundBar1.value -= 0.05f;
                if (_soundToggle2.isOn) _soundBar2.value -= 0.05f;
            }
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                if (_soundToggle1.isOn) _soundBar1.value += 0.05f;
                if (_soundToggle2.isOn) _soundBar2.value += 0.05f;
            }
            if (Input.GetKeyDown(KeyCode.Return))
            {
                if (_index == 2) OffSoundOption();
            }
        }
       
    }


    public void Init()
    {
        gameObject.SetActive(true);
        _gameObject.SetActive(false);
        _index = 0;
        _soundOption.gameObject.SetActive(false);
        _soundBar1.value = 0.5f;
        _soundBar2.value = 0.5f;
    }

    public void StartOptionUI(string mainText, string toggle1Text, string toggle2Text,string toggle3Text , UnityAction action1, UnityAction action2, UnityAction action3)
    {
        Time.timeScale = 0;
        GameManager.Instance.SoundPerse(false);
        PlayerController.Instance.IsTimeStop = true;
        _gameObject.SetActive(true);
        _action1 = action1;
        _action2 = action2;
        _action3 = action3;

        _index = 0;
        _toggleGroup.gameObject.SetActive(true);
        _soundOption.gameObject.SetActive(false);
        _toggle1.isOn = true;
        _text1.gameObject.SetActive(false);
        _text2.gameObject.SetActive(true);

        _main.text = mainText;

        _check1.text = "-" + toggle1Text + "-";
        _text1.text = toggle1Text;

        _check2.text = "-" + toggle2Text + "-";
        _text2.text = toggle2Text;

        _check3.text = "-" + toggle3Text + "-";
        _text3.text = toggle3Text;
    }

    void CheckSoundToggle()
    {
        switch (_index)
        {
            case 0:
                _soundToggle1.isOn = true;
                break;
            case 1:
                _soundToggle2.isOn = true;
                break;
            case 2:
                _soundToggle3.isOn = true;
                break;
        }
    }
    void CheckToggle()
    {
        switch (_index)
        {
            case 0:
                _toggle1.isOn = true;
                break;
            case 1:
                _toggle2.isOn = true;
                break;
            case 2:
                _toggle3.isOn = true;
                break;
        }
    }

    public void SoundOption(string main)
    {
        _index = 0;
        _main.text = main;
        _soundOption.gameObject.SetActive(true);
    }
    void OffSoundOption()
    {
        _index = 0;
        _soundOption.gameObject.SetActive(false);
        _toggleGroup.gameObject.SetActive(true);
        _toggle1.isOn = true;
        _toggle2.isOn = false;
        _toggle3.isOn = false;
    }

    public void SetMusicSound(float value)
    {
        GameManager.Instance.AudioVolumControll(value);
        _soundText1.text = "À½¾Ç º¼·ý: " + (int)(value * 100) + "%";
        _soundCheck1.text = "À½¾Ç º¼·ý: " + (int)(value * 100) + "%";
    }
    public void SetEffectSound(float value)
    {
        // ÀÌÆåÆ® ¿É¼Ç°ª
        _soundText2.text = "ÀÌÆåÆ® º¼·ý: " + (int)(value * 100) + "%";
        _soundCheck2.text = "ÀÌÆåÆ® º¼·ý: " + (int)(value * 100) + "%";
    }

    public void VolumAddBtn()
    {
        if (_soundToggle1.isOn)
        {
            _soundBar1.value += 0.05f;
        }
        else if (_soundToggle2.isOn)
        {
            _soundBar2.value += 0.05f;
        }
    }
    public void VolumSubBtn()
    {
        if (_soundToggle1.isOn)
        {
            _soundBar1.value -= 0.05f;
        }
        else if (_soundToggle2.isOn)
        {
            _soundBar2.value -= 0.05f;
        }
    }

    public void EndOptionUI()
    {
        _gameObject.SetActive(false);
    }


    public void OptionToggle1(bool _bool)
    {
        _index = 0;
        if (_toggleGroup.activeSelf)
        {
            _text1.gameObject.SetActive(!_bool);
            return;
        }
        if (_soundOption.activeSelf)
        {
            _soundText1.gameObject.SetActive(!_bool);
            _volumAddBtn1.gameObject.SetActive(_bool);
            _volumSubBtn1.gameObject.SetActive(_bool);
            Debug.Log("aaa");
            return;
        }
    }
    public void OptionToggle2(bool _bool)
    {
        _index = 1;
        if (_toggleGroup.activeSelf)
        {
            _text2.gameObject.SetActive(!_bool);
            return;
        }
        if (_soundOption.activeSelf)
        {
            _soundText2.gameObject.SetActive(!_bool);
            _volumAddBtn2.gameObject.SetActive(_bool);
            _volumSubBtn2.gameObject.SetActive(_bool);
            Debug.Log("bbb");
            return;
        }
    }
    public void OptionToggle3(bool _bool)
    {
        _index = 2;
        if (_toggleGroup.activeSelf)
        {
            _text3.gameObject.SetActive(!_bool);
            return;
        }
        if (_soundOption.activeSelf)
        {
            _soundText3.gameObject.SetActive(!_bool);
            Debug.Log("ccc");
            return;
        }
    }

}
