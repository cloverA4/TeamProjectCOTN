using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Note : MonoBehaviour
{
    RectTransform rec = new RectTransform();
    RectTransform _judgemonet;
    int _speed;
    int _dir = 0;

    public void Init()
    {
        _speed = 500;
        rec = GetComponent<RectTransform>();
        //판정선
        if (_judgemonet == null) _judgemonet = GameObject.Find("Judgement").GetComponent<RectTransform>();
        //시작위치
        rec.anchoredPosition = new Vector3(1000, -450, 0);
        

        gameObject.SetActive(false);
    }

    public void PlayNote(int dir, int StartPosition)
    {
        _dir = dir;
        rec.anchoredPosition = new Vector3(StartPosition, -450, 0);
        gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        rec.anchoredPosition3D += new Vector3(_dir * Time.deltaTime * _speed, 0, 0);
        //if(rec.anchoredPosition.x < _judgemonet.anchoredPosition.x)
        if (_dir > 0)
        {
            if (rec.anchoredPosition.x >= 0)
            {
                gameObject.SetActive(false);
                GameManager.Instance.LeftNoteRemove(gameObject);
                GameManager.Instance.MissBeat();
            }
        }
        else
        {
            if (rec.anchoredPosition.x <= 0)
            {
                gameObject.SetActive(false);
                GameManager.Instance.RightNoteRemove(gameObject);
            }
        }
        
    }
}
