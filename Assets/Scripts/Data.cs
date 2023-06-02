using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Experimental.GraphView.GraphView;

public class Data : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {

    }
}

public class Item
{
    int _ItemID;

    public int ItemID
    {
        get { return _ItemID; }
        set
        {
            _ItemID = value;
        }
    }

    string _Name;
    public string Name
    {
        get { return _Name; }
        set
        {
            _Name = value;
        }
    }

    enum ItemType
    {
        Shovel,
        Weapon,
        Armor,
        Potion,
    }
    

}
