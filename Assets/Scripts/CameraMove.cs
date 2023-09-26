using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{

    Transform _player;
    Vector3 _offset;
    float _smoothTime = 0.3f; // �ε巯�� �̵��� ���� �ð�

    Vector3 velocity = Vector3.zero;

   

    private void Start()
    {
        _player = PlayerController.Instance.transform;
        _offset = new Vector3(0, 0, -10);
    }

    void LateUpdate() // LateUpdate�� ����Ͽ� ī�޶� ������Ʈ
    {
        // �÷��̾��� ���� ��ġ�� offset�� ���� ��ġ�� ��ǥ ��ġ�� ����
        Vector3 targetPosition = _player.position + _offset;

        // SmoothDamp �Լ��� ����Ͽ� ���� ī�޶� ��ġ���� ��ǥ ��ġ�� ������ �̵�
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, _smoothTime);
    }

    
}
