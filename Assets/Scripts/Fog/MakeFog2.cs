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
    private Color[] visitedColorData; // 초기의 컬러데이터
    private Color[] currentColorData; // 현재의 컬러데이터(플레이어 위치)

    private Vector3[] initialColorDataPo; // 탐색 영역 배열 초기화

    [SerializeField]
    private Transform stage1F1FogPosition;
    [SerializeField]
    private Transform stage1F2FogPosition;
    [SerializeField]
    private Transform stage1F3FogPosition;

    [SerializeField]
    GameObject _Player;

    public void ResetFog()
    {
        transform.localScale = new Vector3(textureWidth, textureHeight, 0);
        // Create the Fog of War texture
        MakeFogOfWarTexture();
        MakeFogOfWarTexture2();

        // 텍스처의 초기 상태를 완전 불투명으로 설정하여 탐험하지 않은 전장의 안개를 나타냅니다.
        visitedColorData = new Color[textureWidth * textureHeight];
        currentColorData = new Color[textureWidth * textureHeight];

        // 탐색 영역 배열 초기화
        initialColorDataPo = new Vector3[textureWidth * textureHeight];



        int index = 0;

        for (int x = 0; x < textureHeight; x++)
        {
            for (int y = 0; y < textureWidth; y++)
            {
                initialColorDataPo[index] = new Vector3(x, y, 0);
                visitedColorData[index] = Color.black;
                currentColorData[index] = Color.black;
                index++;
            }
        }



        fogOfWarTexture.SetPixels(visitedColorData);
        fogOfWarTexture.Apply();
        GetComponent<Renderer>().material.SetTexture("_BaseMap", fogOfWarTexture);
        GetComponent<Renderer>().sortingOrder = 999;
        //Stage1F1UpdateFogOfWar();
    }



    public Texture2D MakeFogOfWarTexture() //탐색하고 있는 영역
    {
        fogOfWarTexture = new Texture2D(textureWidth, textureHeight);
        fogOfWarTexture.filterMode = FilterMode.Point;

        return fogOfWarTexture;
    }

    public Texture2D MakeFogOfWarTexture2() // 탐색된 영역
    {
        fogOfWarTexture = new Texture2D(textureWidth, textureHeight);
        fogOfWarTexture.filterMode = FilterMode.Point;

        return fogOfWarTexture;
    }

    public void Stage1F1UpdateFogOfWar()
    {
        ////Rect의 값과 비교
        //transform.position = new Vector3(stage1F1FogPosition.position.x + textureWidth / 2,
        //                     stage1F1FogPosition.position.y - 1 + textureHeight / 2, 0);
        ////트랜스폼 포지션의 값비교
        //int x = (int)_Player.transform.position.x - (int)stage1F1FogPosition.position.x;
        //int y = (int)_Player.transform.position.y - (int)stage1F1FogPosition.position.y;


        //int index = y * textureWidth + x;
        //asd(index, 0);                      //캐릭터 중앙

        //asd(index, -1);                     //캐릭터 왼쪽+1
        //asd(index, -2);                     //캐릭터 왼쪽+2
        //asd(index, -3);                     //캐릭터 왼쪽+3
        ////asd(index - 2);
        //asd(index, +1);                     //캐릭터 오른쪽+1
        //asd(index, +2);                     //캐릭터 오른쪽+2
        //asd(index, +3);                     //캐릭터 오른쪽+3

        //asd(index, +textureWidth);          //캐릭터 위쪽+1
        //asd(index, +textureWidth * 2);      //캐릭터 위쪽+2
        //asd(index, +textureWidth * 3);      //캐릭터 위쪽+3

        //asd(index, -textureWidth);          //캐릭터 아래쪽+1
        //asd(index, -textureWidth * 2);      //캐릭터 아래쪽+2
        //asd(index, -textureWidth * 3);      //캐릭터 아래쪽+3

        //asd(index, +textureWidth + 1);      //캐릭터 위오른쪽
        //asd(index, +textureWidth * 2 + 1);  //캐릭터 위위오른쪽
        //asd(index, +textureWidth + 2);      //캐릭터 위오른쪽오른쪽

        //asd(index, +textureWidth - 1);      //캐릭터 위왼쪽
        //asd(index, +textureWidth * 2 - 1);  //캐릭터 위위왼쪽
        //asd(index, +textureWidth - 2);      //캐릭터 위왼쪽왼쪽


        //asd(index, -textureWidth - 1);      //캐릭터 아래왼쪽
        //asd(index, -textureWidth * 2 - 1);  //캐릭터 아래아래왼쪽
        //asd(index, -textureWidth - 2);      //캐릭터 아래왼쪽왼쪽


        //asd(index, -textureWidth + 1);      //캐릭터 아래오른쪽
        //asd(index, -textureWidth * 2 + 1);  //캐릭터 아래아래오른쪽
        //asd(index, -textureWidth + 2);      //캐릭터 아래오른쪽오른쪽



        Color[] lastcolorData = visitedColorData;

        for (int i = 0; i < currentColorData.Length; i++)
        {
            if (i == PlayerAround())
            {
                currentColorData[i] = new Color(0, 0, 0, 0);
            }
            else
            {
                currentColorData[i] = new Color(0, 0, 0, 1);
            }
        }

        for (int i = 0; i < visitedColorData.Length; i++)
        {   //new Color(0, 0, 0, 1) 검정 new Color(0, 0, 0, 0) 투명 new Color(0, 0, 0, 0) 반투명
            if (visitedColorData[i] == new Color(0, 0, 0, 0) && currentColorData[i] == new Color(0, 0, 0, 1) )  
                lastcolorData[i] = new Color(0, 0, 0, 0.5f);
            // 방문한곳과 현재정보가 검정 투명 이면 반투명
            if (visitedColorData[i] == new Color(0, 0, 0, 1) && currentColorData[i] == new Color(0, 0, 0, 1)) 
                lastcolorData[i] = new Color(0, 0, 0, 1f);
            // 방문한곳과 현재정보가 검정이면 검정
            if (visitedColorData[i] == new Color(0, 0, 0, 0) && currentColorData[i] == new Color(0, 0, 0, 0)) 
                lastcolorData[i] = new Color(0, 0, 0, 0.0f);
            // 방문한곳과 현재정보가 투명 투명 이면 투명

            
            //검정
            //하양
            //반투명


        }

        // 변경 사항을 안개 텍스처에 적용
        fogOfWarTexture.SetPixels(lastcolorData);
        
        fogOfWarTexture.Apply();
    }

    public void Stage1F2UpdateFogOfWar()
    {
        //Rect의 값과 비교
        transform.position = new Vector3(stage1F2FogPosition.position.x + textureWidth / 2,
                             stage1F2FogPosition.position.y - 1 + textureHeight / 2, 0);
        //트랜스폼 포지션의 값비교
        int x = (int)_Player.transform.position.x - (int)stage1F2FogPosition.position.x;
        int y = (int)_Player.transform.position.y - (int)stage1F2FogPosition.position.y;


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
        //fogOfWarTexture.SetPixels(visitedColorData);
        fogOfWarTexture.SetPixels(currentColorData);

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

        if (x >= visitedColorData.Length || x < 0)
        {
            return;
        }

        if (x >= currentColorData.Length || x < 0)
        {
            return;
        }
        visitedColorData[x] = Color.clear;  //new Color(0, 0, 0, 0f);
        currentColorData[x] = Color.clear;  //지나온곳은 0되야함
    }

    public int PlayerAround()
    {
        //Rect의 값과 비교
        transform.position = new Vector3(stage1F1FogPosition.position.x + textureWidth / 2,
                             stage1F1FogPosition.position.y - 1 + textureHeight / 2, 0);
        //트랜스폼 포지션의 값비교
        int x = (int)_Player.transform.position.x - (int)stage1F1FogPosition.position.x;
        int y = (int)_Player.transform.position.y - (int)stage1F1FogPosition.position.y;


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

        return index;
    }
}
