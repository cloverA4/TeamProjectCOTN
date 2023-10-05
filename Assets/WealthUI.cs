using UnityEngine;
using UnityEngine.UI;

public class WealthUI : MonoBehaviour
{
    [SerializeField] Text _goldCount;
    [SerializeField] Text _diamondCount;


    public void UpdateGold(int _count)
    {
        _goldCount.text = "X " + _count;
    }

    public void UpdataDiamond(int _count)
    {
        _diamondCount.text = "X " + _count;
    }
}
