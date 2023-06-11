using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Door : MonoBehaviour
{
    //1
    //[SerializeField]
    //LayerMask objectLayer;

    void Start()
    {
        GetComponent<SpriteRenderer>().sortingOrder = (int)(transform.position.y + 0.5f) * -1; // y의 위치를 확인하고 이미지를 정렬시켜주는 구문     
    }

    private void Update()
    {
        CheckAndSetActive();
    }



    public void OpenDoor()
    {
        gameObject.SetActive(false);
    }

    private void CheckAndSetActive()
    {
        Vector2 currentPosition = transform.position;

        //2
        //bool hasObjectAbove = CheckForObject(currentPosition + Vector2.up);
        //bool hasObjectBelow = CheckForObject(currentPosition + Vector2.down);
        //bool hasObjectLeft = CheckForObject(currentPosition + Vector2.left);
        //bool hasObjectRight = CheckForObject(currentPosition + Vector2.right);

        //if (!hasObjectAbove && !hasObjectBelow && !hasObjectLeft && !hasObjectRight)
        //{
        //    gameObject.SetActive(false);
        //}

        //따로 변수선언을 하지않고는 아래와같다
        if (!CheckForObject(currentPosition + Vector2.up) && !CheckForObject(currentPosition + Vector2.down) 
            && !CheckForObject(currentPosition + Vector2.left) && !CheckForObject(currentPosition + Vector2.right))
        {
            gameObject.SetActive(false);
        }
    }

    private bool CheckForObject(Vector2 position)
    {
        //1
        //overlap 메서드는 특정 범위 안에 있는 충동체를 감지하는데 사용 마지막에 사용된
        //objectLayer 는 안써도됨 모든 collider2D 물체를 기본적으로 확인한다 나를뺀 다른 콜라이더 검사
        //뭐가 더효율적인..?

        //Collider2D coll = Physics2D.OverlapBox(position, new Vector2(), 0f, objectLayer);
        Collider2D coll = Physics2D.OverlapBox(position, new Vector2(), 0f);

        //OverlapBox는 오브젝트위치, size angle layermask를 사용해야한다
        //오브젝트위치는 문의 위치이고
        //사이즈는 overlapbox의 범위라는데 크게넣으면 바로앞옆위아래 1칸을 확인할수없으므로 1을넣어보기도 0을넣어보기도했는데
        //이게 오브젝트의 정가운데서 0.5 0.5씩할당해서 1의 크기여서 문제는 없으나 확실한 확인을위해 0을넣어도 보고 아무것도
        //안넣기도 해봣는데 되더라 뭐가 더좋은지 모르겠으나 결과는 같은거로 확인
        //angle은 각도라는데 잘모르겠다.. 0으로 그냥주기했다 각도를 넣을필요가
        //없다 그후는 layermask를 확인하는건데 문은 게임상에서 벽 오브젝트가 없다면 제거인데
        //이거? 혹시나 몬스터가 벽이부서짐과 동시에오면 문이안부서지나..? 몬스터
        //콜라이더 유무가 중요할꺼같다


        //Collider2D[] coll = Physics2D.OverlapBoxAll(position, new Vector2(2f, 1f), 0f);
        //이구문은 아직공부중
        return coll != null;
    }

}
