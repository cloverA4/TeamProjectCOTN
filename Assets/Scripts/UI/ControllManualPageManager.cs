using UnityEngine;
using UnityEngine.UI;

public class ControllManualPageManager : MonoBehaviour
{
    [SerializeField] GameObject[] _pages;
    [SerializeField] Button _prevButton; // 이전 페이지 버튼
    [SerializeField] Button _nextButton; // 다음 페이지 버튼
    [SerializeField] GameObject _manual;

    private int _currentPageIndex = 0; // 현재 페이지

    void Start()
    {
        _manual.SetActive(false);
        UpdateButtonState();        
    }

    private void Update()
    {
        if (_manual.activeSelf)
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
        }
    }

    //다음 페이지로 넘어가기
    public void NextPage()
    {
        if(_currentPageIndex <= 0)
        {
            _pages[_currentPageIndex].SetActive(false);
            _currentPageIndex++;
            _pages[_currentPageIndex].SetActive(true);
            UpdateButtonState();

        }
    }

    //이전 페이지로 넘어가기
    public void PreviousPage()
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
        _manual.SetActive(false);
    }

}
