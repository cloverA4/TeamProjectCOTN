using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour
{
    public BoxType _type { get; private set; }
    public void InitBox(BoxType type)
    {
        _type = type;
    }

    public void OpenBox()
    {
        switch (_type)
        {
            case BoxType.Normal:
                break;
            case BoxType.Clear:

                GameObject go = Instantiate(Data.Instance.ItemPrefab, GameManager.Instance.ItemPool.transform);
                go.transform.position = transform.position;
                int dropCount = UnityEngine.Random.Range(3, 6);

                Currency cr = (Currency)Data.Instance.GetItemInfo(101);
                cr.Count = dropCount;
                go.GetComponent<DropItem>().Init(cr);

                GameManager.Instance.EliteMonsterDie();

                DestroyBox();
                break;
        }
    }

    public void DestroyBox()
    {
        Destroy(gameObject);
    }
}
