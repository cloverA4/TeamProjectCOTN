using UnityEngine;

public class ItemInfo : MonoBehaviour
{
    [SerializeField] GameObject _ItemInfo;

    public void SetItemName(string _name)
    {
        var temp = Instantiate(_ItemInfo,transform);
        temp.GetComponent<ItemNameCanvas>().InitItemName(name);
    }
}
