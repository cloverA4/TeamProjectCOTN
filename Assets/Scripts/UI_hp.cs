using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Unity.VisualScripting;
using UnityEditor.Build.Content;
using UnityEngine;
using UnityEngine.UI;

public class UI_hp : MonoBehaviour
{
    [SerializeField] float _MaxHp;

    [SerializeField] float _damge;
    [SerializeField] Image _gameObject;
    [SerializeField] Transform _gridPanel;


    [SerializeField] Sprite Back, Front;

    int count = 0;



    private void Start()
    {
        setMaxHP();
    }

    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            setHp(_damge);
        }
    }

    void setMaxHP()
    {
        //�ߺ����� ���ƾ���.
        for (int i = 0; i < _MaxHp; i++)
        {
            Image obj = Instantiate(_gameObject, _gridPanel);
            obj.gameObject.name = "HP" + i;
        }
    }

    void setHp(float hp)
    {
        for (int i = 0; i < _MaxHp; i++)
        {
            string str = "HP" + i;
            if (i > hp)
            {
                Image im = _gridPanel.Find(str).GetComponent<Image>();
                im.sprite = Back;
                //��ĭ
            }
            else
            {
                //������Ʈ
                Image im = _gridPanel.Find(str).GetComponent<Image>();
                im.sprite = Front;
            }
        }
    }


}