using UnityEngine;

public class InfoMassege : MonoBehaviour
{
    // ��Ȳ�� �´� Text�� �Է��� �ް� �׿� �°� �ؽ�Ʈ�� ��ȯ

    // defult ��ġ�� �÷��̾� ��ġ

    // ���ڰ� ����������� ������ ��ġ


    private void OnEnable() // �Լ��� Ǯ������ Ȱ��ȭ �� �ٷ� �ִϸ��̼� ����
    {
        gameObject.GetComponent<Animator>().SetTrigger("InfoTrigger");
    }
}
