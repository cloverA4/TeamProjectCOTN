using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RetryToggle : MonoBehaviour
{
    [SerializeField] GameObject _retryCheck;
    [SerializeField] GameObject _retryText;

    public void OnCheck()
    {
        _retryCheck.SetActive(true);
        _retryText.SetActive(false);
    }
    public void OffCheck()
    {
        _retryCheck.SetActive(false);
        _retryText.SetActive(true);
    }
}
