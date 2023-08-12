using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spear : MonoBehaviour
{
    private int _spearDamage = 1;
    private float _spearRange = 2;



    //[SerializeField] LayerMask _layerMask;
    //private Transform _playerlocation;
    //private int _spearDamage = 1;
    //private float _spearRange = 2;

    //public void SpearAttackRange(Vector3 AttackRange)
    //{
    //    _playerlocation = PlayerController.Instance.GetComponent<Transform>();
    //    Vector3 Temp = _playerlocation.position;
    //    RaycastHit2D hitdata = Physics2D.Raycast(Temp, AttackRange, 2f, _layerMask);

    //    if (hitdata.collider.tag == "Monster")
    //    {
    //        hitdata.collider.GetComponent<Monster>().TakeDamage(_spearDamage);
    //    }
    //}
}
