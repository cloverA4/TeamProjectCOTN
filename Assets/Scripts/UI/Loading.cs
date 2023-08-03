using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class Loading : MonoBehaviour
{
    [SerializeField] GameObject _loadingText;
    [SerializeField] GameObject _infoText;
    private AsyncOperation operation;

    private void Start()
    {
        _infoText.SetActive(false);
        _loadingText.SetActive(true);
        StartCoroutine(LoadScene());

    }

    private void Update()
    {
        if (_infoText)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                // 게임 종료
            }
            else
            {
                SceneManager.LoadSceneAsync("GameScene");
            }
        }
    }

    IEnumerator LoadScene()
    {
        operation = SceneManager.LoadSceneAsync("GameScene");
        operation.allowSceneActivation = false;

        while (!operation.isDone)
        {
            yield return null;

            //timer += Time.deltaTime;
            //if (operation.progress < 0.9f)
            //{
            //    _slider.value = Mathf.Lerp(operation.progress, 1f, timer);
            //    if (_slider.value >= operation.progress)
            //        timer = 0f;
            //}
            //else
            //{
            //    _slider.value = Mathf.Lerp(_slider.value, 1f, timer);
            //    if (_slider.value >= 0.99f)
            //        operation.allowSceneActivation = true;
            //}
        }

    }


}
