using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    [SerializeField] private int _roomWidth; //너비
    [SerializeField] private int _roomHeight; //높이
    [SerializeField] GameObject EliteMonster = null;
    [SerializeField] GameObject Stair = null;
    [SerializeField] GameObject Box = null;

    List<Vector3> _Roomindex = new List<Vector3>();

    public List<Vector3> Roomindex 
    {
        set { _Roomindex = value; }
        get { return _Roomindex; }  
    }

    void Start()
    {
        CalculateRoomSize();
    }
  
    public void CalculateRoomSize() // 방초기화
    {
        _Roomindex.Clear();

        for (int i = 0; i < _roomWidth; i++)  //
        {
            for (int j = 0; j < _roomHeight; j++)
            {
                Vector3 vec = new Vector3(transform.position.x + i, transform.position.y + j);
                bool b = true;
                if (EliteMonster != null && vec == EliteMonster.transform.position) b = false;
                if (Stair != null && vec == Stair.transform.position) b = false;
                if (Box != null && vec == Box.transform.position) b = false;
                if (b) _Roomindex.Add(new Vector3(transform.position.x + i, transform.position.y + j, 0));
            }
        }
    }
}
