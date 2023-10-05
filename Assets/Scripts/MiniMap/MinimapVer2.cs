using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MinimapVer2 : MonoBehaviour
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

    [SerializeField] RawImage _miniMapImage; // UI RawImage 요소
    [SerializeField] Camera _miniMapCamera; // 미니맵 카메라
    private int _textureWidth = 40; // 텍스처 너비
    private int _textureHeight = 40; // 텍스처 높이
    private float _explorationDistance = 5.0f; // 탐험 거리

    private Texture2D _miniMapTexture; // 미니맵 텍스처
    private Transform _PlayerPos;

    private Color[] visitedColorData; // 초기의 컬러데이터

    private void Start()
    {
        _PlayerPos = PlayerController.Instance.gameObject.transform;

        _miniMapTexture = new Texture2D(_textureWidth, _textureHeight);
        _miniMapTexture.filterMode = FilterMode.Point;
        visitedColorData = new Color[_textureWidth * _textureHeight];
        int index = 0;

        for (int x = 0; x < _textureHeight; x++)
        {
            for (int y = 0; y < _textureWidth; y++)
            {
                visitedColorData[index] = Color.clear;
                index++;
            }
        }


    }


    void _UpdateMiniMapTexture()
    {

        

        Color[] pixels = _miniMapTexture.GetPixels();
        Vector3 playerPosition = _PlayerPos.position;

        for (int y = 0; y < _textureHeight; y++)
        {
            for (int x = 0; x < _textureWidth; x++)
            {
                int pixelIndex = y * _textureWidth + x;
                Color pixelColor = pixels[pixelIndex];

                // 현재 픽셀과 플레이어의 거리 계산
                Vector3 pixelWorldPosition = _miniMapCamera.ViewportToWorldPoint(new Vector3(x / (float)_textureWidth, y / (float)_textureHeight, 0));
                float distanceToPlayer = Vector3.Distance(pixelWorldPosition, playerPosition);

                // 거리가 탐험 거리 이내인 경우 알파값을 1로, 그렇지 않으면 알파값을 0으로 설정
                if (distanceToPlayer <= _explorationDistance)
                {
                    pixelColor.a = 1f; // 알파값을 1로 설정하여 탐험한 지역으로 표시
                }
                else
                {
                    pixelColor.a = 0f; // 알파값을 0으로 설정하여 탐험하지 않은 지역으로 표시
                }

                pixels[pixelIndex] = pixelColor;
            }
        }

        _miniMapTexture.SetPixels(pixels);
        _miniMapTexture.Apply();
    }

    void Update()
    {
        _miniMapTexture.SetPixels(visitedColorData);

        _miniMapImage.texture = _miniMapTexture; // UI RawImage에 텍스처 할당

        RenderTexture renderTexture = new RenderTexture(_textureWidth, _textureHeight, 24);

        _miniMapCamera.targetTexture = renderTexture;
        _miniMapCamera.Render();

        // RenderTexture를 Texture2D로 복사
        RenderTexture.active = renderTexture;
        _miniMapTexture.ReadPixels(new Rect(0, 0, _textureWidth, _textureHeight), 0, 0);
        _miniMapTexture.Apply();
        RenderTexture.active = null;


        // 렌더링 결과를 Texture2D로 가져왔으므로 이제 _miniMapTexture를 사용하여 미니맵을 업데이트합니다.
        _UpdateMiniMapTexture();




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
