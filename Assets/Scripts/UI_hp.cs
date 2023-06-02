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

    public List<Image> Hearts;

    public Sprite Back, Front, halfHeart;

    int count = 0;


    private void Start()
    {
        for (int i = 0; i < _MaxHp; i++)
        {
            Image obj = Instantiate(_gameObject, new Vector3(count--, 0, 0), Quaternion.identity, transform);
            Hearts.Add(obj.GetComponent<Image>());
        }
    }
    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            setHp(_damge);
        }
    }

    void setHp(float dmg)
    {
        float _Hp = _MaxHp - dmg;

        for (int i = 0; i < _MaxHp; i++)
        {
            if (_Hp > i)
            {
                Hearts[i].sprite = Front;

                if (_Hp - (int)_Hp == 0.5 && (int)_Hp == i)
                {
                    Hearts[i].sprite = halfHeart;
                }
            }
        }
    }


}