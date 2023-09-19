using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;


public class MonsterAttackEffectPool : MonoBehaviour
{
    [SerializeField] private MonsterAttackEffectObject _monsterAttackEffectPrefab;

    private IObjectPool<MonsterAttackEffectObject> _monsterAttackEffectPool;

    public IObjectPool<MonsterAttackEffectObject> AttackEffectPool
    {
        get { return _monsterAttackEffectPool; }
        set { _monsterAttackEffectPool = value; }

    }

    private void Awake()
    {
        _monsterAttackEffectPool = new ObjectPool<MonsterAttackEffectObject>(
            CreateMonsterAttackEffect,
            OnGetMonsterAttackEffect,
            OnReleaseMonsterAttackEffect,
            OnDestroyMonsterAttackEffect,
            maxSize: 4
            );
    }


    private MonsterAttackEffectObject CreateMonsterAttackEffect()
    {
        MonsterAttackEffectObject weaponEffectObject = Instantiate(_monsterAttackEffectPrefab).GetComponent<MonsterAttackEffectObject>();
        weaponEffectObject.transform.parent = transform;
        weaponEffectObject.SetMenagedPool(_monsterAttackEffectPool);
        return weaponEffectObject;
    }

    private void OnGetMonsterAttackEffect(MonsterAttackEffectObject monsterAttackEffectObject)
    {
        monsterAttackEffectObject.gameObject.SetActive(true);
    }

    private void OnReleaseMonsterAttackEffect(MonsterAttackEffectObject monsterAttackEffectObject)
    {
        monsterAttackEffectObject.gameObject.SetActive(false);
    }

    private void OnDestroyMonsterAttackEffect(MonsterAttackEffectObject monsterAttackEffectObject)
    {
        Destroy(monsterAttackEffectObject.gameObject);
    }
}
