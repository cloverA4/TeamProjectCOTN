using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShakeCamera : MonoBehaviour
{
    private static ShakeCamera instance;

    // �ǰ��̺�Ʈ���ӽð�
    float _hitDuration = 0.2f;

    public static ShakeCamera Instance
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

    bool _isShake= false;
    float _time;

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

    //public void Shake()
    //{
    //    if(_isShake == false)
    //    {
    //        _isShake = true;
    //    }
    //}

    //// Update is called once per frame
    //void Update()
    //{
    //    if(_isShake == true)
    //    {
    //        ////�ʱⰪ ����
    //        //_time = 0;

    //        //// ī�޶� ĳ���� ��ġ ���� ���� ��������
    //        //_camera.transform.position = PlayerController.Instance.gameObject.transform.position + new Vector3(0, 0, -10f);


    //        //_time += Time.deltaTime;
    //        ////����
    //        //while (_time < shakeDuration)//�����ð�����?
    //        //{
    //        //    //Vector3 shakeOffset = Random.insideUnitSphere * 0.5f;

    //        //    // ī�޶� �������� ���� ��鶧 ��鸲������ �����༭ �������̰� ����� �׺����� ���� ����
    //        //    Vector3 shakeOffset = Random.insideUnitSphere * shakeIntensity;
    //        //    // �� ��ġ = ����ġ + ���� �� 
    //        //    this.gameObject.transform.position = _camera.transform.position + shakeOffset;
    //        //    // ����ð��� ���ӽð��� ����
    //        //    saveTime += Time.deltaTime;
    //        //}
    //        ////�ʱⰪ���� �ǵ�����
    //        //gameObject.transform.position = _camera.transform.position;
    //        ShakeAndFlashCamera();
    //        _isShake = false;
    //    }
    //}
    public void ShakeAndFlashCamera()
    {
        StartCoroutine(PlayerHit()); // �ڷ�ƾ�� �̿��ؼ� �̺�Ʈ �߻� ���Ʒ��� �����̴� �ڷ�ƾ
       // StartCoroutine(FlashBackGround());    //���ȭ���� �������� �Ǿ��� �������� ���ƿ��� �ڷ�ƾ
    }

    IEnumerator PlayerHit()
    {
        // ī�޶� ĳ���� ��ġ ���� ���� ��������
        Vector3 cameraPosition = PlayerController.Instance.transform.position + new Vector3(0, 0, -10f);
        
        // ��鸲 ���� ����
        float shakeIntensity = 0.5f;
        // ����ð��� ������ ����
        float saveTime = 0f;

        // ����ð����� ���� ���ӽð��� ũ�� 
        while (saveTime < _hitDuration)
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
}
