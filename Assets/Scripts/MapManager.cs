using UnityEngine;

public class MapManager : MonoBehaviour
{
    //�κ� �� ������
    [SerializeField] GameObject StageLobbyDoorList;
    [SerializeField] GameObject LobbyBenStair;
    //��������1 1�� �� ������
    [SerializeField] GameObject Stage1F1WeedWallList;
    [SerializeField] GameObject Stage1F1DoorList;
    [SerializeField] GameObject Stage1F1BenStair;
    //��������1 2�� �� ������
    [SerializeField] GameObject Stage1F2WeedWallList;
    [SerializeField] GameObject Stage1F2DoorList;
    [SerializeField] GameObject Stage1F2BenStair;
    //��������1 3�� �� ������
    [SerializeField] GameObject Stage1F3WeedWallList;
    [SerializeField] GameObject Stage1F3DoorList;
    [SerializeField] GameObject Stage1F3BenStair;

    public void ResetMapObject()
    {
        switch (GameManager.Instance.NowStage)
        {
            case Stage.Lobby:
                switch (GameManager.Instance.NowFloor)
                {
                    case floor.f1:
                        ResetDoor(StageLobbyDoorList);
                        LobbyBenStair.GetComponent<Stair>().StageLock();
                        break;
                    case floor.f2:
                    case floor.f3:
                    case floor.fBoss:
                        break;
                }
                break;
            case Stage.Stage1:
                switch (GameManager.Instance.NowFloor)
                {
                    //WeedWallList �ʱ�ȭ
                    //Door �ʱ�ȭ
                    case floor.f1:
                        ResetWall(Stage1F1WeedWallList);
                        ResetDoor(Stage1F1DoorList);
                        Stage1F1BenStair.GetComponent<Stair>().StageLock();
                        break;
                    case floor.f2:
                        ResetWall(Stage1F2WeedWallList);
                        ResetDoor(Stage1F2DoorList);
                        Stage1F2BenStair.GetComponent<Stair>().StageLock();
                        break;
                    case floor.f3:
                        ResetWall(Stage1F3WeedWallList);
                        ResetDoor(Stage1F3DoorList);
                        Stage1F3BenStair.GetComponent<Stair>().StageLock();
                        break;
                    case floor.fBoss:
                        break;
                }
                break;
            case Stage.Stage2:
                switch (GameManager.Instance.NowFloor)
                {
                    case floor.f1:
                        break;
                    case floor.f2:
                    case floor.f3:
                    case floor.fBoss:
                        break;
                }
                break;
        }
    }

    public void ResetDoor(GameObject Door)
    {
        for (int i = 0; i < Door.transform.childCount; i++)
        {
            Door.transform.GetChild(i).GetComponent<Door>().DoorReset();
        }
    }

    public void ResetWall(GameObject Wall)
    {
        for (int i = 0; i < Wall.transform.childCount; i++)
        {
            Wall.transform.GetChild(i).GetComponent<Wall>().WallReset();
        }
    }
}
