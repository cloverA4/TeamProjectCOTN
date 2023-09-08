using UnityEngine;
using System;
public class TestCamera : MonoBehaviour
{
    private void Start()
    {
        PlayerController.Instance.PlayerMoveEvent += new EventHandler(TestMove);
    }

    public void TestMove(object sender, EventArgs s)
    {

    }
}


