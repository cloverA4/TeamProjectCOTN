using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class OptionUI : MonoBehaviour
{
    [SerializeField] GameObject _toggleGroup;
    [SerializeField] GameObject _soundOption;

    [SerializeField] Toggle _toggle1;
    [SerializeField] Toggle _toggle2;
    [SerializeField] Toggle _toggle3;
    [SerializeField] Toggle _toggle4;

    [SerializeField] Text _check1;
    [SerializeField] Text _check2;
    [SerializeField] Text _check3;
    [SerializeField] Text _check4;

    [SerializeField] Text _text1;
    [SerializeField] Text _text2;
    [SerializeField] Text _text3;
    [SerializeField] Text _text4;

    [SerializeField] Text _main;

    int _index = 0;
    UnityAction _action1;
    UnityAction _action2;
    UnityAction _action3;
    UnityAction _action4;

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
    int _soundIndex = 0;


    [SerializeField] GameObject _gameObject;

    AudioSource _audio;
    bool _isActive = false;
    public bool IsActive { set { _isActive = value; } }

    // ���� ������ ���콺

    private void Start()
    {
        Init();
    }

    private void Update()
    {
        if (_isActive)
        {
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                UIManeger.Instance.PlayEffectSound(SoundEffect.UIChange);
                _index++;
                if (_index > 3) _index = 0;
                CheckToggle();
            }
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                UIManeger.Instance.PlayEffectSound(SoundEffect.UIChange);
                _index--;
                if (_index < 0) _index = 3;
                CheckToggle();
            }
            

            if (Input.GetKeyDown(KeyCode.Return))
            {
                UIManeger.Instance.PlayEffectSound(SoundEffect.UISelect);
                switch (_index)
                {
                    case 0:
                        _action1();
                        break;
                    case 1:
                        if (_action2 != null) _action2();
                        break;
                    case 2:
                        if (_action3 != null) _action3();
                        break;
                    case 3:
                        if(_action4 != null) _action4();
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
                UIManeger.Instance.PlayEffectSound(SoundEffect.UIChange);
                _soundIndex++;
                if (_soundIndex > 2) _soundIndex = 0;
                CheckToggle();
            }
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                UIManeger.Instance.PlayEffectSound(SoundEffect.UIChange);
                _soundIndex--;
                if (_soundIndex < 0) _soundIndex = 2;
                CheckToggle();
            }
            if(Input.GetKeyDown(KeyCode.LeftArrow))
            {
                UIManeger.Instance.PlayEffectSound(SoundEffect.UIChange);
                if (_soundToggle1.isOn) _soundBar1.value -= 0.05f;
                if (_soundToggle2.isOn) _soundBar2.value -= 0.05f;
            }
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                UIManeger.Instance.PlayEffectSound(SoundEffect.UIChange);
                if (_soundToggle1.isOn) _soundBar1.value += 0.05f;
                if (_soundToggle2.isOn) _soundBar2.value += 0.05f;
            }
            if (Input.GetKeyDown(KeyCode.Return))
            {
                UIManeger.Instance.PlayEffectSound(SoundEffect.UISelect);
                if (_soundIndex == 2) OffSoundOption();
            }
        }
    }

    public void Init()
    {
        gameObject.SetActive(true);
        _gameObject.SetActive(false);
        _index = 0;
        _soundIndex = 0;
        CheckToggle();
        _soundOption.gameObject.SetActive(false);
        _soundBar1.value = 0.5f;
        _soundBar2.value = 0.5f;
    }

    public void StartOptionUI(string mainText, string toggle1Text, string toggle2Text,string toggle3Text, string toggle4Text, UnityAction action1, UnityAction action2, UnityAction action3, UnityAction action4)
    {
        Time.timeScale = 0;
        GameManager.Instance.SoundPerse(false);
        PlayerController.Instance.IsTimeStop = true;
        _gameObject.SetActive(true);
        _action1 = action1;
        _action2 = action2;
        _action3 = action3;
        _action4 = action4;

        _index = 0;
        _toggleGroup.gameObject.SetActive(true);
        _soundOption.gameObject.SetActive(false);
        _toggle1.isOn = true;
        _text1.gameObject.SetActive(false);
        _text2.gameObject.SetActive(true);
        CheckToggle();

        _main.text = mainText;

        _check1.text = "-" + toggle1Text + "-";
        _text1.text = toggle1Text;

        _check2.text = "-" + toggle2Text + "-";
        _text2.text = toggle2Text;

        _check3.text = "-" + toggle3Text + "-";
        _text3.text = toggle3Text;

        _check4.text = "-" + toggle4Text + "-";
        _text4.text = toggle4Text;
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
            case 3:
                _toggle4.isOn = true;
                break;
        }
        switch (_soundIndex)
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

    public void SoundOption(string main)
    {
        _index = 0;
        _soundIndex = 0;
        _main.text = main;
        _toggleGroup.gameObject.SetActive(false);
        _soundOption.gameObject.SetActive(true);
        OptionToggle1(true);
        CheckToggle();
    }
    void OffSoundOption()
    {
        _index = 0;
        _soundIndex = 0;
        _soundOption.gameObject.SetActive(false);
        _toggleGroup.gameObject.SetActive(true);
        OptionToggle1(true);
        CheckToggle();
    }

    public void SetMusicSound(float value)
    {
        float temp = ((float)Mathf.RoundToInt(value * 100) / 100);
        _soundBar1.SetValueWithoutNotify(temp);
        GameManager.Instance.AudioVolumControll(temp);
        _soundText1.text = "���� ����: " + temp * 100 + "%";
        _soundCheck1.text = "���� ����: " + temp * 100 + "%";
    }
    public void SetEffectSound(float value)
    {
        // ����Ʈ �ɼǰ�
        float temp = ((float)Mathf.RoundToInt(value * 100) / 100);
        _soundBar2.SetValueWithoutNotify(temp);
        GameManager.Instance.AudioVolumControll(temp);
        _soundText2.text = "����Ʈ ����: " + temp * 100 + "%";
        _soundCheck2.text = "����Ʈ ����: " + temp * 100 + "%";
        UIManeger.Instance.VolumeChange(temp);
    }
    //public void 

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
        UIManeger.Instance.ActiveMenuChange(UIMenu.Null);
    }


    public void OptionToggle1(bool _bool)
    {
        if (_toggleGroup.activeSelf)
        {
            _index = 0;
            _text1.gameObject.SetActive(!_bool);
            return;
        }
        if (_soundOption.activeSelf)
        {
            _soundIndex = 0;
            _soundText1.gameObject.SetActive(!_bool);
            _volumAddBtn1.gameObject.SetActive(_bool);
            _volumSubBtn1.gameObject.SetActive(_bool);
            return;
        }
    }
    public void OptionToggle2(bool _bool)
    {
        if (_toggleGroup.activeSelf)
        {
            _index = 1;
            _text2.gameObject.SetActive(!_bool);
            return;
        }
        if (_soundOption.activeSelf)
        {
            _soundIndex = 1;
            _soundText2.gameObject.SetActive(!_bool);
            _volumAddBtn2.gameObject.SetActive(_bool);
            _volumSubBtn2.gameObject.SetActive(_bool);
            return;
        }
    }
    public void OptionToggle3(bool _bool)
    {
        if (_toggleGroup.activeSelf)
        {
            _index = 2;
            _text3.gameObject.SetActive(!_bool);
            return;
        }
        if (_soundOption.activeSelf)
        {
            _soundIndex = 2;
            _soundText3.gameObject.SetActive(!_bool);
            return;
        }
    }
    public void OptionToggle4(bool _bool)
    {
        if (_toggleGroup.activeSelf)
        {
            _index = 3;
            _text4.gameObject.SetActive(!_bool);
            return;
        }
    }
}
