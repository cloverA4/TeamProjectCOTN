using Mono.Cecil;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;


public class Wall : MonoBehaviour
{
    Sprite _attackedWall;
    public int _hp = 2;
    //public GameObject _gameObject;


    private SpriteRenderer _spriteRenderer;
    void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        GetComponent<SpriteRenderer>().sortingOrder = (int)(transform.position.y + 0.5f) * -1;
        _attackedWall = Resources.Load<Sprite>("Wall/isometric_pixel_flat_0074");

    }

    public void DamageWall(int ADWall)
    {
        //spriteRenderer�� ��������Ʈ�� _attackedWall(������������ ��)���� �ٲ��ִ°�
        _spriteRenderer.sprite = _attackedWall;

        _hp -= ADWall; //���� ����ü�¿��� ADWall ��ŭ���ֱ�
        Debug.Log(_hp);
        if (_hp <= 0) //���� _hp�� ���� 0�̶�� ���� ������Ʈ�� ��Ȱ��ȭ�ϱ�
        {
            Vector2 currentPosition = transform.position;
            Collider2D coll = Physics2D.OverlapBox(currentPosition + Vector2.up, new Vector2(), 0f);
            if (coll != null) CheckDoor(coll);            

            coll = Physics2D.OverlapBox(currentPosition + Vector2.down, new Vector2(), 0f);
            if (coll != null) CheckDoor(coll);

            coll = Physics2D.OverlapBox(currentPosition + Vector2.right, new Vector2(), 0f);
            if (coll != null) CheckDoor(coll);

            coll = Physics2D.OverlapBox(currentPosition + Vector2.left, new Vector2(), 0f);
            if (coll != null) CheckDoor(coll);

            gameObject.SetActive(false);
        }

        void CheckDoor(Collider2D col)
        {
            if (col.tag == "Door")
            {
                col.GetComponent<Door>().updateWallCount();
            }
        }
    }
}

