using UnityEngine;

public class LobbyFog : MonoBehaviour
{
    [SerializeField] GameObject _door;
    SpriteRenderer _spriteRenderer;

    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (!_door.activeSelf)
        {
            _spriteRenderer.enabled = false;
        }
        else
        {
            _spriteRenderer.enabled = true;
        }
    }
}
