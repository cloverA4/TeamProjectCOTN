using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallAndDoorManager : MonoBehaviour
{
    [SerializeField]
    GameObject Stage1F1WeedWallList;
    [SerializeField]
    GameObject Stage1F1DoorList;
    [SerializeField]
    GameObject Stage1F2WeedWallList;
    [SerializeField]
    GameObject Stage1F2DoorList;
    [SerializeField]
    GameObject Stage1F3WeedWallList;
    [SerializeField]
    GameObject Stage1F3DoorList;


    public void ResetWallAndDoor()
    {
        //������� ��������
        //������ �����Ҷ�

        //WeedWallList �ʱ�ȭ
        for (int i = 0; i < Stage1F1WeedWallList.transform.childCount; i++)
        {
            Stage1F1WeedWallList.transform.GetChild(i).GetComponent<Wall>().WallReset();
        }
        for (int i = 0; i < Stage1F2WeedWallList.transform.childCount; i++)
        {
            Stage1F2WeedWallList.transform.GetChild(i).GetComponent<Wall>().WallReset();
        }
        for (int i = 0; i < Stage1F3WeedWallList.transform.childCount; i++)
        {
            Stage1F3WeedWallList.transform.GetChild(i).GetComponent<Wall>().WallReset();
        }

        //Door �ʱ�ȭ
        for (int i = 0; i < Stage1F1DoorList.transform.childCount; i++)
        {
            Stage1F1DoorList.transform.GetChild(i).GetComponent<Door>().DoorReset();
        }
        for (int i = 0; i < Stage1F2DoorList.transform.childCount; i++)
        {
            Stage1F2DoorList.transform.GetChild(i).GetComponent<Door>().DoorReset();
        }
        for (int i = 0; i < Stage1F3DoorList.transform.childCount; i++)
        {
            Stage1F3DoorList.transform.GetChild(i).GetComponent<Door>().DoorReset();
        }

    }
}
