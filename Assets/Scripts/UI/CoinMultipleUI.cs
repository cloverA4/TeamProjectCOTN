using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoinMultipleUI : MonoBehaviour
{
    [SerializeField] GameObject _text;
    [SerializeField] Text _multipleText;

    private void Start()
    {
        gameObject.SetActive(false);
    }

    public void OnCoinMultiple(float multiple , int multipleIndex)
    {
        if (multiple > 0)
        {
            if(multipleIndex < 2)
            {
                _multipleText.text = multiple.ToString();
                gameObject.SetActive(true);
            }
            else if(multipleIndex >= 2)
            {
                _multipleText.text = multiple.ToString();
                gameObject.SetActive(true);
                _multipleText.color = Color.red;
            }
           
            // ���� ����� �ִ�ġ �϶� ���� ���� ���������� ���ϴ� ���� �߰�
        }
        else if(multiple <= 0)
        {
            gameObject.SetActive(false);
        }


    }
}
