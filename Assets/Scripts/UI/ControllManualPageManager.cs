using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControllManualPageManager : MonoBehaviour
{
    [SerializeField] GameObject[] _pages;
    [SerializeField] Button _prevButton; // ���� ������ ��ư
    [SerializeField] Button _nextButton; // ���� ������ ��ư

    private int _currentPageIndex = 0; // ���� ������

    void Start()
    {
        UpdateButtonState();
    }

    //���� �������� �Ѿ��
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

    //���� �������� �Ѿ��
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

}
