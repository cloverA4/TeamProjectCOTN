using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class EliteMonsterThrowDagger : MonoBehaviour
{
    private string targetLayerName = "Wall"; // ������ ��� ���̾� �̸�
    private float destroyDelay = 0.2f; // �浹 �� ���� ���� �ð�

    public void Init(Vector3 vec , Vector3 Attackdirection)
    {
        //�����ɽ�Ʈ�� ���� �Ÿ��� �˾� �´����� �װŸ���ŭ scale�� x ���� �÷��ְ� x�������� ���� scale�� x���� ������ ������ִ� ����

        Transform SpecialAttackEffect = gameObject.transform;
        // ����ĳ��Ʈ ���� ��ġ�� �����մϴ�.
        SpecialAttackEffect.position = vec;

        // Raycast�� �����Ͽ� �浹 ������ �����ɴϴ�.
        RaycastHit2D hit = Physics2D.Raycast(SpecialAttackEffect.position, Attackdirection, 100f, 1 << LayerMask.NameToLayer("Wall"));
        
        if (hit)
        {
            Debug.Log(hit.collider.name);
            // �浹 ���������� �Ÿ��� �����մϴ�.
            float distanceToHit = hit.distance;
            Debug.Log(distanceToHit);

            // x ������ ���� �Ÿ��� ���� �����մϴ�.
            float newScaleX = Mathf.Min(distanceToHit);

            float newXPosition = 0;
            if (Attackdirection == Vector3.left)
            {
                newXPosition = -newScaleX / 2;
            }
            else if (Attackdirection == Vector3.right)
            {
                newXPosition = newScaleX / 2;
            }
            // x ������ ���� �������� x ���� �������� �����մϴ�.


            // �����ϰ� �������� ������Ʈ�մϴ�.
            transform.localScale = new Vector3(hit.distance, 1, 1);
            transform.localPosition += new Vector3(newXPosition, 0, 0);
        }
        Invoke("DestroyEffect", destroyDelay);
    }

    private void DestroyEffect()
    {
        // ����Ʈ�� �����մϴ�.
        Destroy(gameObject);
    }

}
