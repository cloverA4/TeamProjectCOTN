using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShakeCamera : MonoBehaviour
{
    private static ShakeCamera instance;

    // 피격이벤트지속시간
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
    //        ////초기값 저장
    //        //_time = 0;

    //        //// 카메라가 캐릭터 위치 값을 가진 지역변수
    //        //_camera.transform.position = PlayerController.Instance.gameObject.transform.position + new Vector3(0, 0, -10f);


    //        //_time += Time.deltaTime;
    //        ////흔들기
    //        //while (_time < shakeDuration)//일정시간동안?
    //        //{
    //        //    //Vector3 shakeOffset = Random.insideUnitSphere * 0.5f;

    //        //    // 카메라를 랜덤으로 흔들고 흔들때 흔들림강도를 곱해줘서 역동적이게 만들기 그변수를 따로 저장
    //        //    Vector3 shakeOffset = Random.insideUnitSphere * shakeIntensity;
    //        //    // 내 위치 = 내위치 + 흔드는 값 
    //        //    this.gameObject.transform.position = _camera.transform.position + shakeOffset;
    //        //    // 경과시간을 게임시간에 저장
    //        //    saveTime += Time.deltaTime;
    //        //}
    //        ////초기값으로 되돌리기
    //        //gameObject.transform.position = _camera.transform.position;
    //        ShakeAndFlashCamera();
    //        _isShake = false;
    //    }
    //}
    public void ShakeAndFlashCamera()
    {
        StartCoroutine(PlayerHit()); // 코루틴을 이용해서 이벤트 발생 위아래로 움직이는 코루틴
       // StartCoroutine(FlashBackGround());    //배경화면이 빨간색이 되었다 정상으로 돌아오는 코루틴
    }

    IEnumerator PlayerHit()
    {
        // 카메라가 캐릭터 위치 값을 가진 지역변수
        Vector3 cameraPosition = PlayerController.Instance.transform.position + new Vector3(0, 0, -10f);
        
        // 흔들림 강도 변수
        float shakeIntensity = 0.5f;
        // 경과시간을 저장할 변수
        float saveTime = 0f;

        // 경과시간보다 흔드는 지속시간이 크면 
        while (saveTime < _hitDuration)
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
}
