
using UnityEngine;
using UnityEngine.UI;


public class MiniMapCamera : MonoBehaviour
{


   


 


    public RawImage minimapImage; // 미니맵 UI Raw Image

    public GameObject[] enemies; // 적 게임 오브젝트 배열

    public GameObject[] walls; // 벽 게임 오브젝트 배열
    public GameObject[] bedrocks; // 배드락 게임 오브젝트 배열
    public GameObject[] treasureBoxes; // 보물 상자 게임 오브젝트 배열

    public GameObject[] items; // 아이템 게임 오브젝트 배열
    public GameObject[] stairs; // 계단 게임 오브젝트 배열

    public GameObject[] npcs; // NPC 게임 오브젝트 배열

    //============================================================================

    private int textureWidth = 40;
    private int textureHeight = 40;

    private Texture2D MinimapTexture;
    private Color[] visitedColorData; // 초기의 컬러데이터
    private Color[] currentColorData; // 현재의 컬러데이터(플레이어 위치)

    [SerializeField]
    private Transform stage1F1FogPosition;
    [SerializeField]
    private Transform stage1F2FogPosition;
    [SerializeField]
    private Transform stage1F3FogPosition;



    void MakeMinimap()
    {
        transform.localScale = new Vector3(textureWidth, textureHeight, 0);
        // Create the Minimap texture

        MinimapTexture = new Texture2D(textureWidth, textureHeight);
        MinimapTexture.filterMode = FilterMode.Point;

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

        MinimapTexture.SetPixels(visitedColorData);
        MinimapTexture.Apply();


    }


    private void LateUpdate()
    {
        transform.position = PlayerController.Instance.transform.position + new Vector3(0, 0, -10);

        // 플레이어 위치를 미니맵 UI Raw Image 위 좌표로 변환
        Vector2 playerPositionOnMinimap = WorldToMinimapPosition(PlayerController.Instance.transform.position);

        // 플레이어 아이콘 위치를 업데이트 (파랑색)
        UpdateMinimapIcon(playerPositionOnMinimap, Color.blue);

        // 적들의 위치를 미니맵 UI Raw Image 위 좌표로 변환하여 아이콘 위치 업데이트 (빨강색)
        foreach (GameObject enemy in enemies)
        {
            Vector2 enemyPositionOnMinimap = WorldToMinimapPosition(enemy.transform.position);
            UpdateMinimapIcon(enemyPositionOnMinimap, Color.red);
        }

        // 벽들의 위치를 미니맵 UI Raw Image 위 좌표로 변환하여 아이콘 위치 업데이트 (갈색)
        foreach (GameObject wall in walls)
        {
            Vector2 wallPositionOnMinimap = WorldToMinimapPosition(wall.transform.position);
            UpdateMinimapIcon(wallPositionOnMinimap, new Color(0.6f, 0.4f, 0.2f));
        }

        // 배드락의 위치를 미니맵 UI Raw Image 위 좌표로 변환하여 아이콘 위치 업데이트 (회색)
        foreach (GameObject bedrock in bedrocks)
        {
            Vector2 bedrockPositionOnMinimap = WorldToMinimapPosition(bedrock.transform.position);
            UpdateMinimapIcon(bedrockPositionOnMinimap, Color.gray);
        }

        // 보물 상자의 위치를 미니맵 UI Raw Image 위 좌표로 변환하여 아이콘 위치 업데이트 (주황색)
        foreach (GameObject treasureBox in treasureBoxes)
        {
            Vector2 boxPositionOnMinimap = WorldToMinimapPosition(treasureBox.transform.position);
            UpdateMinimapIcon(boxPositionOnMinimap, new Color(1.0f, 0.5f, 0.0f)); // 주황색
        }

        // 아이템의 위치를 미니맵 UI Raw Image 위 좌표로 변환하여 아이콘 위치 업데이트 (노랑색)
        foreach (GameObject item in items)
        {
            Vector2 itemPositionOnMinimap = WorldToMinimapPosition(item.transform.position);
            UpdateMinimapIcon(itemPositionOnMinimap, Color.yellow);
        }

        // 계단의 위치를 미니맵 UI Raw Image 위 좌표로 변환하여 아이콘 위치 업데이트 (보라색)
        foreach (GameObject stair in stairs)
        {
            Vector2 stairPositionOnMinimap = WorldToMinimapPosition(stair.transform.position);
            UpdateMinimapIcon(stairPositionOnMinimap, Color.magenta);
        }

        // NPC의 위치를 미니맵 UI Raw Image 위 좌표로 변환하여 아이콘 위치 업데이트 (초록색)
        foreach (GameObject npc in npcs)
        {
            Vector2 npcPositionOnMinimap = WorldToMinimapPosition(npc.transform.position);
            UpdateMinimapIcon(npcPositionOnMinimap, Color.green);
        }


    }

    // 세계 좌표를 미니맵 UI Raw Image 위 좌표로 변환
    private Vector2 WorldToMinimapPosition(Vector3 worldPosition)
    {
        // 여기에서 변환식을 작성하세요. (미니맵 이미지와 게임 월드의 크기 및 스케일을 고려)
        // 예시: 미니맵은 게임 월드의 1/10 크기로 가정
        return new Vector2(worldPosition.x / 10, worldPosition.z / 10);
    }

    // 미니맵 아이콘 위치 업데이트
    private void UpdateMinimapIcon(Vector2 position, Color iconColor)
    {
        // 미니맵 UI Raw Image에서 아이콘 위치 및 색상을 업데이트
        // 여기에서 UI Raw Image에 아이콘을 그리거나 위치를 업데이트하는 코드를 작성하세요.
        // 예시: 미니맵 UI Raw Image의 자식으로 아이콘 이미지를 가지고 있는 것을 가정
        foreach (Transform child in minimapImage.transform)
        {
            SpriteRenderer iconSpriteRenderer = child.GetComponent<SpriteRenderer>();
            if (iconSpriteRenderer != null)
            {
                iconSpriteRenderer.transform.position = position;
                iconSpriteRenderer.color = iconColor;
            }
        }
    }
}
