using UnityEngine;
using UnityEngine.Pool;

public class MissInfoMassege : MonoBehaviour
{
    IObjectPool<MissInfoMassege> _pool;

    public void Init()
    {
        gameObject.SetActive(false);
    }
    public void SetPool(IObjectPool<MissInfoMassege> pool) 
    {
        _pool = pool;
    }
    public void EndAni()
    {
       Release();
    }
    public void Release()
    {
        _pool.Release(this);
    }
}
