using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallAndDoorManager : MonoBehaviour
{
    [SerializeField]
    GameObject Stage1WeedWallList;
    [SerializeField]
    GameObject Stage1DoorList;

    public void ResetWallAndDoor()
    {
        //������� ��������
        //������ �����Ҷ�
        for (int i = 0; i < Stage1WeedWallList.transform.childCount; i++)
        {
            Stage1WeedWallList.transform.GetChild(i).GetComponent<Wall>().WallReset();
        }

        for (int i = 0; i < Stage1DoorList.transform.childCount; i++)
        {
            Stage1DoorList.transform.GetChild(i).GetComponent<Door>().DoorReset();
        }

    }
}
