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
    private Color[] initialColorData; // �ʱ��� �÷�������
    private Color[] currentColorData; //������ �÷�������(�÷��̾� ��ġ)
    //    private bool[,] exploredAreas; //Ž���ȿ���

    private Vector3[] initialColorDataPo; // Ž�� ���� �迭 �ʱ�ȭ

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
        // Ž�� ���� �迭 �ʱ�ȭ
        // exploredAreas = new bool[textureWidth, textureHeight];

        // Set the initial state of the texture to fully opaque, representing unexplored fog of war
        // �ؽ�ó�� �ʱ� ���¸� ���� ���������� �����Ͽ� Ž������ ���� ������ �Ȱ��� ��Ÿ���ϴ�.
        initialColorData = new Color[textureWidth * textureHeight];
        currentColorData = new Color[textureWidth * textureHeight];

        // Ž�� ���� �迭 �ʱ�ȭ
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



    public Texture2D MakeFogOfWarTexture() //Ž���ȿ���
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
        // ������ġ�� ��� 9���� �޾Ƽ� ����ȭ
        // �Ȱ� ������ �ݺ��ϰ� Ž���� ������ �Ȱ� �ؽ�ó�� ������Ʈ�մϴ�.
        //for (int x = minX; x <= maxX; x++)
        //{
        //    for (int y = minY; y <= maxY; y++)
        //    {
        //        //if (!exploredAreas[x, y])
        //        //{
        //        //    //�ش� ������ Ž���� ������ ǥ���ϰ� �Ȱ� �ؽ�ó�� ������Ʈ�մϴ�.
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


        //Rect�� ���� ��
        transform.position = new Vector3(stage1FogPosition.position.x + textureWidth / 2, stage1FogPosition.position.y - 1 + textureHeight / 2, 0);
        //Ʈ������ �������� ����
        int x = (int)_Player.transform.position.x - (int)stage1FogPosition.position.x;
        int y = (int)_Player.transform.position.y - (int)stage1FogPosition.position.y;


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
