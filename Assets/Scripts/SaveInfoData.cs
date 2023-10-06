using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SaveInfoData", menuName = "ScriptableObject/SaveInfoData")]
public class SaveInfoData : ScriptableObject //ScriptableObject�� ���
{
    public List<UnlockNeedDia> unlockNeedDias;
}

[Serializable]
public class UnlockNeedDia
{
    public int level;
    public int NeedDia;
}