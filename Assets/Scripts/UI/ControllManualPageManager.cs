using UnityEngine;
using UnityEngine.UI;

public class ControllManualPageManager : MonoBehaviour
{
    [SerializeField] GameObject[] _pages;
    [SerializeField] Button _prevButton; // 이전 페이지 버튼
    [SerializeField] Button _nextButton; // 다음 페이지 버튼
    [SerializeField] GameObject _manual;

    private int _currentPageIndex = 0; // 현재 페이지
    bool _isActive = false;
    public bool IsActive { set { _isActive = value; } }

    bool _isOption = false;
    public bool IsOption { set { _isOption = value; } }
    private void Awake()
    {
        _manual.SetActive(false);
    }

    void Start()
    {
        UpdateButtonState();
    }

    private void Update()
    {
        if (_isActive)
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                UIManeger.Instance.PlayEffectSound(SoundEffect.UIChange);
                PreviousPage();
            }
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                UIManeger.Instance.PlayEffectSound(SoundEffect.UIChange);
                NextPage();
            }
            if(Input.GetKeyDown(KeyCode.Escape))
            {
                OffMenual();
            }
        }
    }
    public void NextPage() //다음 페이지로 넘어가기
    {
        if(_currentPageIndex <= 0)
        {
            _pages[_currentPageIndex].SetActive(false);
            _currentPageIndex++;
            _pages[_currentPageIndex].SetActive(true);
            UpdateButtonState();

        }
    }    
    public void PreviousPage() //이전 페이지로 넘어가기
    {
        if (_currentPageIndex > 0)
        {
            _pages[_currentPageIndex].SetActive(false);
            _currentPageIndex--;
            _pages[_currentPageIndex].SetActive(true);
            UpdateButtonState();
        }   
    }
    public void ExitManual()
    {
        UIManeger.Instance.OffControllManual();
    }
    private void UpdateButtonState()
    {
        _prevButton.interactable = _currentPageIndex > 0;
        _nextButton.interactable = _currentPageIndex < _pages.Length - 1;
    }
    public void OnMenual()
    {
        _manual.SetActive(true);
    }
    public void OffMenual()
    {
        if(_isOption)
        {
            _isOption = false;
            StartCoroutine(UIManeger.Instance.ActiveMenuChange(UIMenu.Option));
        }
        else
        {
            PlayerController.Instance.IsTimeStop = false;
            StartCoroutine(UIManeger.Instance.ActiveMenuChange(UIMenu.Null));
        }
        _manual.SetActive(false);
    }
}
