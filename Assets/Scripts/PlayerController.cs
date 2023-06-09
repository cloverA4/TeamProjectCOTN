using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    SpriteRenderer _spriter; // ���� ����� �ʱ�ȭ�ϱ�
    [SerializeField] LayerMask _layerMask;

    int shovelPower = 1;

    // Start is called before the first frame update
    void Start()
    {
        _spriter = GetComponent<SpriteRenderer>(); // ���������� �������� �Լ�
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow)) // �� ȭ��ǥ�� �Է� �޾�����
        {
            MoveCharacter(Vector3.up);
        }

        if (Input.GetKeyDown(KeyCode.DownArrow)) // �Ʒ� ȭ��ǥ�� �Է� �޾�����
        {
            MoveCharacter(Vector3.down);
        }

        if (Input.GetKeyDown(KeyCode.RightArrow)) // ������ ȭ��ǥ�� �Է� �޾�����
        {
            MoveCharacter(Vector3.right);
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow)) // ���� ȭ��ǥ�� �Է� �޾�����
        {
            MoveCharacter(Vector3.left);
        }
    }

    void MoveCharacter(Vector3 vec)
    {
        Vector3 Temp = transform.position + new Vector3(0, -0.5f, 0);
        RaycastHit2D hitdata = Physics2D.Raycast(Temp, vec, 1f, _layerMask);
        // �������� ������� 
        _spriter.flipX = false;

        Debug.Log(transform.position);
        if (hitdata)
        {
            if (hitdata.collider.tag == "WeedWall") // weedwall�� ������Ÿ�� �±׷� ���Ӵٸ�
            {
                //Debug.Log(hitdata.collider.gameObject); // ������Ÿ�ݶ��̴����ӿ�����Ʈ�� ���� ������ ��µȴ�
                //Destroy(hitdata.collider.gameObject); // ������Ÿ�ݶ��̴����ӿ�����Ʈ�� �ı��Ѵ�
                //setActiveȰ���ؼ� ���μ��� ǥ���غ���
                hitdata.collider.GetComponent<Wall>().DamageWall(shovelPower);

            }
            else if (hitdata.collider.tag == "Door") // Door��(��) ������Ÿ�� �±׷� ���Ӵٸ�
            {

                hitdata.collider.GetComponent<Door>().OpenDoor();

            }
            else if (hitdata.collider.tag == "BadRock") // weedwall�� ������Ÿ�� �±׷� ���Ӵٸ�
            {
                //hitdata.collider.GetComponent<Door>().InvincibilityWall();
            }
            //else if(hitdata.collider.tag == "���±��̸�")
            //{
            //    ���ݿ��ϸ��̼�
            //    ����
            //    ���� ü���� ������
            //    ���� ü���� 0�̉�����
            //    ���� ������Ʈ�� �μ���?
            //}
        }
        else
        {
            transform.position += vec;
        }
    }
}
