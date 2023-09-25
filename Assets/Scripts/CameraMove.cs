using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{

    Transform _player;
    Vector3 _offset;
    float _smoothTime = 0.3f; // 부드러운 이동을 위한 시간

    Vector3 velocity = Vector3.zero;

    private void Start()
    {
        _player = PlayerController.Instance.transform;
        _offset = new Vector3(0, 0, -10);
    }

    void LateUpdate() // LateUpdate를 사용하여 카메라 업데이트
    {
        // 플레이어의 현재 위치에 offset을 더한 위치를 목표 위치로 설정
        Vector3 targetPosition = _player.position + _offset;

        // SmoothDamp 함수를 사용하여 현재 카메라 위치에서 목표 위치로 서서히 이동
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, _smoothTime);

        

    }

    public void ShakeAndFlashCamera()
    {
        StartCoroutine(ShakeCamera()); // 코루틴을 이용해서 이벤트 발생 위아래로 움직이는 코루틴
        StartCoroutine(FlashBackGround());    //배경화면이 빨간색이 되었다 정상으로 돌아오는 코루틴
    }

    IEnumerator ShakeCamera()
    {
        // 카메라가 캐릭터 위치 값을 가진 지역변수
        Vector3 cameraPosition = PlayerController.Instance.transform.position + new Vector3(0,0,-10f);
        // 흔드는 지속 시간 값 변수
        float shakeDuration = 0.5f;
        // 흔들림 강도 변수
        float shakeIntensity = 0.5f;
        // 경과시간을 저장할 변수
        float saveTime = 0f;

        // 경과시간보다 흔드는 지속시간이 크면 
        while (saveTime < shakeDuration)
        {
            // 카메라를 랜덤으로 흔들고 흔들때 흔들림강도를 곱해줘서 역동적이게 만들기 그변수를 따로 저장
            Vector3 shakeOffset = Random.insideUnitSphere * shakeIntensity;
            // 내 위치 = 내위치 + 흔드는 값 
            transform.position = cameraPosition + shakeOffset;
            // 경과시간을 게임시간에 저장
            saveTime += Time.deltaTime;
            // 리턴
            yield return null;

        }
        // 카메라위치 다시 제자리로 돌리기
        transform.position = cameraPosition;
    }

    IEnumerator FlashBackGround()
    {
        Camera mainCamera = Camera.main;
        // 원래 백그라운드 색깔을 가지고 있는 변수
        Color originalBackgroundColor = mainCamera.backgroundColor;
        // 색깔을 빨간색으로 만들어줄 변수
        Color flashColor = new Color(1f, 0f, 0f, 0.5f);
        // 빨간색을 유지시킬 시간
        float flashDuration = 0.2f;

        // 메인카메라를 빨강으로 만들어주기
        mainCamera.backgroundColor = flashColor;
        // 리턴할때 빨간색을 유지시킬 시간을 받아와서 리턴
        yield return new WaitForSeconds(flashDuration);

        // 원래색깔로 초기화
        mainCamera.backgroundColor = originalBackgroundColor;
    }
}
