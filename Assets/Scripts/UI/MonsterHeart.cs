using UnityEngine;

public class MonsterHeart : MonoBehaviour
{
    [SerializeField] GameObject _fullHeart;

    public void EmptyHeartActive()
    {
        _fullHeart.SetActive(false);
    }
    public void FullHeartActive()
    {
        _fullHeart.SetActive(true);
    }

}
