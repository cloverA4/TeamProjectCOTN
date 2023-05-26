using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class IdleController : MonoBehaviour
{
    Rigidbody2D _rigid; // 물리연산 선언과 초기화
    SpriteRenderer _spriter; // 변수 선언과 초기화하기
    Animator _anim; // 애니메이션 선언과 초기화
    Transform _idle;

    [SerializeField]
    LayerMask _layerMask;

    [SerializeField]
    LayerMask _layerMask2;

    int shovelPower = 1;



    // Start is called before the first frame update
    void Start()
    {
        _idle = GetComponent<Transform>();
        _rigid = GetComponent<Rigidbody2D>(); // 컴포넌트를 가져오는 함수
        _spriter = GetComponent<SpriteRenderer>(); // 마찬가지로 가져오는 함수
        _anim = GetComponent<Animator>(); // Animator 변수 선언과 초기화하기






    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.UpArrow)) // 위 화살표를 입력 받았을때
        {
            Vector3 Temp = transform.position + new Vector3(0, -0.5f, 0);
            RaycastHit2D hitdata = Physics2D.Raycast(Temp, Vector3.up, 1f, _layerMask);
            
            //레이를 발사하는 위치

            Debug.Log(transform.position);
            // 위쪽으로 빔을쐈을때
            if (hitdata) // 빔을쐇을때
            {
                
                if (hitdata.collider.tag == "WeedWall") // weedwall이 힛데이타에 태그로 들어왓다면
                {
                    //Debug.Log(hitdata.collider.gameObject);
                    //Destroy(hitdata.collider.gameObject);
                    hitdata.collider.GetComponent<Wall>().DamageWall(shovelPower);

                }

                else if (hitdata.collider.tag == "Door") // Door이(가) 힛데이타에 태그로 들어왓다면
                {
                    
                    hitdata.collider.GetComponent<Door>().OpenDoor();

                }

                else if (hitdata.collider.tag == "BadRock") // weedwall이 힛데이타에 태그로 들어왓다면
                {
                    
                }

                //else if(hitdata.collider.tag == "적태그이름")
                //{
                //    공격에니메이션
                //    공격
                //    적의 체력이 낮아짐
                //    적의 체력이 0이됬을때
                //    적의 오브젝트가 부서짐?
                //}
            }
            else
            {
                _idle.position += Vector3.up;
            }
           
        }

        if (Input.GetKeyDown(KeyCode.DownArrow)) // 아래 화살표를 입력 받았을때
        {
            Vector3 Temp = transform.position + new Vector3(0, -0.5f, 0);
            RaycastHit2D hitdata = Physics2D.Raycast(Temp, Vector3.down, 1f, _layerMask);
            // 아래쪽으로 빔을쏘는 

            
            if (hitdata) // 빔을쐇을때
            {
                //Debug.Log(hitdata.collider.gameObject.name);
                if (hitdata.collider.tag == "WeedWall") // weedwall이 힛데이타에 태그로 들어왓다면
                {
                    //Debug.Log(hitdata.collider.gameObject);
                    //Destroy(hitdata.collider.gameObject);
                    hitdata.collider.GetComponent<Wall>().DamageWall(shovelPower);

                }
                else if (hitdata.collider.tag == "Door") // Door이(가) 힛데이타에 태그로 들어왓다면
                {

                    hitdata.collider.GetComponent<Door>().OpenDoor();

                }

                else if (hitdata.collider.tag == "BadRock") // weedwall이 힛데이타에 태그로 들어왓다면
                {

                }
                //else if(hitdata.collider.tag == "적태그이름")
                //{
                //    공격에니메이션
                //    공격
                //    적의 체력이 낮아짐
                //    적의 체력이 0이됬을때
                //    적의 오브젝트가 부서짐?
                //}
            }
            else
            {
                _idle.position += Vector3.down;
            }
        }

        if (Input.GetKeyDown(KeyCode.RightArrow)) // 오른쪽 화살표를 입력 받았을때
        {
            Vector3 Temp = transform.position + new Vector3(0, -0.5f, 0);
            RaycastHit2D hitdata = Physics2D.Raycast(Temp, Vector3.right, 1f, _layerMask);
            // 오른쪽으로 빔을쏘는 

            Debug.Log(transform.position);
            if (hitdata)
            {
                if (hitdata.collider.tag == "WeedWall") // weedwall이 힛데이타에 태그로 들어왓다면
                {
                    //Debug.Log(hitdata.collider.gameObject);
                    //Destroy(hitdata.collider.gameObject);
                    hitdata.collider.GetComponent<Wall>().DamageWall(shovelPower);

                }

                else if (hitdata.collider.tag == "Door") // Door이(가) 힛데이타에 태그로 들어왓다면
                {

                    hitdata.collider.GetComponent<Door>().OpenDoor();

                }

                else if (hitdata.collider.tag == "BadRock") // weedwall이 힛데이타에 태그로 들어왓다면
                {

                }
                //else if(hitdata.collider.tag == "적태그이름")
                //{
                //    공격에니메이션
                //    공격
                //    적의 체력이 낮아짐
                //    적의 체력이 0이됬을때
                //    적의 오브젝트가 부서짐?
                //}
            }
            else
            {
                _idle.position += Vector3.right;
                _spriter.flipX = true;
            }

        }

        if (Input.GetKeyDown(KeyCode.LeftArrow)) // 왼쪽 화살표를 입력 받았을때
        {
            Vector3 Temp = transform.position + new Vector3(0, -0.5f, 0);
            RaycastHit2D hitdata = Physics2D.Raycast(Temp, Vector3.left, 1f, _layerMask);
            // 왼쪽으로 빔을쏘는 

            Debug.Log(transform.position);
            if (hitdata)
            {
                if (hitdata.collider.tag == "WeedWall") // weedwall이 힛데이타에 태그로 들어왓다면
                {
                    //Debug.Log(hitdata.collider.gameObject); // 힛데이타콜라이더게임오브젝트에 대한 정보가 출력된다
                    //Destroy(hitdata.collider.gameObject); // 힛데이타콜라이더게임오브젝트를 파괴한다
                    //setActive활용해서 벽부수는 표현해보기
                    hitdata.collider.GetComponent<Wall>().DamageWall(shovelPower);

                }

                else if (hitdata.collider.tag == "Door") // Door이(가) 힛데이타에 태그로 들어왓다면
                {

                    hitdata.collider.GetComponent<Door>().OpenDoor();

                }

                else if (hitdata.collider.tag == "BadRock") // weedwall이 힛데이타에 태그로 들어왓다면
                {

                }
                //else if(hitdata.collider.tag == "적태그이름")
                //{
                //    공격에니메이션
                //    공격
                //    적의 체력이 낮아짐
                //    적의 체력이 0이됬을때
                //    적의 오브젝트가 부서짐?
                //}
            }
            else
            {
                _idle.position += Vector3.left;
                _spriter.flipX = false;
            }
        }

    


    }


    


}
