using System.Collections.Generic;
using UnityEngine;

public class MonsterHPUI : MonoBehaviour
{
    [SerializeField] Transform _hpBase;
    [SerializeField] GameObject _heart;
    int _maxHP = 0;
    int _nowHP = 0;
    List<GameObject> _hearts = new List<GameObject>();

    public void Init(int hp)
    {
        _maxHP = _nowHP = hp;
        setMaxHP();
        gameObject.SetActive(false);
    }
    
    public void setMaxHP()
    {
        for (int i = 0; i < _maxHP; i++)
        {
            GameObject temp = Instantiate(_heart, _hpBase);
            _heart.GetComponent<MonsterHeart>().FullHeartActive();
            _hearts.Add(temp);
        }
    }
    void HPAllDisabel()
    {
        for(int i = 0; i < _hearts.Count; i++)
        {
            _hearts[i].GetComponent<MonsterHeart>().EmptyHeartActive();
        }
    }
    public void MonsterHPUpdata(int dmg)
    {
        HPAllDisabel();
        _nowHP -= dmg;
        if (_nowHP < 0) _nowHP = 0;
        if (gameObject.activeSelf == false) gameObject.SetActive(true);
        for(int i = 0; i < _nowHP; i++)
        {
            _hearts[i].GetComponent<MonsterHeart>().FullHeartActive();
        }
    }
}
