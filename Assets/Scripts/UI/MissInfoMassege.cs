using UnityEngine;
using UnityEngine.Pool;

public class MissInfoMassege : MonoBehaviour
{
    // 상황에 맞는 Text를 입력을 받고 그에 맞게 텍스트를 변환

    // defult 위치는 플레이어 위치

    // 박자가 빗나갔을경우 심장쪽 위치

    IObjectPool<MissInfoMassege> _pool;

    private void Start()
    {

    }
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
