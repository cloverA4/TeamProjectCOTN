using System.Collections;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    SpriteRenderer _spriter; // ���� ����� �ʱ�ȭ�ϱ�
    [SerializeField] LayerMask _layerMask;

    int shovelPower = 1;
    bool isLive = true;
    bool _isSuccess = true;
    bool _isDubbleClick = true;

    // Start is called before the first frame update
    void Start()
    {
        _spriter = GetComponent<SpriteRenderer>(); // ���������� �������� �Լ�
    }

    // Update is called once per frame
    void Update()
    {
        if (isLive && _isSuccess && _isDubbleClick)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow)) // �� ȭ��ǥ�� �Է� �޾�����
            {
                Judgement();
                MoveCharacter(Vector3.up);
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow)) // �Ʒ� ȭ��ǥ�� �Է� �޾�����
            {
                Judgement();
                MoveCharacter(Vector3.down);
            }
            else if(Input.GetKeyDown(KeyCode.RightArrow)) // ������ ȭ��ǥ�� �Է� �޾�����
            {
                Judgement();
                MoveCharacter(Vector3.right);
                _spriter.flipX = true;
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow)) // ���� ȭ��ǥ�� �Է� �޾�����
            {
                Judgement();
                MoveCharacter(Vector3.left);
                _spriter.flipX = false;
            }
        }
    }

    void Judgement()
    {
        _isDubbleClick = false;
        Invoke("DubbleLock", 0.1f);

        if (!GameManager.Instance.IsSuccess())
        {
            _isSuccess = false;
            Invoke("penalty", 60 / GameManager.Instance.BPM);
            return;
        }
    }

    void DubbleLock()
    {
        _isDubbleClick = true;
    }
    void penalty()
    {
        Debug.Log("�г�Ƽ ����!");
        _isSuccess = true;
        _isDubbleClick = true;
    }

    void MoveCharacter(Vector3 vec)
    {
        Vector3 Temp = transform.position + new Vector3(0, -0.5f, 0);
        RaycastHit2D hitdata = Physics2D.Raycast(Temp, vec, 1f, _layerMask);
        // �������� ������� 
        
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
