using UnityEngine;
using UnityEngine.UI;

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
        rec.anchoredPosition = new Vector3(1000, -420, 0);
        gameObject.SetActive(false);
    }

    void Update()
    {
        rec.anchoredPosition3D += new Vector3(_dir * Time.deltaTime * _speed, 0, 0);
        if (_dir > 0)
        {
            if (rec.anchoredPosition.x >= 0)
            {
                gameObject.SetActive(false);
                GameManager.Instance.LeftNoteRemove(gameObject);
                GameManager.Instance.MissBeat();
                GameManager.Instance.EndMusic();
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

    public void PlayNote(int dir, int StartPosition)
    {
        _dir = dir;
        rec.anchoredPosition = new Vector3(StartPosition, -420, 0);
        gameObject.SetActive(true);

        if (GameManager.Instance.Audio.time >= GameManager.Instance.Audio.clip.length - 30)
        {
            GetComponent<Image>().color = new Color32(255, 85, 91, 255);
        }
        else
        {
            GetComponent<Image>().color = new Color32(64, 244, 255, 255);
        }
    }
}
