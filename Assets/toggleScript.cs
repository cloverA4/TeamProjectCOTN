using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class toggleScript : MonoBehaviour
{
    [SerializeField] GameObject _t1;
    [SerializeField] GameObject _t2;


    public void selectTestToggle()
    {
        _t1.gameObject.SetActive(false);
    }

    public void selectTest2Toggle()
    {
        _t2.gameObject.SetActive(false);
    }
}
