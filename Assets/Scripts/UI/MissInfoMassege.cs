using UnityEngine;
using UnityEngine.Pool;

public class MissInfoMassege : MonoBehaviour
{
    // ��Ȳ�� �´� Text�� �Է��� �ް� �׿� �°� �ؽ�Ʈ�� ��ȯ

    // defult ��ġ�� �÷��̾� ��ġ

    // ���ڰ� ����������� ������ ��ġ

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
