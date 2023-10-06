using UnityEngine;

public class HeartPrefeb : MonoBehaviour
{
    [SerializeField] GameObject _fullHeart;
    [SerializeField] GameObject _halfHeart;

    public void EmptyHeartActive()
    {
        _fullHeart.SetActive(false);
        _halfHeart.SetActive(false);
    }
    public void FullHeartActive()
    {
        _fullHeart.SetActive(true);
        _halfHeart.SetActive(false);
    }
    public void HalfHeartActive()
    {
        _halfHeart.SetActive(true);
        _halfHeart.SetActive(false);
    }
}
