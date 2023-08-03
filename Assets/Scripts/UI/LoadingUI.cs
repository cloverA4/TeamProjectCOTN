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
        if (_infoText) // �ε��� ������ ���� �ؽ�Ʈ�� ������
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                // ���� ����
                Debug.Log("���� ����");
                Application.Quit();
            }
            else if(Input.anyKeyDown)
            {
                // loadingUI�� ��
                gameObject.SetActive(false);
            }
        }
    }

    void LoadGame()
    {
        // data�� LoadData �Լ� ȣ��
        //Data.Instance.LoadData();
        if (SceneManager.GetActiveScene().name != "GameScene") SceneManager.LoadScene("GameScene"); // ���Ӿ� �ε�

        _loadingText.SetActive(false);
        _infoText.SetActive(true);
    }



}
