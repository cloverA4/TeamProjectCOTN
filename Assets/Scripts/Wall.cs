
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;


public class Wall : MonoBehaviour
{
    Sprite _attackedWall;
    int _hp;
    LayerMask objectLayer;
    //public GameObject _gameObject;


    private SpriteRenderer _spriteRenderer;
    void Start()
    {
        _hp = 2;
        _spriteRenderer = GetComponent<SpriteRenderer>();
        GetComponent<SpriteRenderer>().sortingOrder = (int)(transform.position.y + 0.8f) * -1;
        _attackedWall = Resources.Load<Sprite>("Wall/isometric_pixel_flat_0074");
        Debug.Log((int)(-0.3 + 0.3f) * -1); // 0
        Debug.Log((int)(-1.3 + 0.3f) * -1); // 0
        Debug.Log((int)(-2.3 + 0.3f) * -1); // 1
        /////////////////-2 * -1 2? 왜 1이나와 ?????

    }

    public void DamageWall(int ADWall)
    {
        //spriteRenderer의 스프라이트를 _attackedWall(데미지를받은 벽)으로 바꿔주는것
        _spriteRenderer.sprite = _attackedWall;

        _hp -= ADWall; //벽의 남은체력에서 ADWall 만큼빼주기

        if (_hp <= 0) //만약 벽의 _hp의 값이 0이라면 게임 오브젝트를 비활성화하기
        {
            Vector2 currentPosition = transform.position;
            Vector2[] vecs = new Vector2[] { Vector2.up, Vector2.down, Vector2.right, Vector2.left };

            for (int i = 0; i < vecs.Length; i++)
            {
                Collider2D coll = Physics2D.OverlapBox(currentPosition + vecs[i], new Vector2(), 0f);
                if (coll != null && coll.tag == "Door") // collider2D.tag가 "문"이고 null이 아니라면
                {
                    coll.GetComponent<Door>().updateWallCount(); // 충돌체의 문코드의 updateWallCount 함수를 실행한다
                }
            }

            gameObject.SetActive(false); //벽 파괴 구문
        }
    }
}

