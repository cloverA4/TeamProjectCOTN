using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    [SerializeField]
    private int _roomWidth; //�ʺ�
    [SerializeField]
    private int _roomHeight; //����

    
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

    
  
    public void CalculateRoomSize() // ���ʱ�ȭ
    {
        //RaycastHit2D _hitdataArea = Physics2D.Raycast(transform.position, Vector3.right);
        //_roomWidth = (int)(_hitdataArea.collider.transform.position.x - transform.position.x);
        //_hitdataArea = Physics2D.Raycast(transform.position, Vector3.up);
        //_roomHeight = (int)(_hitdataArea.collider.transform.position.y - transform.position.y);
        _Roomindex.Clear();

        for (int i = 0; i < _roomWidth; i++)  //
        {
            for (int j = 0; j < _roomHeight; j++) 
            {
                _Roomindex.Add(new Vector3(transform.position.x + i, transform.position.y + j, 0));
            }
        }
    }
}
