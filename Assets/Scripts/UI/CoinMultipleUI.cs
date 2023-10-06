using UnityEngine;
using UnityEngine.UI;

public class CoinMultipleUI : MonoBehaviour
{
    [SerializeField] GameObject _text;
    [SerializeField] Text _multipleText;
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
