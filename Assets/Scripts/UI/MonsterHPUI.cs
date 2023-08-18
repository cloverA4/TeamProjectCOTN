using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterHPUI : MonoBehaviour
{
    [SerializeField] Transform _hpBase;
    [SerializeField] GameObject _heart;
    int _maxHP = 0;
    int _nowHP = 0;
    public void Init(int hp)
    {
        _maxHP = _nowHP = hp;
        setMaxHP();
        gameObject.SetActive(false);
    }
    
    List<GameObject> _hearts = new List<GameObject>();  
    public void setMaxHP()
    {
        for (int i = 0; i < _maxHP; i++)
        {
            GameObject temp = Instantiate(_heart, _hpBase);
            _heart.GetComponent<MonsterHeart>().FullHeartActive();
            _hearts.Add(temp);
        }
    }

    public void MonsterHPUpdata(int nowHP)
    {
        _nowHP = _maxHP - nowHP;
        if (gameObject.activeSelf == false) gameObject.SetActive(true);
        for(int i = 0; i < _nowHP; i++)
        {
            _hearts[i].GetComponent<MonsterHeart>().EmptyHeartActive();
        }
    }
}
