using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class NormalGreatSwordEffectPool : MonoBehaviour
{

    [SerializeField] private WeaponEffectObject _normalGreatSwordEffectPrefab;

    private IObjectPool<WeaponEffectObject> _weaponEffectPool;

    public IObjectPool<WeaponEffectObject> WeaponEffectPool
    {
        get { return _weaponEffectPool; }
        set { _weaponEffectPool = value; }

    }

    private void Awake()
    {
        _weaponEffectPool = new ObjectPool<WeaponEffectObject>(
            CreateWeaponEffect,
            OnGetWeaponEffect,
            OnReleaseWeaponEffect,
            OnDestroyWeaponEffect,
            maxSize: 4
            );
    }


    private WeaponEffectObject CreateWeaponEffect()
    {
        WeaponEffectObject weaponEffectObject = Instantiate(_normalGreatSwordEffectPrefab).GetComponent<WeaponEffectObject>();
        weaponEffectObject.transform.parent = transform;
        weaponEffectObject.SetMenagedPool(_weaponEffectPool);
        return weaponEffectObject;
    }

    private void OnGetWeaponEffect(WeaponEffectObject weaponEffectObject)
    {
        weaponEffectObject.gameObject.SetActive(true);
    }

    private void OnReleaseWeaponEffect(WeaponEffectObject weaponEffectObject)
    {
        weaponEffectObject.gameObject.SetActive(false);
    }

    private void OnDestroyWeaponEffect(WeaponEffectObject weaponEffectObject)
    {
        Destroy(weaponEffectObject.gameObject);
    }

}
