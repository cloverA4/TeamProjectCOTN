using UnityEngine;

public class MakeFog2 : MonoBehaviour
{
    private int textureWidth = 40;
    private int textureHeight = 40;

    private Texture2D fogOfWarTexture;
    private Color[] visitedColorData; // �ʱ��� �÷�������
    private Color[] currentColorData; // ������ �÷�������(�÷��̾� ��ġ)

    [SerializeField]
    private Transform stage1F1FogPosition;
    [SerializeField]
    private Transform stage1F2FogPosition;
    [SerializeField]
    private Transform stage1F3FogPosition;

    GameObject _Player;

    private void Awake()
    {
        _Player = PlayerController.Instance.gameObject;
    }

    public void ResetFog()
    {
        transform.localScale = new Vector3(textureWidth, textureHeight, 0);
        // Create the Fog of War texture

        fogOfWarTexture = new Texture2D(textureWidth, textureHeight);
        fogOfWarTexture.filterMode = FilterMode.Point;

        // �ؽ�ó�� �ʱ� ���¸� ���� ���������� �����Ͽ� Ž������ ���� ������ �Ȱ��� ��Ÿ���ϴ�.
        visitedColorData = new Color[textureWidth * textureHeight];
        currentColorData = new Color[textureWidth * textureHeight];

        int index = 0;

        for (int x = 0; x < textureHeight; x++)
        {
            for (int y = 0; y < textureWidth; y++)
            {
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

    public void FogOfWarStageMove()
    {
        ResetFog();
        UpdateFogOfWar();
    }

    public void UpdateFogOfWar() // �Ȱ� ������Ʈ ����
    {
        if ((GameManager.Instance.NowStage == Stage.Lobby) && (GameManager.Instance.NowFloor == floor.f1)) 
        {
            return;
        } 
        //����ó�� �κ� �������� �Ȱ� ������Ʈ x

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
        }

        // ���� ������ �Ȱ� �ؽ�ó�� ����
        fogOfWarTexture.SetPixels(lastcolorData);
        
        fogOfWarTexture.Apply();
    }

    public int PlayerAround() 
    {
        
        if ((GameManager.Instance.NowStage == Stage.Stage1) && (GameManager.Instance.NowFloor == floor.f1)) 
        {
            //Rect�� ���� ��
            transform.position = new Vector3(stage1F1FogPosition.position.x + textureWidth / 2, stage1F1FogPosition.position.y-1 + textureHeight / 2, 0);
            //Ʈ������ �������� ����
            int x = (int)_Player.transform.position.x - (int)stage1F1FogPosition.position.x;
            int y = (int)_Player.transform.position.y - (int)stage1F1FogPosition.position.y;
            int index = y * textureWidth + x;

            PlayerPosition(index, 0);                      //ĳ���� �߾�

            PlayerPosition(index, -1);                     //ĳ���� ����+1
            PlayerPosition(index, -2);                     //ĳ���� ����+2
            PlayerPosition(index, -3);                     //ĳ���� ����+3
                                                           //asd(index - 2);
            PlayerPosition(index, +1);                     //ĳ���� ������+1
            PlayerPosition(index, +2);                     //ĳ���� ������+2
            PlayerPosition(index, +3);                     //ĳ���� ������+3

            PlayerPosition(index, +textureWidth);          //ĳ���� ����+1
            PlayerPosition(index, +textureWidth * 2);      //ĳ���� ����+2
            PlayerPosition(index, +textureWidth * 3);      //ĳ���� ����+3

            PlayerPosition(index, -textureWidth);          //ĳ���� �Ʒ���+1
            PlayerPosition(index, -textureWidth * 2);      //ĳ���� �Ʒ���+2
            PlayerPosition(index, -textureWidth * 3);      //ĳ���� �Ʒ���+3

            PlayerPosition(index, +textureWidth + 1);      //ĳ���� ��������
            PlayerPosition(index, +textureWidth * 2 + 1);  //ĳ���� ����������
            PlayerPosition(index, +textureWidth + 2);      //ĳ���� �������ʿ�����

            PlayerPosition(index, +textureWidth - 1);      //ĳ���� ������
            PlayerPosition(index, +textureWidth * 2 - 1);  //ĳ���� ��������
            PlayerPosition(index, +textureWidth - 2);      //ĳ���� �����ʿ���


            PlayerPosition(index, -textureWidth - 1);      //ĳ���� �Ʒ�����
            PlayerPosition(index, -textureWidth * 2 - 1);  //ĳ���� �Ʒ��Ʒ�����
            PlayerPosition(index, -textureWidth - 2);      //ĳ���� �Ʒ����ʿ���


            PlayerPosition(index, -textureWidth + 1);      //ĳ���� �Ʒ�������
            PlayerPosition(index, -textureWidth * 2 + 1);  //ĳ���� �Ʒ��Ʒ�������
            PlayerPosition(index, -textureWidth + 2);      //ĳ���� �Ʒ������ʿ�����

            return index;
        } //�������� 1�� 1��

        if ((GameManager.Instance.NowStage == Stage.Stage1) && (GameManager.Instance.NowFloor == floor.f2)) 
        {
            //Rect�� ���� ��
            transform.position = new Vector3(stage1F2FogPosition.position.x - 1 + textureWidth / 2,
                                 stage1F2FogPosition.position.y - 1 + textureHeight / 2, 0);
            //Ʈ������ �������� ����
            int x = (int)_Player.transform.position.x - (int)stage1F2FogPosition.position.x;
            int y = (int)_Player.transform.position.y - (int)stage1F2FogPosition.position.y;
            int index = y * textureWidth + x;

            PlayerPosition(index, 0);                      //ĳ���� �߾�

            PlayerPosition(index, -1);                     //ĳ���� ����+1
            PlayerPosition(index, -2);                     //ĳ���� ����+2
            PlayerPosition(index, -3);                     //ĳ���� ����+3
                                                           //asd(index - 2);
            PlayerPosition(index, +1);                     //ĳ���� ������+1
            PlayerPosition(index, +2);                     //ĳ���� ������+2
            PlayerPosition(index, +3);                     //ĳ���� ������+3

            PlayerPosition(index, +textureWidth);          //ĳ���� ����+1
            PlayerPosition(index, +textureWidth * 2);      //ĳ���� ����+2
            PlayerPosition(index, +textureWidth * 3);      //ĳ���� ����+3

            PlayerPosition(index, -textureWidth);          //ĳ���� �Ʒ���+1
            PlayerPosition(index, -textureWidth * 2);      //ĳ���� �Ʒ���+2
            PlayerPosition(index, -textureWidth * 3);      //ĳ���� �Ʒ���+3

            PlayerPosition(index, +textureWidth + 1);      //ĳ���� ��������
            PlayerPosition(index, +textureWidth * 2 + 1);  //ĳ���� ����������
            PlayerPosition(index, +textureWidth + 2);      //ĳ���� �������ʿ�����

            PlayerPosition(index, +textureWidth - 1);      //ĳ���� ������
            PlayerPosition(index, +textureWidth * 2 - 1);  //ĳ���� ��������
            PlayerPosition(index, +textureWidth - 2);      //ĳ���� �����ʿ���


            PlayerPosition(index, -textureWidth - 1);      //ĳ���� �Ʒ�����
            PlayerPosition(index, -textureWidth * 2 - 1);  //ĳ���� �Ʒ��Ʒ�����
            PlayerPosition(index, -textureWidth - 2);      //ĳ���� �Ʒ����ʿ���


            PlayerPosition(index, -textureWidth + 1);      //ĳ���� �Ʒ�������
            PlayerPosition(index, -textureWidth * 2 + 1);  //ĳ���� �Ʒ��Ʒ�������
            PlayerPosition(index, -textureWidth + 2);      //ĳ���� �Ʒ������ʿ�����

            return index;
        } //�������� 1�� 2��

        if ((GameManager.Instance.NowStage == Stage.Stage1) && (GameManager.Instance.NowFloor == floor.f3)) 
        {
            //Rect�� ���� ��
            transform.position = new Vector3(stage1F3FogPosition.position.x - 1 + textureWidth / 2,
                                 stage1F3FogPosition.position.y - 1 + textureHeight / 2, 0);
            //Ʈ������ �������� ����
            int x = (int)_Player.transform.position.x - (int)stage1F3FogPosition.position.x;
            int y = (int)_Player.transform.position.y - (int)stage1F3FogPosition.position.y;
            int index = y * textureWidth + x;

            PlayerPosition(index, 0);                      //ĳ���� �߾�

            PlayerPosition(index, -1);                     //ĳ���� ����+1
            PlayerPosition(index, -2);                     //ĳ���� ����+2
            PlayerPosition(index, -3);                     //ĳ���� ����+3
                                                           //asd(index - 2);
            PlayerPosition(index, +1);                     //ĳ���� ������+1
            PlayerPosition(index, +2);                     //ĳ���� ������+2
            PlayerPosition(index, +3);                     //ĳ���� ������+3

            PlayerPosition(index, +textureWidth);          //ĳ���� ����+1
            PlayerPosition(index, +textureWidth * 2);      //ĳ���� ����+2
            PlayerPosition(index, +textureWidth * 3);      //ĳ���� ����+3

            PlayerPosition(index, -textureWidth);          //ĳ���� �Ʒ���+1
            PlayerPosition(index, -textureWidth * 2);      //ĳ���� �Ʒ���+2
            PlayerPosition(index, -textureWidth * 3);      //ĳ���� �Ʒ���+3

            PlayerPosition(index, +textureWidth + 1);      //ĳ���� ��������
            PlayerPosition(index, +textureWidth * 2 + 1);  //ĳ���� ����������
            PlayerPosition(index, +textureWidth + 2);      //ĳ���� �������ʿ�����

            PlayerPosition(index, +textureWidth - 1);      //ĳ���� ������
            PlayerPosition(index, +textureWidth * 2 - 1);  //ĳ���� ��������
            PlayerPosition(index, +textureWidth - 2);      //ĳ���� �����ʿ���


            PlayerPosition(index, -textureWidth - 1);      //ĳ���� �Ʒ�����
            PlayerPosition(index, -textureWidth * 2 - 1);  //ĳ���� �Ʒ��Ʒ�����
            PlayerPosition(index, -textureWidth - 2);      //ĳ���� �Ʒ����ʿ���


            PlayerPosition(index, -textureWidth + 1);      //ĳ���� �Ʒ�������
            PlayerPosition(index, -textureWidth * 2 + 1);  //ĳ���� �Ʒ��Ʒ�������
            PlayerPosition(index, -textureWidth + 2);      //ĳ���� �Ʒ������ʿ�����

            return index;

        } //�������� 1�� 3��

            return 0;
    }

    void PlayerPosition(int index, int i)
    {
        int x = index + i;

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

}
