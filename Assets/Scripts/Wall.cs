
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;


public class Wall : MonoBehaviour
{
    Sprite _normalWall;
    Sprite _attackedWall;
    int _hp;
    int _wallMaxHp = 2;

    private SpriteRenderer _spriteRenderer;
    //LayerMask objectLayer;
    //public GameObject _gameObject;



    void Start()
    {
        _hp = _wallMaxHp;
        _spriteRenderer = GetComponent<SpriteRenderer>();
        GetComponent<SpriteRenderer>().sortingOrder = (int)transform.position.y * -1;

        _attackedWall = Resources.Load<Sprite>("Wall/isometric_pixel_flat_0074");
    }

    

    public void WallReset()
    {
        _normalWall = Resources.Load<Sprite>("Wall/WeedWallVer3");
        _spriteRenderer.sprite = _normalWall;
        _hp = _wallMaxHp;
        gameObject.SetActive(true);
    }

    public void DamageWall(int ADWall)
    {
        //spriteRenderer�� ��������Ʈ�� _attackedWall(������������ ��)���� �ٲ��ִ°�
        _spriteRenderer.sprite = _attackedWall;

        _hp -= ADWall; //���� ����ü�¿��� ADWall ��ŭ���ֱ�

        if (_hp <= 0) //���� ���� _hp�� ���� 0�̶�� ���� ������Ʈ�� ��Ȱ��ȭ�ϱ�
        {
            Vector2 currentPosition = transform.position;
            Vector2[] vecs = new Vector2[] { Vector2.up, Vector2.down, Vector2.right, Vector2.left };

            for (int i = 0; i < vecs.Length; i++)
            {
                Collider2D coll = Physics2D.OverlapBox(currentPosition + vecs[i], new Vector2(), 0f);
                if (coll != null && coll.tag == "Door") // collider2D.tag�� "��"�̰� null�� �ƴ϶��
                {
                    coll.GetComponent<Door>().updateWallCount(); // �浹ü�� ���ڵ��� updateWallCount �Լ��� �����Ѵ�
                }
            }

            gameObject.SetActive(false); //�� �ı� ����
        }
    }
}

