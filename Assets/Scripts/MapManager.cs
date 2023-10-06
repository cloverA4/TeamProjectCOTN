using UnityEngine;

public class MapManager : MonoBehaviour
{
    //로비 맵 데이터
    [SerializeField] GameObject StageLobbyDoorList;
    [SerializeField] GameObject LobbyBenStair;
    //스테이지1 1층 맵 데이터
    [SerializeField] GameObject Stage1F1WeedWallList;
    [SerializeField] GameObject Stage1F1DoorList;
    [SerializeField] GameObject Stage1F1BenStair;
    //스테이지1 2층 맵 데이터
    [SerializeField] GameObject Stage1F2WeedWallList;
    [SerializeField] GameObject Stage1F2DoorList;
    [SerializeField] GameObject Stage1F2BenStair;
    //스테이지1 3층 맵 데이터
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
                    //WeedWallList 초기화
                    //Door 초기화
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
