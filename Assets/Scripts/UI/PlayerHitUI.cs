using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHitUI : MonoBehaviour
{
    [SerializeField] Image _hitBackGround;
    float _hitDuration = 0.2f;

    //�ǰݽ� ������
    private Color flashColor = new Color(1f, 0f, 0f, 0.3f);

    public void HitUI()
    {
        StartCoroutine(FlashBackGround());
    }

    IEnumerator FlashBackGround()
    {
        // ��� �̹����� ������ ���������� ����
        _hitBackGround.color = flashColor;

        // ���� �ð� ���� ���
        yield return new WaitForSeconds(_hitDuration);

        // ĵ�������� �������� �ǵ�����
        _hitBackGround.color = Color.clear;
    }
}
