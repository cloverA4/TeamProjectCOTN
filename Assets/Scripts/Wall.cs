
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
        /////////////////-2 * -1 2? �� 1�̳��� ?????

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

