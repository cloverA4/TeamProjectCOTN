using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakeCamera : MonoBehaviour
{
    //�긦 �̱������� �����
    bool _isShake= false;
    float _time;
    Vector3 _pos;

    public void Shake()
    {
        if(_isShake == false)
        {
            _isShake = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(_isShake == true)
        {
            //�ʱⰪ ����
            _time = 0;
            _pos = transform.position;

            _time += Time.deltaTime;
            //����
            while (_time < 0.2)//�����ð�����?
            {
                Vector3 shakeOffset = Random.insideUnitSphere * 0.5f;
            }

            //�ʱⰪ���� �ǵ�����
            transform.position = _pos;
            _isShake = false;
        }
    }
}
