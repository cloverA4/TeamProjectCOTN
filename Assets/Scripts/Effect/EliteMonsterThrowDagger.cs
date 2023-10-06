using UnityEngine;

public class EliteMonsterThrowDagger : MonoBehaviour
{
    private float destroyDelay = 0.2f; // 충돌 후 삭제 지연 시간

    public void Init(Vector3 vec , Vector3 Attackdirection)
    {
        Transform SpecialAttackEffect = gameObject.transform; // 레이캐스트 시작 위치를 지정합니다.
        SpecialAttackEffect.position = vec;

        // Raycast를 실행하여 충돌 정보를 가져옵니다.
        RaycastHit2D hit = Physics2D.Raycast(SpecialAttackEffect.position, Attackdirection, 100f, 1 << LayerMask.NameToLayer("Wall"));
        
        if (hit)
        {
            // x 포지션 값을 스케일의 x 값의 절반으로 설정합니다.
            float newXPosition = 0;
            if (Attackdirection == Vector3.left)
            {
                newXPosition = -hit.distance / 2;
            }
            else if (Attackdirection == Vector3.right)
            {
                newXPosition = hit.distance / 2;
            }
            // 스케일과 포지션을 업데이트합니다.
            transform.localScale = new Vector3(hit.distance, 1, 1);
            transform.localPosition += new Vector3(newXPosition, 0, 0);
        }
        Invoke("DestroyEffect", destroyDelay);
    }

    private void DestroyEffect()
    {
        // 이펙트를 삭제합니다.
        Destroy(gameObject);
    }

}
