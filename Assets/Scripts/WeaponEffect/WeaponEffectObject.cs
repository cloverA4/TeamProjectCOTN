using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class WeaponEffectObject : MonoBehaviour
{
    
    private Vector3 _Direction;

    private IObjectPool<WeaponEffectObject> _weaponEffectPool;

    public void SetMenagedPool(IObjectPool<WeaponEffectObject> pool)
    {
        _weaponEffectPool = pool;
    }

    public void Swing(Vector3 dir)
    {
        _Direction = dir;
        Invoke("DestroySpear", 1f);
    }

    public void DestroySpear()
    {
        _weaponEffectPool.Release(this);
    }
}
