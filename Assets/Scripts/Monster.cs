using UnityEngine;
using System;

public class Monster : MonoBehaviour
{
    MonsterType _type;
    public MonsterType Type { get { return _type; } }

    void Start()
    {
        GameManager.Instance.MosterMoveEnvent += new EventHandler(MonsterMove);
    }

    public void Init(MonsterType Type)
    {
        _type = Type;
    }

    void MonsterMove(object sender, EventArgs s)
    {

    }
}

public enum MonsterType
{
    Monster1,
    Monster2,
    Monster3,
    Monster4,
    EliteMonster,
}
