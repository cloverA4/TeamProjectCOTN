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
    private Color[] visitedColorData; // �ʱ��� �÷�������
    private Color[] currentColorData; // ������ �÷�������(�÷��̾� ��ġ)

    private Vector3[] initialColorDataPo; // Ž�� ���� �迭 �ʱ�ȭ

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

        // �ؽ�ó�� �ʱ� ���¸� ���� ���������� �����Ͽ� Ž������ ���� ������ �Ȱ��� ��Ÿ���ϴ�.
        visitedColorData = new Color[textureWidth * textureHeight];
        currentColorData = new Color[textureWidth * textureHeight];

        // Ž�� ���� �迭 �ʱ�ȭ
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



    public Texture2D MakeFogOfWarTexture() //Ž���ϰ� �ִ� ����
    {
        fogOfWarTexture = new Texture2D(textureWidth, textureHeight);
        fogOfWarTexture.filterMode = FilterMode.Point;

        return fogOfWarTexture;
    }

    public Texture2D MakeFogOfWarTexture2() // Ž���� ����
    {
        fogOfWarTexture = new Texture2D(textureWidth, textureHeight);
        fogOfWarTexture.filterMode = FilterMode.Point;

        return fogOfWarTexture;
    }

    public void Stage1F1UpdateFogOfWar()
    {
        ////Rect�� ���� ��
        //transform.position = new Vector3(stage1F1FogPosition.position.x + textureWidth / 2,
        //                     stage1F1FogPosition.position.y - 1 + textureHeight / 2, 0);
        ////Ʈ������ �������� ����
        //int x = (int)_Player.transform.position.x - (int)stage1F1FogPosition.position.x;
        //int y = (int)_Player.transform.position.y - (int)stage1F1FogPosition.position.y;


        //int index = y * textureWidth + x;
        //asd(index, 0);                      //ĳ���� �߾�

        //asd(index, -1);                     //ĳ���� ����+1
        //asd(index, -2);                     //ĳ���� ����+2
        //asd(index, -3);                     //ĳ���� ����+3
        ////asd(index - 2);
        //asd(index, +1);                     //ĳ���� ������+1
        //asd(index, +2);                     //ĳ���� ������+2
        //asd(index, +3);                     //ĳ���� ������+3

        //asd(index, +textureWidth);          //ĳ���� ����+1
        //asd(index, +textureWidth * 2);      //ĳ���� ����+2
        //asd(index, +textureWidth * 3);      //ĳ���� ����+3

        //asd(index, -textureWidth);          //ĳ���� �Ʒ���+1
        //asd(index, -textureWidth * 2);      //ĳ���� �Ʒ���+2
        //asd(index, -textureWidth * 3);      //ĳ���� �Ʒ���+3

        //asd(index, +textureWidth + 1);      //ĳ���� ��������
        //asd(index, +textureWidth * 2 + 1);  //ĳ���� ����������
        //asd(index, +textureWidth + 2);      //ĳ���� �������ʿ�����

        //asd(index, +textureWidth - 1);      //ĳ���� ������
        //asd(index, +textureWidth * 2 - 1);  //ĳ���� ��������
        //asd(index, +textureWidth - 2);      //ĳ���� �����ʿ���


        //asd(index, -textureWidth - 1);      //ĳ���� �Ʒ�����
        //asd(index, -textureWidth * 2 - 1);  //ĳ���� �Ʒ��Ʒ�����
        //asd(index, -textureWidth - 2);      //ĳ���� �Ʒ����ʿ���


        //asd(index, -textureWidth + 1);      //ĳ���� �Ʒ�������
        //asd(index, -textureWidth * 2 + 1);  //ĳ���� �Ʒ��Ʒ�������
        //asd(index, -textureWidth + 2);      //ĳ���� �Ʒ������ʿ�����



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
        {   //new Color(0, 0, 0, 1) ���� new Color(0, 0, 0, 0) ���� new Color(0, 0, 0, 0) ������
            if (visitedColorData[i] == new Color(0, 0, 0, 0) && currentColorData[i] == new Color(0, 0, 0, 1) )  
                lastcolorData[i] = new Color(0, 0, 0, 0.5f);
            // �湮�Ѱ��� ���������� ���� ���� �̸� ������
            if (visitedColorData[i] == new Color(0, 0, 0, 1) && currentColorData[i] == new Color(0, 0, 0, 1)) 
                lastcolorData[i] = new Color(0, 0, 0, 1f);
            // �湮�Ѱ��� ���������� �����̸� ����
            if (visitedColorData[i] == new Color(0, 0, 0, 0) && currentColorData[i] == new Color(0, 0, 0, 0)) 
                lastcolorData[i] = new Color(0, 0, 0, 0.0f);
            // �湮�Ѱ��� ���������� ���� ���� �̸� ����

            
            //����
            //�Ͼ�
            //������


        }

        // ���� ������ �Ȱ� �ؽ�ó�� ����
        fogOfWarTexture.SetPixels(lastcolorData);
        
        fogOfWarTexture.Apply();
    }

    public void Stage1F2UpdateFogOfWar()
    {
        //Rect�� ���� ��
        transform.position = new Vector3(stage1F2FogPosition.position.x + textureWidth / 2,
                             stage1F2FogPosition.position.y - 1 + textureHeight / 2, 0);
        //Ʈ������ �������� ����
        int x = (int)_Player.transform.position.x - (int)stage1F2FogPosition.position.x;
        int y = (int)_Player.transform.position.y - (int)stage1F2FogPosition.position.y;


        int index = y * textureWidth + x;
        asd(index, 0);                      //ĳ���� �߾�

        asd(index, -1);                     //ĳ���� ����+1
        asd(index, -2);                     //ĳ���� ����+2
        asd(index, -3);                     //ĳ���� ����+3
        //asd(index - 2);
        asd(index, +1);                     //ĳ���� ������+1
        asd(index, +2);                     //ĳ���� ������+2
        asd(index, +3);                     //ĳ���� ������+3

        asd(index, +textureWidth);          //ĳ���� ����+1
        asd(index, +textureWidth * 2);      //ĳ���� ����+2
        asd(index, +textureWidth * 3);      //ĳ���� ����+3

        asd(index, -textureWidth);          //ĳ���� �Ʒ���+1
        asd(index, -textureWidth * 2);      //ĳ���� �Ʒ���+2
        asd(index, -textureWidth * 3);      //ĳ���� �Ʒ���+3

        asd(index, +textureWidth + 1);      //ĳ���� ��������
        asd(index, +textureWidth * 2 + 1);  //ĳ���� ����������
        asd(index, +textureWidth + 2);      //ĳ���� �������ʿ�����

        asd(index, +textureWidth - 1);      //ĳ���� ������
        asd(index, +textureWidth * 2 - 1);  //ĳ���� ��������
        asd(index, +textureWidth - 2);      //ĳ���� �����ʿ���


        asd(index, -textureWidth - 1);      //ĳ���� �Ʒ�����
        asd(index, -textureWidth * 2 - 1);  //ĳ���� �Ʒ��Ʒ�����
        asd(index, -textureWidth - 2);      //ĳ���� �Ʒ����ʿ���


        asd(index, -textureWidth + 1);      //ĳ���� �Ʒ�������
        asd(index, -textureWidth * 2 + 1);  //ĳ���� �Ʒ��Ʒ�������
        asd(index, -textureWidth + 2);      //ĳ���� �Ʒ������ʿ�����


        // ���� ������ �Ȱ� �ؽ�ó�� ����
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
        currentColorData[x] = Color.clear;  //�����°��� 0�Ǿ���
    }

    public int PlayerAround()
    {
        //Rect�� ���� ��
        transform.position = new Vector3(stage1F1FogPosition.position.x + textureWidth / 2,
                             stage1F1FogPosition.position.y - 1 + textureHeight / 2, 0);
        //Ʈ������ �������� ����
        int x = (int)_Player.transform.position.x - (int)stage1F1FogPosition.position.x;
        int y = (int)_Player.transform.position.y - (int)stage1F1FogPosition.position.y;


        int index = y * textureWidth + x;

        asd(index, 0);                      //ĳ���� �߾�

        asd(index, -1);                     //ĳ���� ����+1
        asd(index, -2);                     //ĳ���� ����+2
        asd(index, -3);                     //ĳ���� ����+3
        //asd(index - 2);
        asd(index, +1);                     //ĳ���� ������+1
        asd(index, +2);                     //ĳ���� ������+2
        asd(index, +3);                     //ĳ���� ������+3

        asd(index, +textureWidth);          //ĳ���� ����+1
        asd(index, +textureWidth * 2);      //ĳ���� ����+2
        asd(index, +textureWidth * 3);      //ĳ���� ����+3

        asd(index, -textureWidth);          //ĳ���� �Ʒ���+1
        asd(index, -textureWidth * 2);      //ĳ���� �Ʒ���+2
        asd(index, -textureWidth * 3);      //ĳ���� �Ʒ���+3

        asd(index, +textureWidth + 1);      //ĳ���� ��������
        asd(index, +textureWidth * 2 + 1);  //ĳ���� ����������
        asd(index, +textureWidth + 2);      //ĳ���� �������ʿ�����

        asd(index, +textureWidth - 1);      //ĳ���� ������
        asd(index, +textureWidth * 2 - 1);  //ĳ���� ��������
        asd(index, +textureWidth - 2);      //ĳ���� �����ʿ���


        asd(index, -textureWidth - 1);      //ĳ���� �Ʒ�����
        asd(index, -textureWidth * 2 - 1);  //ĳ���� �Ʒ��Ʒ�����
        asd(index, -textureWidth - 2);      //ĳ���� �Ʒ����ʿ���


        asd(index, -textureWidth + 1);      //ĳ���� �Ʒ�������
        asd(index, -textureWidth * 2 + 1);  //ĳ���� �Ʒ��Ʒ�������
        asd(index, -textureWidth + 2);      //ĳ���� �Ʒ������ʿ�����

        return index;
    }
}
