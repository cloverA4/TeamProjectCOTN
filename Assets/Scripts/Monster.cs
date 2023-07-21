using UnityEngine;
using System;

public class Monster : MonoBehaviour
{
    

    // Start is called before the first frame update
    void Start()
    {
        GameManager.Instance.MosterMoveEnvent += new EventHandler(MonsterMove);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void MonsterMove(object sender, EventArgs s)
    {

    }
}
