using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fog : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Texture2D txt = new Texture2D(5000, 5000);
        txt.Apply();

        Sprite sprite = Sprite.Create(txt, new Rect(0, 0, 5000, 5000), new Vector2(0.5f, 0.5f));
        GetComponent<SpriteRenderer>().sprite = sprite;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
