using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapCamVer2 : MonoBehaviour
{
    [SerializeField] private Transform _stageLobbyMinimapCamPos;
    [SerializeField] private Transform _stage1F1MinimapCamPos;
    [SerializeField] private Transform _stage1F2MinimapCamPos;
    [SerializeField] private Transform _stage1F3MinimapCamPos;
    [SerializeField] private Transform _stage1FBossMinimapCamPos;

    [SerializeField] private GameObject _stageLobbyFloor;
    [SerializeField] private GameObject _stage1F1MinimapFloor;
    [SerializeField] private GameObject _stage1F2MinimapFloor;
    [SerializeField] private GameObject _stage1F3MinimapFloor;
    [SerializeField] private GameObject _stage1FBossFloor;

    void Update()
    {
        if (GameManager.Instance.NowStage == Stage.Lobby)
        {
            transform.position = _stageLobbyMinimapCamPos.position + new Vector3(0, 0, -10);
            _stageLobbyFloor.SetActive(true);
        }
        else if ((GameManager.Instance.NowStage == Stage.Stage1) && (GameManager.Instance.NowFloor == floor.f1))
        {
            transform.position = _stage1F1MinimapCamPos.position + new Vector3(0, 0, -10);
            _stage1F1MinimapFloor.SetActive(true);
        }
        else if ((GameManager.Instance.NowStage == Stage.Stage1) && (GameManager.Instance.NowFloor == floor.f2))
        {
            transform.position = _stage1F2MinimapCamPos.position + new Vector3(0, 0, -10);
            _stage1F2MinimapFloor.SetActive(true);
        }
        else if ((GameManager.Instance.NowStage == Stage.Stage1) && (GameManager.Instance.NowFloor == floor.f3))
        {
            transform.position = _stage1F3MinimapCamPos.position + new Vector3(0, 0, -10);
            _stage1F3MinimapFloor.SetActive(true);
        }
        else if ((GameManager.Instance.NowStage == Stage.Stage1) && (GameManager.Instance.NowFloor == floor.fBoss))
        {
            transform.position = _stage1FBossMinimapCamPos.position + new Vector3(0, 0, -10);
            _stage1FBossFloor.SetActive(true);
        }
    }
}
