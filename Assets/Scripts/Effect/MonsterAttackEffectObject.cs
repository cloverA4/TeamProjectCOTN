using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class MonsterAttackEffectObject : MonoBehaviour
{
    private IObjectPool<MonsterAttackEffectObject> _monsterAttackEffectObject;

    public void SetMenagedPool(IObjectPool<MonsterAttackEffectObject> pool)
    {
        _monsterAttackEffectObject = pool;
    }

    public void SwingAndRemove()
    {
        Invoke("DestroyMonsterAttackEffect", 0.3f);
    }

    public void DestroyMonsterAttackEffect()
    {
        GetComponent<SpriteRenderer>().flipX = false;
        transform.rotation = Quaternion.identity;
        _monsterAttackEffectObject.Release(this);
    }
}
