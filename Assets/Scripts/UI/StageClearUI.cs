using UnityEngine;

public class StageClearUI : MonoBehaviour
{
    [SerializeField] GameObject _clearText;
    [SerializeField] ParticleSystem _clearPE;

    public void Start()
    {
        _clearPE.gameObject.SetActive(false);
        _clearText.SetActive(false);
    }
    public void OnClearEffect()
    {
        _clearText.SetActive(true);
        _clearPE.gameObject.SetActive(true);
        _clearPE.Play();
        Invoke("OffClearEffect", 3f);
    }
    void OffClearEffect()
    {
        _clearText.SetActive(false);
        _clearPE.gameObject.SetActive(false);
    }
}
