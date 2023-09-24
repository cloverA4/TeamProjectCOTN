using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class EliteMonsterThrowDagger : MonoBehaviour
{
    private LayerMask _obstacleLayer; // 장애물 레이어


    private void Start()
    {
        _obstacleLayer =
            (1 << LayerMask.NameToLayer("Wall"));
    }

    private void Update()
    {
        
        StartLaserAttack();
        
    }

    private void StartLaserAttack()
    {
        Vector3 DaggerDirection = transform.forward;

        RaycastHit2D hit = Physics2D.Raycast(transform.position, DaggerDirection, 100f, _obstacleLayer);
        // 엘리트 몬스터 대거 발사루프
        if (hit.collider != null)
        {
            float laserLength = hit.distance;
            Vector2 laserPosition = transform.position + (DaggerDirection * laserLength / 2f);

            GameObject laserEffect = Instantiate(this.gameObject, laserPosition, Quaternion.identity);

            // 레이저 이펙트의 길이 설정
            laserEffect.transform.localScale = new Vector3(1f, laserLength, 1f);

            // 0.2초 후에 레이저 이펙트 삭제
            Destroy(laserEffect, 0.2f);
        }

        // 레이저를 0.2초 후에 파괴
        Destroy(gameObject, 0.2f);
    }
}
