using UnityEngine;

public class EliteMonsterThrowDagger : MonoBehaviour
{
    private float destroyDelay = 0.2f; // 충돌 후 삭제 지연 시간

    public void Init(Vector3 vec , Vector3 Attackdirection)
    {
        //레이케스트를 쏴서 거리를 알아 온다음에 그거리만큼 scale의 x 값을 늘려주고 x포지션의 값을 scale의 x값의 절반을 만들어주는 구문

        Transform SpecialAttackEffect = gameObject.transform;
        // 레이캐스트 시작 위치를 지정합니다.
        SpecialAttackEffect.position = vec;

        // Raycast를 실행하여 충돌 정보를 가져옵니다.
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
            // x 포지션 값을 스케일의 x 값의 절반으로 설정합니다.


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
