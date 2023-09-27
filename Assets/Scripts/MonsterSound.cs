using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSound : MonoBehaviour
{
    private static MonsterSound instance;

    AudioSource _audio;

    private void Awake()
    {
        if (null == instance)
        {
            instance = this;

            //�� ��ȯ�� �Ǵ��� �ı����� �ʰ� �Ѵ�.
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            //���� �� �̵��� �Ǿ��µ� �� ������ Hierarchy�� GameMgr�� ������ ���� �ִ�.
            //�׷� ��쿣 ���� ������ ����ϴ� �ν��Ͻ��� ��� ������ִ� ��찡 ���� �� ����.
            //�׷��� �̹� ���������� instance�� �ν��Ͻ��� �����Ѵٸ� �ڽ��� �������ش�.
            Destroy(this.gameObject);
        }
        _audio = GetComponent<AudioSource>();
    }

    public static MonsterSound Instance
    {
        get
        {
            if (null == instance)
            {
                return null;
            }
            return instance;
        }
    }

    public void MonsterDeath(MonsterType type)
    {
        switch (type)
        {
            case MonsterType.Monster1:
                _audio.clip = Data.Instance.SoundEffect[(int)SoundEffect.SlimeDeath];
                break;
            case MonsterType.Monster2:
                _audio.clip = Data.Instance.SoundEffect[(int)SoundEffect.ZombieDeath];
                break;
            case MonsterType.Monster3:
                _audio.clip = Data.Instance.SoundEffect[(int)SoundEffect.CowDeath];
                break;
            case MonsterType.Monster4:
                _audio.clip = Data.Instance.SoundEffect[(int)SoundEffect.GoblinDeath];
                break;
            case MonsterType.EliteMonster:
                _audio.clip = Data.Instance.SoundEffect[(int)SoundEffect.EliteGoblinDeath];
                break;
        }
        _audio.Play();
    }
}
