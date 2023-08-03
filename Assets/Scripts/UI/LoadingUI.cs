using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class LoadingUI : GenericSingleton<LoadingUI>
{
    [SerializeField] GameObject _loadingText;
    [SerializeField] GameObject _infoText;

    private void Start()
    {
        LoadingInit();
        LoadGame();

    }
    void LoadingInit()
    {
        gameObject.SetActive(true);
        _infoText.SetActive(false);
        _loadingText.SetActive(true);
    }

    private void Update()
    {   
        if (_infoText) // 로딩이 끝나고 설명 텍스트가 떴을때
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                // 게임 종료
                Debug.Log("게임 종료");
                Application.Quit();
            }
            else if(Input.anyKeyDown)
            {
                // loadingUI를 끔
                gameObject.SetActive(false);
            }
        }
    }

    void LoadGame()
    {
        // data의 LoadData 함수 호출
        //Data.Instance.LoadData();
        if (SceneManager.GetActiveScene().name != "GameScene") SceneManager.LoadScene("GameScene"); // 게임씬 로드

        _loadingText.SetActive(false);
        _infoText.SetActive(true);
    }



}
