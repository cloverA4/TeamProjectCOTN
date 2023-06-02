using Mono.Cecil;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Wall : MonoBehaviour
{
    Sprite _attackedWall;
    public int _hp = 2;
    //public GameObject _gameObject;


    private SpriteRenderer _spriteRenderer;
    void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        GetComponent<SpriteRenderer>().sortingOrder = (int)(transform.position.y+0.5f) * -1;
        _attackedWall = Resources.Load<Sprite>("Wall/isometric_pixel_flat_0074");

    }

    public void DamageWall(int ADWall)
    {
        //spriteRenderer의 스프라이트를 _attackedWall(데미지를받은 벽)으로 바꿔주는것
        _spriteRenderer.sprite = _attackedWall;
        
        _hp -= ADWall; //벽의 남은체력에서 ADWall 만큼빼주기
        Debug.Log(_hp);
        if (_hp <= 0) //만약 _hp의 값이 0이라면 게임 오브젝트를 비활성화하기
        {
            gameObject.SetActive(false);
           
        }
    }
}
