using UnityEngine;

public class InfoMassege : MonoBehaviour
{
    // 상황에 맞는 Text를 입력을 받고 그에 맞게 텍스트를 변환

    // defult 위치는 플레이어 위치

    // 박자가 빗나갔을경우 심장쪽 위치


    private void OnEnable() // 함수가 풀링에서 활성화 시 바로 애니메이션 실행
    {
        gameObject.GetComponent<Animator>().SetTrigger("InfoTrigger");
    }
}
