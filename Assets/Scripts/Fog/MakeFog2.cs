using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

public class MakeFog2 : MonoBehaviour
{
    private int textureWidth = 40;
    private int textureHeight = 40;

    private Texture2D fogOfWarTexture;
    private Color[] initialColorData; // 초기의 컬러데이터
    private Color[] currentColorData; //현재의 컬러데이터(플레이어 위치)
    //    private bool[,] exploredAreas; //탐색된영역

    private Vector3[] initialColorDataPo; // 탐색 영역 배열 초기화

    [SerializeField]
    private Transform stage1FogPosition;

    [SerializeField]
    GameObject _Player;





    void Start()
    {
        transform.localScale = new Vector3(textureWidth, textureHeight, 0);
        // Create the Fog of War texture
        MakeFogOfWarTexture();
        MakeFogOfWarTexture2();
        //Sprites();

        // Initialize explored areas array
        // 탐색 영역 배열 초기화
        // exploredAreas = new bool[textureWidth, textureHeight];

        // Set the initial state of the texture to fully opaque, representing unexplored fog of war
        // 텍스처의 초기 상태를 완전 불투명으로 설정하여 탐험하지 않은 전장의 안개를 나타냅니다.
        initialColorData = new Color[textureWidth * textureHeight];
        currentColorData = new Color[textureWidth * textureHeight];

        // 탐색 영역 배열 초기화
        initialColorDataPo = new Vector3[textureWidth * textureHeight];



        int index = 0;

        for (int x = 0; x < textureHeight; x++)
        {
            for (int y = 0; y < textureWidth; y++)
            {
                initialColorDataPo[index] = new Vector3(x, y, 0);
                initialColorData[index] = Color.black;
                currentColorData[index] = Color.black;
                index++;
            }
        }



        fogOfWarTexture.SetPixels(initialColorData);
        fogOfWarTexture.Apply();
        GetComponent<Renderer>().material.SetTexture("_BaseMap", fogOfWarTexture);
        GetComponent<Renderer>().sortingOrder = 999;
        UpdateFogOfWar();
        // Use the fogOfWarTexture as needed (e.g., assign it to a material)
    }



    public Texture2D MakeFogOfWarTexture() //탐색된영역
    {
        fogOfWarTexture = new Texture2D(textureWidth, textureHeight);

        fogOfWarTexture.filterMode = FilterMode.Point;

        return fogOfWarTexture;
    }

    public Texture2D MakeFogOfWarTexture2() //
    {
        fogOfWarTexture = new Texture2D(textureWidth, textureHeight);
        fogOfWarTexture.filterMode = FilterMode.Point;

        return fogOfWarTexture;
    }

    //public void Sprites()
    //{
    //    Sprite sprite = Sprite.Create(fogOfWarTexture, new Rect(0, 0, textureWidth, textureHeight), new Vector2(0.5f, 0.5f));
    //    GetComponent<SpriteRenderer>().sprite = sprite;
    //}

    //public void FogAndPlayerPosition()
    //{
    //    initialColorDataPo[];
    //}

    public void UpdateFogOfWar()
    {
        // 백터위치를 잡고 9개를 받아서 색깔변화
        // 안개 영역을 반복하고 탐색된 영역과 안개 텍스처를 업데이트합니다.
        //for (int x = minX; x <= maxX; x++)
        //{
        //    for (int y = minY; y <= maxY; y++)
        //    {
        //        //if (!exploredAreas[x, y])
        //        //{
        //        //    //해당 영역을 탐색한 것으로 표시하고 안개 텍스처를 업데이트합니다.
        //        //    exploredAreas[x, y] = true;
        //        //    int index = y * 40 + x;

        //        //    initialColorDataPo[index] = new Vector3(0, 0, 0);

        //        //    initialColorData[index] = new Color(0, 0, 0, 0.5f);
        //        //    currentColorData[index] = Color.clear;
        //        //}


        //        initialColorData[index] = new Color(0, 0, 0, 0f);
        //        currentColorData[index] = Color.clear;
        //    }
        //}


        //Rect의 값과 비교
        transform.position = new Vector3(stage1FogPosition.position.x + textureWidth / 2, stage1FogPosition.position.y - 1 + textureHeight / 2, 0);
        //트랜스폼 포지션의 값비교
        int x = (int)_Player.transform.position.x - (int)stage1FogPosition.position.x;
        int y = (int)_Player.transform.position.y - (int)stage1FogPosition.position.y;


        int index = y * textureWidth + x;
        asd(index, 0);                      //캐릭터 중앙

        asd(index, -1);                     //캐릭터 왼쪽+1
        asd(index, -2);                     //캐릭터 왼쪽+2
        asd(index, -3);                     //캐릭터 왼쪽+3
        //asd(index - 2);
        asd(index, +1);                     //캐릭터 오른쪽+1
        asd(index, +2);                     //캐릭터 오른쪽+2
        asd(index, +3);                     //캐릭터 오른쪽+3

        asd(index, +textureWidth);          //캐릭터 위쪽+1
        asd(index, +textureWidth * 2);      //캐릭터 위쪽+2
        asd(index, +textureWidth * 3);      //캐릭터 위쪽+3

        asd(index, -textureWidth);          //캐릭터 아래쪽+1
        asd(index, -textureWidth * 2);      //캐릭터 아래쪽+2
        asd(index, -textureWidth * 3);      //캐릭터 아래쪽+3

        asd(index, +textureWidth + 1);      //캐릭터 위오른쪽
        asd(index, +textureWidth * 2 + 1);  //캐릭터 위위오른쪽
        asd(index, +textureWidth + 2);      //캐릭터 위오른쪽오른쪽

        asd(index, +textureWidth - 1);      //캐릭터 위왼쪽
        asd(index, +textureWidth * 2 - 1);  //캐릭터 위위왼쪽
        asd(index, +textureWidth - 2);      //캐릭터 위왼쪽왼쪽


        asd(index, -textureWidth - 1);      //캐릭터 아래왼쪽
        asd(index, -textureWidth * 2 - 1);  //캐릭터 아래아래왼쪽
        asd(index, -textureWidth - 2);      //캐릭터 아래왼쪽왼쪽


        asd(index, -textureWidth + 1);      //캐릭터 아래오른쪽
        asd(index, -textureWidth * 2 + 1);  //캐릭터 아래아래오른쪽
        asd(index, -textureWidth + 2);      //캐릭터 아래오른쪽오른쪽


        // 변경 사항을 안개 텍스처에 적용
        fogOfWarTexture.SetPixels(currentColorData);
        fogOfWarTexture.SetPixels(initialColorData);
        fogOfWarTexture.Apply();
        //Sprites();
    }


    void asd(int index, int i)
    {
        int x = index + i;

        if (x >= initialColorDataPo.Length || x < 0)
        {
            return;
        }

        initialColorData[x] = new Color(0, 0, 0, 0f);
        currentColorData[x] = Color.clear;
    }
   
}
