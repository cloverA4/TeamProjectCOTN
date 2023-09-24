using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class EliteMonsterThrowDagger : MonoBehaviour
{
    private LayerMask _obstacleLayer; // ��ֹ� ���̾�


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
        // ����Ʈ ���� ��� �߻����
        if (hit.collider != null)
        {
            float laserLength = hit.distance;
            Vector2 laserPosition = transform.position + (DaggerDirection * laserLength / 2f);

            GameObject laserEffect = Instantiate(this.gameObject, laserPosition, Quaternion.identity);

            // ������ ����Ʈ�� ���� ����
            laserEffect.transform.localScale = new Vector3(1f, laserLength, 1f);

            // 0.2�� �Ŀ� ������ ����Ʈ ����
            Destroy(laserEffect, 0.2f);
        }

        // �������� 0.2�� �Ŀ� �ı�
        Destroy(gameObject, 0.2f);
    }
}
