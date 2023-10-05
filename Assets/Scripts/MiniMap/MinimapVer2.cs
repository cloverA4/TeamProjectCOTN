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

    [SerializeField] RawImage _miniMapImage; // UI RawImage ���
    [SerializeField] Camera _miniMapCamera; // �̴ϸ� ī�޶�
    private int _textureWidth = 40; // �ؽ�ó �ʺ�
    private int _textureHeight = 40; // �ؽ�ó ����
    private float _explorationDistance = 5.0f; // Ž�� �Ÿ�

    private Texture2D _miniMapTexture; // �̴ϸ� �ؽ�ó
    private Transform _PlayerPos;

    private Color[] visitedColorData; // �ʱ��� �÷�������

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

                // ���� �ȼ��� �÷��̾��� �Ÿ� ���
                Vector3 pixelWorldPosition = _miniMapCamera.ViewportToWorldPoint(new Vector3(x / (float)_textureWidth, y / (float)_textureHeight, 0));
                float distanceToPlayer = Vector3.Distance(pixelWorldPosition, playerPosition);

                // �Ÿ��� Ž�� �Ÿ� �̳��� ��� ���İ��� 1��, �׷��� ������ ���İ��� 0���� ����
                if (distanceToPlayer <= _explorationDistance)
                {
                    pixelColor.a = 1f; // ���İ��� 1�� �����Ͽ� Ž���� �������� ǥ��
                }
                else
                {
                    pixelColor.a = 0f; // ���İ��� 0���� �����Ͽ� Ž������ ���� �������� ǥ��
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

        _miniMapImage.texture = _miniMapTexture; // UI RawImage�� �ؽ�ó �Ҵ�

        RenderTexture renderTexture = new RenderTexture(_textureWidth, _textureHeight, 24);

        _miniMapCamera.targetTexture = renderTexture;
        _miniMapCamera.Render();

        // RenderTexture�� Texture2D�� ����
        RenderTexture.active = renderTexture;
        _miniMapTexture.ReadPixels(new Rect(0, 0, _textureWidth, _textureHeight), 0, 0);
        _miniMapTexture.Apply();
        RenderTexture.active = null;


        // ������ ����� Texture2D�� ���������Ƿ� ���� _miniMapTexture�� ����Ͽ� �̴ϸ��� ������Ʈ�մϴ�.
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
