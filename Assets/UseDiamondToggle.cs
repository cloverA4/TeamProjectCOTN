using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public class UseDiamondToggle : MonoBehaviour
{
    [SerializeField] GameObject _useDiamondCheck;
    [SerializeField] GameObject _useDiamondText;

    public void OnCheck()
    {
        _useDiamondCheck.SetActive(true);
        _useDiamondText.SetActive(false);
    }
    public void OffCheck()
    {
        _useDiamondCheck.SetActive(false);
        _useDiamondText.SetActive(true);
    }
}
