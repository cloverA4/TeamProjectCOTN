using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllManual : MonoBehaviour
{
    [SerializeField] GameObject _manual;

    private void Start()
    {
        _manual.SetActive(false);
    }

    public void OnManual()
    {
        _manual.SetActive(true);
    }

    public void OffManual()
    {
        _manual.SetActive(false);
    }
}
