using UnityEngine;
using UnityEngine.UI;

public class ItemNameCanvas : MonoBehaviour
{
    [SerializeField] Text _itemName;

    private void Update()
    {
        if(_itemName.gameObject.activeSelf == false)
        {
            Destroy(gameObject);
        }
    }

    public void InitItemName(string _name)
    {
        _itemName.gameObject.SetActive(true);
        _itemName.GetComponent<Text>().text = _name;
    }

    public void NotEnoughCurrency(string _str)
    {
        _itemName.gameObject.SetActive(true);
        _itemName.GetComponent<Text>().text = _str;
    }
}
