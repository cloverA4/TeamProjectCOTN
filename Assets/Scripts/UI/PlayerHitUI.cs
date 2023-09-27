using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHitUI : MonoBehaviour
{
    [SerializeField] Image _hitBackGround;
    float _hitDuration = 0.2f;

    //피격시 배경색깔
    private Color flashColor = new Color(1f, 0f, 0f, 0.3f);

    public void HitUI()
    {
        StartCoroutine(FlashBackGround());
    }

    IEnumerator FlashBackGround()
    {
        // 배경 이미지의 색상을 빨간색으로 변경
        _hitBackGround.color = flashColor;

        // 일정 시간 동안 대기
        yield return new WaitForSeconds(_hitDuration);

        // 캔버스색깔 투명으로 되돌리기
        _hitBackGround.color = Color.clear;
    }
}
