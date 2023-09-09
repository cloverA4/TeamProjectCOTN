using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering.Universal;
using UnityEngine;
using UnityEngine.UI;

public class ItemInfo : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SetItemName("TestName");

        }
    }

    [SerializeField] GameObject _ItemInfo;

    public void SetItemName(string _name)
    {
        var temp = Instantiate(_ItemInfo,transform);
        temp.GetComponent<ItemNameCanvas>().InitItemName(name);
    }
}
