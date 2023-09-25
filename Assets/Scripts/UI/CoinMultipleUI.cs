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
    }

    public void OnCoinMultiple(float multiple , int multipleIndex)
    {
        if (multiple > 0)
        {
            if(multipleIndex < 2)
            {
                _multipleText.text = multiple.ToString();
                _multipleText.color = Color.white;
            }
            else if(multipleIndex >= 2)
            {
                _multipleText.text = multiple.ToString();
                _multipleText.color = Color.red;
            }
           
            // 코인 배수가 최대치 일때 글자 색이 빨간색으로 변하는 조건 추가
        }
    }

    public void DisableCoinMUltiple()
    {
        gameObject.SetActive(false);
    }
    public void ActiveCoinMultiple()
    {
        gameObject.SetActive(true);
    }
}
