using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
                //해금된 모든 아이템 중 랜덤?

                int random = UnityEngine.Random.Range(0, Data.Instance.CharacterSaveData._unlockItemId.Count);
                int ItemID = Data.Instance.CharacterSaveData._unlockItemId[random];
                
                GameObject SpawnItem = Instantiate(Data.Instance.ItemPrefab, GameManager.Instance.ItemPool.transform);
                SpawnItem.GetComponent<DropItem>().Init(Data.Instance.GetItemInfo(ItemID), DropItemType.Drop);
                SpawnItem.transform.position = transform.position;
                break;
            case BoxType.Clear:

                GameObject go = Instantiate(Data.Instance.ItemPrefab, GameManager.Instance.ItemPool.transform);
                go.transform.position = transform.position;
                int dropCount = UnityEngine.Random.Range(3, 6);

                Currency cr = (Currency)Data.Instance.GetItemInfo(101);
                cr.Count = dropCount;
                go.GetComponent<DropItem>().Init(cr);
                go.GetComponent<DropItem>().OpenItemInfo();

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
