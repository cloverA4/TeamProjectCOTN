using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RePlayToggle : MonoBehaviour
{
    [SerializeField] GameObject _replayCheck;
    [SerializeField] GameObject _replayText;

    public void OnCheck()
    {
        _replayCheck.SetActive(true);
        _replayText.SetActive(false);
    }
    public void OffCheck()
    {
        _replayCheck.SetActive(false);
        _replayText.SetActive(true);
    }
}
