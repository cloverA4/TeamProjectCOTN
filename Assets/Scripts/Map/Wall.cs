using UnityEngine;

public class Wall : MonoBehaviour
{
    [SerializeField] Sprite _normalWall;
    [SerializeField] Sprite _attackedWall;
    [SerializeField] SpriteRenderer _spriteRenderer;

    int _hp;
    int _wallMaxHp = 2;

    private void Awake()
    {
        _hp = _wallMaxHp;
        _spriteRenderer.GetComponent<SpriteRenderer>().sortingOrder = (int)(transform.position.y - 1) * -1;
    }    

    public void WallReset()
    {
        _spriteRenderer.GetComponent<SpriteRenderer>().sortingOrder = (int)(transform.position.y - 1) * -1;
        _spriteRenderer.sprite = _normalWall;
        _hp = _wallMaxHp;
        gameObject.SetActive(true);
    }

    public void DamageWall(int ADWall)
    {
        _spriteRenderer.sprite = _attackedWall; //spriteRenderer�� ��������Ʈ�� _attackedWall(������������ ��)���� �ٲ��ִ°�

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

