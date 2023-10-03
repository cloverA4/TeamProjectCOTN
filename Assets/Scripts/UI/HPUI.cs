using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPUI : MonoBehaviour
{
    [SerializeField]
    GameObject[] Hearts;
    public void setHP()
    {
        PlayerController pc = PlayerController.Instance;
        ResetHP();

        int damge = pc.MaxHP - pc.NowHP;

        for (int i = 0; i < pc.MaxHP / 2; i++)
        {
            Hearts[i].gameObject.SetActive(true);
            Hearts[i].transform.Find("FullHeart").gameObject.SetActive(true);
            Hearts[i].transform.Find("HalfHeart").gameObject.SetActive(false);
        }
        for (int i = 0; i < damge / 2; i++)
        {
            Hearts[i].transform.Find("FullHeart").gameObject.SetActive(false);
            Hearts[i].transform.Find("HalfHeart").gameObject.SetActive(false);

        }
        if (damge % 2 == 1)
        {
            Hearts[damge / 2].transform.Find("HalfHeart").gameObject.SetActive(true);
            Hearts[damge / 2].transform.Find("FullHeart").gameObject.SetActive(false);
        }
    }

    void ResetHP()
    {
        for (int i = 0; i < Hearts.Length; i++)
        {
            Hearts[i].gameObject.SetActive(false);
        }
    }
}
