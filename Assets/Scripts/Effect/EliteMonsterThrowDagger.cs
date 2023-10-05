using UnityEngine;

public class EliteMonsterThrowDagger : MonoBehaviour
{
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

            float newXPosition = 0;
            if (Attackdirection == Vector3.left)
            {
                newXPosition = -hit.distance / 2;
            }
            else if (Attackdirection == Vector3.right)
            {
                newXPosition = hit.distance / 2;
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
