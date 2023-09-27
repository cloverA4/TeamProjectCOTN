using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    private static CameraMove instance;

    public static CameraMove Instance
    {
        get
        {
            if (null == instance)
            {
                return null;
            }
            return instance;
        }
    }

    Transform _player;
    Vector3 _offset;
    Vector3 velocity = Vector3.zero;
    float _time = 0f; // 시간초기화
    float _smoothTime = 0.3f; // 부드러운 이동을 위한 시간
    bool _isShake = false;
    float _shakeEndTime = 0.2f; // 흔드는 시간
    float shakeIntensity = 0.1f;// 흔드는 강도

    public void Shake()
    {
        if (_isShake == false)
        {
            _isShake = true;
        }
    }

    private void Awake()
    {
        if (null == instance)
        {
            instance = this;

            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }



    private void Start()
    {
        _player = PlayerController.Instance.transform;
        _offset = new Vector3(0, 0, -10);
    }

    void LateUpdate() // LateUpdate를 사용하여 카메라 업데이트
    {
        // 플레이어의 현재 위치에 offset을 더한 위치를 목표 위치로 설정
        Vector3 targetPosition = _player.position + _offset;
        if (_isShake)
        {
            _time += Time.deltaTime;
            if (_time < _shakeEndTime)
            {
                // 카메라 흔들기
                Vector3 shakeOffset = Random.insideUnitSphere * shakeIntensity;
                transform.position += shakeOffset;
            }
            else
            {
                // 흔들기가 끝났으면 원래 위치로 복귀
                transform.position = Vector3.SmoothDamp(transform.position, _player.position + _offset, ref velocity, _smoothTime);
                _isShake = false;
                _time = 0f;
            }
        }
        else
        {
            // SmoothDamp 함수를 사용하여 현재 카메라 위치에서 목표 위치로 서서히 이동
            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, _smoothTime);
        }
    }

    
}
