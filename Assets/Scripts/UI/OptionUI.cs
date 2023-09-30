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

    [SerializeField] Text _soundText1;
    [SerializeField] Text _soundText2;
    [SerializeField] Slider _soundBar1;
    [SerializeField] Slider _soundBar2;

    [SerializeField] Button _button;

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
            }
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                _index--;
                if (_index < 0) _index = 2;
            }
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
                if (gameObject.activeSelf == false)
                {
                    Time.timeScale = 1;
                }
            }
        }
       
    }


    public void Init()
    {
        gameObject.SetActive(false);
        _index = 0;
        _soundOption.gameObject.SetActive(false);
        _soundBar1.value = 0.5f;
        _soundBar2.value = 0.5f;
    }

    public void StartOptionUI(string mainText, string toggle1Text, string toggle2Text,string toggle3Text , UnityAction action1, UnityAction action2, UnityAction action3)
    {
        Time.timeScale = 0;
        _action1 = action1;
        _action2 = action2;
        _action3 = action3;

        _index = 0;
        gameObject.SetActive(true);
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

    public void SoundOption(string main)
    {
        _main.text = main;
        _soundOption.gameObject.SetActive(true);
    }
    public void SetMusicSound(float value)
    {
        GameManager.Instance.AudioVolumControll(value);
        _soundText1.text = "À½¾Ç º¼·ý: " + (int)(value * 100) + "%";
    }
    public void SetEffectSound(float value)
    {
        // ÀÌÆåÆ® ¿É¼Ç°ª
        _soundText2.text = "À½¾Ç º¼·ý: " + (int)(value * 100) + "%";
    }

    public void EndOptionUI()
    {
        gameObject.SetActive(false);
    }

    public void OnBtn()
    {
        _soundOption.gameObject.SetActive(false);
        _toggleGroup.gameObject.SetActive(true);
    }

    public void OptionToggle1(bool _bool)
    {
        _text1.gameObject.SetActive(!_bool);
    }
    public void OptionToggle2(bool _bool)
    {
        _text2.gameObject.SetActive(!_bool);
    }
    public void OptionToggle3(bool _bool)
    {
        _text3.gameObject.SetActive(!_bool);
    }

}
