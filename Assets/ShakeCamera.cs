using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakeCamera : MonoBehaviour
{
    //얘를 싱글턴으로 만들어
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
            //초기값 저장
            _time = 0;
            _pos = transform.position;

            _time += Time.deltaTime;
            //흔들기
            while (_time < 0.2)//일정시간동안?
            {
                Vector3 shakeOffset = Random.insideUnitSphere * 0.5f;
            }

            //초기값으로 되돌리기
            transform.position = _pos;
            _isShake = false;
        }
    }
}
