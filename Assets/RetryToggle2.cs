using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RetryToggle2 : MonoBehaviour
{
    [SerializeField] GameObject _retryCheck2;
    [SerializeField] GameObject _retryText2;

    public void OnCheck()
    {
        _retryCheck2.SetActive(true);
        _retryText2.SetActive(false);
    }
    public void OffCheck()
    {
        _retryCheck2.SetActive(false);
        _retryText2.SetActive(true);
    }

}
