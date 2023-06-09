using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Unity.VisualScripting;
using UnityEditor.Build.Content;
using UnityEngine;
using UnityEngine.UI;

public class UIManeger : MonoBehaviour
{
    [SerializeField] int _maxHP;

    [SerializeField] int _hpCount;

    [SerializeField] GameObject _heart;

    [SerializeField] Transform _HPPanel;


    private void Start()
    {
       

        setMaxHP();
    }

    private void Update()
    {
    //    if (Input.GetMouseButton(0))
    //    {
    //        setHP(_hpCount);
    //    }
    }

    void setMaxHP()
    {
        for (int i = 0; i < _maxHP; i++)
        {
            GameObject obj = Instantiate(_heart, _HPPanel);
            obj.gameObject.name = "emptyHP" + i;
        }
    }

    void setHP(float hp)
    {

        for (int i = 0; i < _maxHP; i++)
        {
            string str = "HP" + i;

            if (i < hp)
            {

            }
        }
    }
}