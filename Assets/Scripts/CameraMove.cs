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

    public void ShakeAndFlashCamera()
    {
        StartCoroutine(ShakeCamera()); // �ڷ�ƾ�� �̿��ؼ� �̺�Ʈ �߻� ���Ʒ��� �����̴� �ڷ�ƾ
        StartCoroutine(FlashBackGround());    //���ȭ���� �������� �Ǿ��� �������� ���ƿ��� �ڷ�ƾ
    }

    IEnumerator ShakeCamera()
    {
        // ī�޶� ĳ���� ��ġ ���� ���� ��������
        Vector3 cameraPosition = PlayerController.Instance.transform.position + new Vector3(0,0,-10f);
        // ���� ���� �ð� �� ����
        float shakeDuration = 0.5f;
        // ��鸲 ���� ����
        float shakeIntensity = 0.5f;
        // ����ð��� ������ ����
        float saveTime = 0f;

        // ����ð����� ���� ���ӽð��� ũ�� 
        while (saveTime < shakeDuration)
        {
            // ī�޶� �������� ���� ��鶧 ��鸲������ �����༭ �������̰� ����� �׺����� ���� ����
            Vector3 shakeOffset = Random.insideUnitSphere * shakeIntensity;
            // �� ��ġ = ����ġ + ���� �� 
            transform.position = cameraPosition + shakeOffset;
            // ����ð��� ���ӽð��� ����
            saveTime += Time.deltaTime;
            // ����
            yield return null;

        }
        // ī�޶���ġ �ٽ� ���ڸ��� ������
        transform.position = cameraPosition;
    }

    IEnumerator FlashBackGround()
    {
        Camera mainCamera = Camera.main;
        // ���� ��׶��� ������ ������ �ִ� ����
        Color originalBackgroundColor = mainCamera.backgroundColor;
        // ������ ���������� ������� ����
        Color flashColor = new Color(1f, 0f, 0f, 0.5f);
        // �������� ������ų �ð�
        float flashDuration = 0.2f;

        // ����ī�޶� �������� ������ֱ�
        mainCamera.backgroundColor = flashColor;
        // �����Ҷ� �������� ������ų �ð��� �޾ƿͼ� ����
        yield return new WaitForSeconds(flashDuration);

        // ��������� �ʱ�ȭ
        mainCamera.backgroundColor = originalBackgroundColor;
    }
}
