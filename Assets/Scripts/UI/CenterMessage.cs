using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CenterMessage : MonoBehaviour
{
    [SerializeField] Text _centerMessage;
    RectTransform rect;

    private void Awake()
    {
        _centerMessage.gameObject.SetActive(false);
        rect = _centerMessage.GetComponent<RectTransform>();
    }

    public void SetCenterMessage(string str)
    {
        _centerMessage.text = str;
        rect.anchoredPosition = new Vector3(0, 50, 0);
        _centerMessage.gameObject.SetActive(true);
    }

    void Update()
    {
        if(_centerMessage.gameObject.activeSelf)
        {
            if(rect.anchoredPosition.y < 125)
            {
                rect.anchoredPosition += new Vector2(0, Time.deltaTime * 40);
            }
            else
            {
                _centerMessage.gameObject.SetActive(false);
            }            
        }
    }
}
