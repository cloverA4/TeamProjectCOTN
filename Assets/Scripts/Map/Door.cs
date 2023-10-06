using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] SpriteRenderer _doorImage;

    private int _wallCount;
    private int _WallCountReset = 2; //주변의 벽 갯수 주변벽이깨지면 문파괴

    void Start()
    {
        _wallCount = _WallCountReset;
    }

    public void DoorReset()
    {
        _doorImage.sortingOrder = (int)(transform.position.y - 1) * -1; // y의 위치를 확인하고 이미지를 정렬시켜주는 구문     
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
