using UnityEngine;
using UnityEngine.Pool;

public class GameInfoMassege : MonoBehaviour
{
    IObjectPool<GameInfoMassege> _pool;

    public void Init()
    {
        gameObject.SetActive(false);
    }
    public void SetPool(IObjectPool<GameInfoMassege> pool)
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
