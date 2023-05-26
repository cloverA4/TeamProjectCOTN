using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class IdleController : MonoBehaviour
{
    Rigidbody2D _rigid; // �������� ����� �ʱ�ȭ
    SpriteRenderer _spriter; // ���� ����� �ʱ�ȭ�ϱ�
    Animator _anim; // �ִϸ��̼� ����� �ʱ�ȭ
    Transform _idle;

    [SerializeField]
    LayerMask _layerMask;

    [SerializeField]
    LayerMask _layerMask2;

    int shovelPower = 1;



    // Start is called before the first frame update
    void Start()
    {
        _idle = GetComponent<Transform>();
        _rigid = GetComponent<Rigidbody2D>(); // ������Ʈ�� �������� �Լ�
        _spriter = GetComponent<SpriteRenderer>(); // ���������� �������� �Լ�
        _anim = GetComponent<Animator>(); // Animator ���� ����� �ʱ�ȭ�ϱ�






    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.UpArrow)) // �� ȭ��ǥ�� �Է� �޾�����
        {
            Vector3 Temp = transform.position + new Vector3(0, -0.5f, 0);
            RaycastHit2D hitdata = Physics2D.Raycast(Temp, Vector3.up, 1f, _layerMask);
            
            //���̸� �߻��ϴ� ��ġ

            Debug.Log(transform.position);
            // �������� ����������
            if (hitdata) // �����i����
            {
                
                if (hitdata.collider.tag == "WeedWall") // weedwall�� ������Ÿ�� �±׷� ���Ӵٸ�
                {
                    //Debug.Log(hitdata.collider.gameObject);
                    //Destroy(hitdata.collider.gameObject);
                    hitdata.collider.GetComponent<Wall>().DamageWall(shovelPower);

                }

                else if (hitdata.collider.tag == "Door") // Door��(��) ������Ÿ�� �±׷� ���Ӵٸ�
                {
                    
                    hitdata.collider.GetComponent<Door>().OpenDoor();

                }

                else if (hitdata.collider.tag == "BadRock") // weedwall�� ������Ÿ�� �±׷� ���Ӵٸ�
                {
                    
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
                _idle.position += Vector3.up;
            }
           
        }

        if (Input.GetKeyDown(KeyCode.DownArrow)) // �Ʒ� ȭ��ǥ�� �Է� �޾�����
        {
            Vector3 Temp = transform.position + new Vector3(0, -0.5f, 0);
            RaycastHit2D hitdata = Physics2D.Raycast(Temp, Vector3.down, 1f, _layerMask);
            // �Ʒ������� ������� 

            
            if (hitdata) // �����i����
            {
                //Debug.Log(hitdata.collider.gameObject.name);
                if (hitdata.collider.tag == "WeedWall") // weedwall�� ������Ÿ�� �±׷� ���Ӵٸ�
                {
                    //Debug.Log(hitdata.collider.gameObject);
                    //Destroy(hitdata.collider.gameObject);
                    hitdata.collider.GetComponent<Wall>().DamageWall(shovelPower);

                }
                else if (hitdata.collider.tag == "Door") // Door��(��) ������Ÿ�� �±׷� ���Ӵٸ�
                {

                    hitdata.collider.GetComponent<Door>().OpenDoor();

                }

                else if (hitdata.collider.tag == "BadRock") // weedwall�� ������Ÿ�� �±׷� ���Ӵٸ�
                {

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
                _idle.position += Vector3.down;
            }
        }

        if (Input.GetKeyDown(KeyCode.RightArrow)) // ������ ȭ��ǥ�� �Է� �޾�����
        {
            Vector3 Temp = transform.position + new Vector3(0, -0.5f, 0);
            RaycastHit2D hitdata = Physics2D.Raycast(Temp, Vector3.right, 1f, _layerMask);
            // ���������� ������� 

            Debug.Log(transform.position);
            if (hitdata)
            {
                if (hitdata.collider.tag == "WeedWall") // weedwall�� ������Ÿ�� �±׷� ���Ӵٸ�
                {
                    //Debug.Log(hitdata.collider.gameObject);
                    //Destroy(hitdata.collider.gameObject);
                    hitdata.collider.GetComponent<Wall>().DamageWall(shovelPower);

                }

                else if (hitdata.collider.tag == "Door") // Door��(��) ������Ÿ�� �±׷� ���Ӵٸ�
                {

                    hitdata.collider.GetComponent<Door>().OpenDoor();

                }

                else if (hitdata.collider.tag == "BadRock") // weedwall�� ������Ÿ�� �±׷� ���Ӵٸ�
                {

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
                _idle.position += Vector3.right;
                _spriter.flipX = true;
            }

        }

        if (Input.GetKeyDown(KeyCode.LeftArrow)) // ���� ȭ��ǥ�� �Է� �޾�����
        {
            Vector3 Temp = transform.position + new Vector3(0, -0.5f, 0);
            RaycastHit2D hitdata = Physics2D.Raycast(Temp, Vector3.left, 1f, _layerMask);
            // �������� ������� 

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
                _idle.position += Vector3.left;
                _spriter.flipX = false;
            }
        }

    


    }


    


}
