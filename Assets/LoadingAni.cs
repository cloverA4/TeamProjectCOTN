using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingAni : MonoBehaviour
{
    [SerializeField] string[] _texts;
    Text _text;

    private void Start()
    {
        _text = GetComponent<Text>();
        StartCoroutine(CoTextAnim());
    }

    IEnumerator CoTextAnim()
    {
        int i = 0;
        while (true)
        {
            _text.text = _texts[i++];
            if (i >= _texts.Length) i = 0;
            yield return new WaitForSeconds(0.2f);
        }
        yield return null;
    }
}
