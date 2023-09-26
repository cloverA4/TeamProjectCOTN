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

    
}
