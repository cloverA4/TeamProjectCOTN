using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericSingleton<T> : MonoBehaviour where T : MonoBehaviour
{
    public static GenericSingleton<T> Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            OnWake();
        }
        else
        {
            GameObject.Destroy(gameObject);
        }
    }

    protected virtual void OnWake()
    {
    }
}
