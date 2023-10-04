
using UnityEngine;
using UnityEngine.UI;


public class MiniMapCamera : MonoBehaviour
{


   


 


    public RawImage minimapImage; // �̴ϸ� UI Raw Image

    public GameObject[] enemies; // �� ���� ������Ʈ �迭

    public GameObject[] walls; // �� ���� ������Ʈ �迭
    public GameObject[] bedrocks; // ���� ���� ������Ʈ �迭
    public GameObject[] treasureBoxes; // ���� ���� ���� ������Ʈ �迭

    public GameObject[] items; // ������ ���� ������Ʈ �迭
    public GameObject[] stairs; // ��� ���� ������Ʈ �迭

    public GameObject[] npcs; // NPC ���� ������Ʈ �迭

    //============================================================================

    private int textureWidth = 40;
    private int textureHeight = 40;

    private Texture2D MinimapTexture;
    private Color[] visitedColorData; // �ʱ��� �÷�������
    private Color[] currentColorData; // ������ �÷�������(�÷��̾� ��ġ)

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

        // �÷��̾� ��ġ�� �̴ϸ� UI Raw Image �� ��ǥ�� ��ȯ
        Vector2 playerPositionOnMinimap = WorldToMinimapPosition(PlayerController.Instance.transform.position);

        // �÷��̾� ������ ��ġ�� ������Ʈ (�Ķ���)
        UpdateMinimapIcon(playerPositionOnMinimap, Color.blue);

        // ������ ��ġ�� �̴ϸ� UI Raw Image �� ��ǥ�� ��ȯ�Ͽ� ������ ��ġ ������Ʈ (������)
        foreach (GameObject enemy in enemies)
        {
            Vector2 enemyPositionOnMinimap = WorldToMinimapPosition(enemy.transform.position);
            UpdateMinimapIcon(enemyPositionOnMinimap, Color.red);
        }

        // ������ ��ġ�� �̴ϸ� UI Raw Image �� ��ǥ�� ��ȯ�Ͽ� ������ ��ġ ������Ʈ (����)
        foreach (GameObject wall in walls)
        {
            Vector2 wallPositionOnMinimap = WorldToMinimapPosition(wall.transform.position);
            UpdateMinimapIcon(wallPositionOnMinimap, new Color(0.6f, 0.4f, 0.2f));
        }

        // ������ ��ġ�� �̴ϸ� UI Raw Image �� ��ǥ�� ��ȯ�Ͽ� ������ ��ġ ������Ʈ (ȸ��)
        foreach (GameObject bedrock in bedrocks)
        {
            Vector2 bedrockPositionOnMinimap = WorldToMinimapPosition(bedrock.transform.position);
            UpdateMinimapIcon(bedrockPositionOnMinimap, Color.gray);
        }

        // ���� ������ ��ġ�� �̴ϸ� UI Raw Image �� ��ǥ�� ��ȯ�Ͽ� ������ ��ġ ������Ʈ (��Ȳ��)
        foreach (GameObject treasureBox in treasureBoxes)
        {
            Vector2 boxPositionOnMinimap = WorldToMinimapPosition(treasureBox.transform.position);
            UpdateMinimapIcon(boxPositionOnMinimap, new Color(1.0f, 0.5f, 0.0f)); // ��Ȳ��
        }

        // �������� ��ġ�� �̴ϸ� UI Raw Image �� ��ǥ�� ��ȯ�Ͽ� ������ ��ġ ������Ʈ (�����)
        foreach (GameObject item in items)
        {
            Vector2 itemPositionOnMinimap = WorldToMinimapPosition(item.transform.position);
            UpdateMinimapIcon(itemPositionOnMinimap, Color.yellow);
        }

        // ����� ��ġ�� �̴ϸ� UI Raw Image �� ��ǥ�� ��ȯ�Ͽ� ������ ��ġ ������Ʈ (�����)
        foreach (GameObject stair in stairs)
        {
            Vector2 stairPositionOnMinimap = WorldToMinimapPosition(stair.transform.position);
            UpdateMinimapIcon(stairPositionOnMinimap, Color.magenta);
        }

        // NPC�� ��ġ�� �̴ϸ� UI Raw Image �� ��ǥ�� ��ȯ�Ͽ� ������ ��ġ ������Ʈ (�ʷϻ�)
        foreach (GameObject npc in npcs)
        {
            Vector2 npcPositionOnMinimap = WorldToMinimapPosition(npc.transform.position);
            UpdateMinimapIcon(npcPositionOnMinimap, Color.green);
        }


    }

    // ���� ��ǥ�� �̴ϸ� UI Raw Image �� ��ǥ�� ��ȯ
    private Vector2 WorldToMinimapPosition(Vector3 worldPosition)
    {
        // ���⿡�� ��ȯ���� �ۼ��ϼ���. (�̴ϸ� �̹����� ���� ������ ũ�� �� �������� ���)
        // ����: �̴ϸ��� ���� ������ 1/10 ũ��� ����
        return new Vector2(worldPosition.x / 10, worldPosition.z / 10);
    }

    // �̴ϸ� ������ ��ġ ������Ʈ
    private void UpdateMinimapIcon(Vector2 position, Color iconColor)
    {
        // �̴ϸ� UI Raw Image���� ������ ��ġ �� ������ ������Ʈ
        // ���⿡�� UI Raw Image�� �������� �׸��ų� ��ġ�� ������Ʈ�ϴ� �ڵ带 �ۼ��ϼ���.
        // ����: �̴ϸ� UI Raw Image�� �ڽ����� ������ �̹����� ������ �ִ� ���� ����
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
