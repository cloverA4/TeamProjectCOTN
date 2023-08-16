using System;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "SaveInfoData", menuName = "ScriptableObject/SaveInfoData")]
public class SaveInfoData : ScriptableObject //ScriptableObject¸¦ »ó¼Ó
{
    public List<UnlockNeedDia> unlockNeedDias;
    public List<UnlockCount> unlockCount;
    public List<UnlockItemInfo> unlockItemInfo;
}

[Serializable]
public class UnlockNeedDia
{
    public int level;
    public int NeedDia;
}

[Serializable]
public class UnlockCount
{
    public string name;
    public int count;
}

[Serializable]
public class UnlockItemInfo
{
    public int Itemid;
    public bool IsUnlocked;
}