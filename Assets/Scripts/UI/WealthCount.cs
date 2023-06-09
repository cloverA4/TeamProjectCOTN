using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WealthCount : MonoBehaviour
{
    [SerializeField] Text _goldCount;
    [SerializeField] Text _diamondCount;
    public int _gold;
    public int _diamond;


    private void Update()
    {
        _goldCount.text = "X " + _gold;
        _diamondCount.text = "X "+ _diamond;
    }
}
