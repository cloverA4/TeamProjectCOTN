using System.Collections;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    SpriteRenderer _spriter; // 변수 선언과 초기화하기
    [SerializeField] 
    LayerMask _layerMask;
    [SerializeField]
    MakeFog2 _MakeFog2;

    int shovelPower = 1;
    bool isLive = true;
    bool _isSuccess = true;
    bool _isDubbleClick = true;

    // Start is called before the first frame update
    void Start()
    {
        _spriter = GetComponent<SpriteRenderer>(); // 마찬가지로 가져오는 함수
    }

    // Update is called once per frame
    void Update()
    {
        //if (Data.Instance.Player.State == CharacterState.Live && _isSuccess && _isDubbleClick)
        if (_isSuccess && _isDubbleClick)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow)) // 위 화살표를 입력 받았을때
            {
                if (!Judgement()) return;
                MoveCharacter(Vector3.up);
                _MakeFog2.UpdateFogOfWar();
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow)) // 아래 화살표를 입력 받았을때
            {
                if (!Judgement()) return;
                MoveCharacter(Vector3.down);
                _MakeFog2.UpdateFogOfWar();
            }
            else if(Input.GetKeyDown(KeyCode.RightArrow)) // 오른쪽 화살표를 입력 받았을때
            {
                if (!Judgement()) return;
                MoveCharacter(Vector3.right);
                _spriter.flipX = true;
                _MakeFog2.UpdateFogOfWar();
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow)) // 왼쪽 화살표를 입력 받았을때
            {
                if(!Judgement()) return;
                MoveCharacter(Vector3.left);
                _spriter.flipX = false;
                _MakeFog2.UpdateFogOfWar();
            }
        }
    }

    bool Judgement()
    {
        _isDubbleClick = false;
        Invoke("DubbleLock", 0.1f);

        if (!GameManager.Instance.IsSuccess())
        {
            _isSuccess = false;
            Invoke("penalty", 60 / GameManager.Instance.BPM);
            return false;
        }
        return true;
    }

    void DubbleLock()
    {
        _isDubbleClick = true;
    }
    void penalty()
    {
        Debug.Log("패널티 해제!");
        _isSuccess = true;
        _isDubbleClick = true;
    }

    void MoveCharacter(Vector3 vec)
    {
        Vector3 Temp = transform.position + new Vector3(0, -0.5f, 0);
        RaycastHit2D hitdata = Physics2D.Raycast(Temp, vec, 1f, _layerMask);
        // 왼쪽으로 빔을쏘는 
        
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

    void Death()
    {        
        GameManager.Instance.StageFail();
    }
}
