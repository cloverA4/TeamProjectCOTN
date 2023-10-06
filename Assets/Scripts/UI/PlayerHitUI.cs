using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHitUI : MonoBehaviour
{
    [SerializeField] Image _hitBackGround;

    private float _hitDuration = 0.2f;    
    private Color flashColor = new Color(1f, 0f, 0f, 0.3f); //피격시 배경색깔

    public void HitUI()
    {
        StartCoroutine(FlashBackGround());
    }

    IEnumerator FlashBackGround()
    {
        _hitBackGround.color = flashColor; // 배경 이미지의 색상을 빨간색으로 변경
        yield return new WaitForSeconds(_hitDuration); // 일정 시간 동안 대기
        _hitBackGround.color = Color.clear; // 캔버스색깔 투명으로 되돌리기
    }
}
