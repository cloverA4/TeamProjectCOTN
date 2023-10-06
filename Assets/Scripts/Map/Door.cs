using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] SpriteRenderer _doorImage;

    private int _wallCount;
    private int _WallCountReset = 2; //�ֺ��� �� ���� �ֺ����̱����� ���ı�

    void Start()
    {
        _wallCount = _WallCountReset;
    }

    public void DoorReset()
    {
        _doorImage.sortingOrder = (int)(transform.position.y - 1) * -1; // y�� ��ġ�� Ȯ���ϰ� �̹����� ���Ľ����ִ� ����     
        _wallCount = _WallCountReset;
        gameObject.SetActive(true);
    }

    public void updateWallCount()
    {
        _wallCount--;
        if(_wallCount <= 0)
        {
            OpenDoor();
        }
    }

    public void OpenDoor()
    {
        gameObject.SetActive(false);
    }
}
