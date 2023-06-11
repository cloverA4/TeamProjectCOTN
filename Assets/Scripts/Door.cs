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
        GetComponent<SpriteRenderer>().sortingOrder = (int)(transform.position.y + 0.5f) * -1; // y�� ��ġ�� Ȯ���ϰ� �̹����� ���Ľ����ִ� ����     
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

        //���� ���������� �����ʰ�� �Ʒ��Ͱ���
        if (!CheckForObject(currentPosition + Vector2.up) && !CheckForObject(currentPosition + Vector2.down) 
            && !CheckForObject(currentPosition + Vector2.left) && !CheckForObject(currentPosition + Vector2.right))
        {
            gameObject.SetActive(false);
        }
    }

    private bool CheckForObject(Vector2 position)
    {
        //1
        //overlap �޼���� Ư�� ���� �ȿ� �ִ� �浿ü�� �����ϴµ� ��� �������� ����
        //objectLayer �� �Ƚᵵ�� ��� collider2D ��ü�� �⺻������ Ȯ���Ѵ� ������ �ٸ� �ݶ��̴� �˻�
        //���� ��ȿ������..?

        //Collider2D coll = Physics2D.OverlapBox(position, new Vector2(), 0f, objectLayer);
        Collider2D coll = Physics2D.OverlapBox(position, new Vector2(), 0f);

        //OverlapBox�� ������Ʈ��ġ, size angle layermask�� ����ؾ��Ѵ�
        //������Ʈ��ġ�� ���� ��ġ�̰�
        //������� overlapbox�� ������µ� ũ�Գ����� �ٷξտ����Ʒ� 1ĭ�� Ȯ���Ҽ������Ƿ� 1���־�⵵ 0���־�⵵�ߴµ�
        //�̰� ������Ʈ�� ������� 0.5 0.5���Ҵ��ؼ� 1�� ũ�⿩�� ������ ������ Ȯ���� Ȯ�������� 0���־ ���� �ƹ��͵�
        //�ȳֱ⵵ �ؔf�µ� �Ǵ��� ���� �������� �𸣰����� ����� �����ŷ� Ȯ��
        //angle�� ������µ� �߸𸣰ڴ�.. 0���� �׳��ֱ��ߴ� ������ �����ʿ䰡
        //���� ���Ĵ� layermask�� Ȯ���ϴ°ǵ� ���� ���ӻ󿡼� �� ������Ʈ�� ���ٸ� �����ε�
        //�̰�? Ȥ�ó� ���Ͱ� ���̺μ����� ���ÿ����� ���̾Ⱥμ�����..? ����
        //�ݶ��̴� ������ �߿��Ҳ�����


        //Collider2D[] coll = Physics2D.OverlapBoxAll(position, new Vector2(2f, 1f), 0f);
        //�̱����� ����������
        return coll != null;
    }

}
