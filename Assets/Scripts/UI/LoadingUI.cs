using UnityEngine;
using System;

public class LoadingUI : MonoBehaviour
{
    [SerializeField] GameObject _loadingText;
    [SerializeField] GameObject _infoText;

    bool _isLoading;

    private void Start()
    {
        Data.Instance.LoadingEnd += new EventHandler(LoadingEnd);
        _isLoading = false;
        LoadingInit();
    }

    private void Update()
    {
        if (_isLoading)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Application.Quit();
            }
            else if (Input.anyKeyDown)
            {
                Data.Instance.SceneChange();
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.F8))
            {
                PlayerPrefs.DeleteAll();
            }
        }
    }
    void LoadingEnd(object sender, EventArgs s)
    {
        DisableLoadingText();
        _isLoading = true;
    }
    void LoadingInit()
    {
        gameObject.SetActive(true);
        _infoText.SetActive(false);
        _loadingText.SetActive(true);
    }
    void DisableLoadingText()
    {
        _loadingText.SetActive(false);
        _infoText.SetActive(true);
    }
}
