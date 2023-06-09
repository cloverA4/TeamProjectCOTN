using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    SpriteRenderer _spriter; // 변수 선언과 초기화하기
    [SerializeField] LayerMask _layerMask;

    int shovelPower = 1;

    // Start is called before the first frame update
    void Start()
    {
        _spriter = GetComponent<SpriteRenderer>(); // 마찬가지로 가져오는 함수
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow)) // 위 화살표를 입력 받았을때
        {
            MoveCharacter(Vector3.up);
        }

        if (Input.GetKeyDown(KeyCode.DownArrow)) // 아래 화살표를 입력 받았을때
        {
            MoveCharacter(Vector3.down);
        }

        if (Input.GetKeyDown(KeyCode.RightArrow)) // 오른쪽 화살표를 입력 받았을때
        {
            MoveCharacter(Vector3.right);
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow)) // 왼쪽 화살표를 입력 받았을때
        {
            MoveCharacter(Vector3.left);
        }
    }

    void MoveCharacter(Vector3 vec)
    {
        Vector3 Temp = transform.position + new Vector3(0, -0.5f, 0);
        RaycastHit2D hitdata = Physics2D.Raycast(Temp, vec, 1f, _layerMask);
        // 왼쪽으로 빔을쏘는 
        _spriter.flipX = false;

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
                //hitdata.collider.GetComponent<Door>().InvincibilityWall();
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
            transform.position += vec;
        }
    }
}
