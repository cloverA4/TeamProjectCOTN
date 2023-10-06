using UnityEngine;

public class ShopKeeper : MonoBehaviour
{
    SpriteRenderer _childSpriteRenderer;
    void Start()
    {
        _childSpriteRenderer = GetComponentInChildren<SpriteRenderer>();
        _childSpriteRenderer.sortingOrder = (int)(transform.position.y - 1) * -1; // 레이어 값변환
    }

    void Update()
    {
        PlayerLook();
    }

    void PlayerLook()
    {
        if (PlayerController.Instance.gameObject.transform.position.x > gameObject.transform.position.x)
        {
            _childSpriteRenderer.flipX = false; 
        }
        else
        {
            _childSpriteRenderer.flipX = true;
        }
    }
}
