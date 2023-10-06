using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHitUI : MonoBehaviour
{
    [SerializeField] Image _hitBackGround;

    private float _hitDuration = 0.2f;    
    private Color flashColor = new Color(1f, 0f, 0f, 0.3f); //�ǰݽ� ������

    public void HitUI()
    {
        StartCoroutine(FlashBackGround());
    }

    IEnumerator FlashBackGround()
    {
        _hitBackGround.color = flashColor; // ��� �̹����� ������ ���������� ����
        yield return new WaitForSeconds(_hitDuration); // ���� �ð� ���� ���
        _hitBackGround.color = Color.clear; // ĵ�������� �������� �ǵ�����
    }
}
