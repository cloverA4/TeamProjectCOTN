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

    public void OnCoinMultiple(float multiple)
    {
        if (multiple > 0)
        {
            _multipleText.text = multiple.ToString();
            gameObject.SetActive(false);
            // 코인 배수가 최대치 일때 글자 색이 빨간색으로 변하는 조건 추가
        }
        else if(multiple <= 0)
        {
            gameObject.SetActive(false);
        }


    }
}
