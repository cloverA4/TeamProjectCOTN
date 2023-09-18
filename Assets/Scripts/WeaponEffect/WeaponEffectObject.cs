using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class WeaponEffectObject : MonoBehaviour
{

    private IObjectPool<WeaponEffectObject> _weaponEffectPool;

    public void SetMenagedPool(IObjectPool<WeaponEffectObject> pool)
    {
        _weaponEffectPool = pool;
    }

    public void SwingAndRemove()
    {
        Invoke("DestroyWeaponEffect", 0.3f);
    }

    public void DestroyWeaponEffect()
    {
        GetComponent<SpriteRenderer>().flipX = false;
        transform.rotation = Quaternion.identity;
        _weaponEffectPool.Release(this);
    }
}
