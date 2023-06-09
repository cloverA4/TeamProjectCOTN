using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class Loading : MonoBehaviour
{
    [SerializeField] Slider _slider;
    private AsyncOperation operation;

    private void Start()
    {
        _slider.value = 0;
        StartCoroutine(LoadScene());
    }

    IEnumerator LoadScene()
    {
        operation = SceneManager.LoadSceneAsync("GameScene");
        operation.allowSceneActivation = false;

        float timer = 0f;
        while (!operation.isDone)
        {
            yield return null;

            timer += Time.deltaTime;
            if (operation.progress < 0.9f)
            {
                _slider.value = Mathf.Lerp(operation.progress, 1f, timer);
                if (_slider.value >= operation.progress)
                    timer = 0f;
            }
            else
            {
                _slider.value = Mathf.Lerp(_slider.value, 1f, timer);
                if (_slider.value >= 0.99f)
                    operation.allowSceneActivation = true;
            }
        }

    }


}
