using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Note : MonoBehaviour
{
    RectTransform rec = new RectTransform();
    RectTransform _judgemonet;
    int _speed;

    public void Init()
    {
        _speed = 1000;
        rec = GetComponent<RectTransform>();
        //판정선
        if (_judgemonet == null) _judgemonet = GameObject.Find("Judgement").GetComponent<RectTransform>();
        //시작위치
        rec.anchoredPosition = new Vector3(1000, -490, 0);

        gameObject.SetActive(false);
    }

    public void PlayNote()
    {
        rec.anchoredPosition = new Vector3(1000, -490, 0);
        gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        rec.anchoredPosition3D += new Vector3(-1 * Time.deltaTime * _speed, 0, 0);
        //if(rec.anchoredPosition.x < _judgemonet.anchoredPosition.x)
        if(rec.anchoredPosition.x <= 0)
        {
            gameObject.SetActive(false);
            GameManager.Instance.ActiveNoteRemove(gameObject);
        }
    }
}
