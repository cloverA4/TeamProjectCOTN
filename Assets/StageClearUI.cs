using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageClearUI : MonoBehaviour
{
    [SerializeField] GameObject _clearText;
    [SerializeField] GameObject _clearPE;

    public void Start()
    {
        _clearPE.SetActive(false);
        _clearText.SetActive(false);
    }

    public void OnClearEffect()
    {
        _clearText.SetActive(true);
        _clearPE.SetActive(true);
        Invoke("OffClearEffect", 10f);     
    }

    void OffClearEffect()
    {
        _clearText.SetActive(false);
        _clearPE.SetActive(false);
    }
}
