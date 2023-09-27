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
    float _time = 0f; // �ð��ʱ�ȭ
    float _smoothTime = 0.3f; // �ε巯�� �̵��� ���� �ð�
    bool _isShake = false;
    float _shakeEndTime = 0.2f; // ���� �ð�
    float shakeIntensity = 0.1f;// ���� ����

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

    void LateUpdate() // LateUpdate�� ����Ͽ� ī�޶� ������Ʈ
    {
        // �÷��̾��� ���� ��ġ�� offset�� ���� ��ġ�� ��ǥ ��ġ�� ����
        Vector3 targetPosition = _player.position + _offset;
        if (_isShake)
        {
            _time += Time.deltaTime;
            if (_time < _shakeEndTime)
            {
                // ī�޶� ����
                Vector3 shakeOffset = Random.insideUnitSphere * shakeIntensity;
                transform.position += shakeOffset;
            }
            else
            {
                // ���Ⱑ �������� ���� ��ġ�� ����
                transform.position = Vector3.SmoothDamp(transform.position, _player.position + _offset, ref velocity, _smoothTime);
                _isShake = false;
                _time = 0f;
            }
        }
        else
        {
            // SmoothDamp �Լ��� ����Ͽ� ���� ī�޶� ��ġ���� ��ǥ ��ġ�� ������ �̵�
            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, _smoothTime);
        }
    }

    
}
