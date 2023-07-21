using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyToggle : MonoBehaviour
{
    [SerializeField] GameObject _lobbyCheck;
    [SerializeField] GameObject _lobbyText;
    
    public void OnCheck()
    {
        _lobbyCheck.SetActive(true);
        _lobbyText.SetActive(false);
    }
    public void OffCheck()
    {
        _lobbyCheck.SetActive(false);
        _lobbyText.SetActive(true);
    }
}
