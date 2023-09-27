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

            //씬 전환이 되더라도 파괴되지 않게 한다.
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            //만약 씬 이동이 되었는데 그 씬에도 Hierarchy에 GameMgr이 존재할 수도 있다.
            //그럴 경우엔 이전 씬에서 사용하던 인스턴스를 계속 사용해주는 경우가 많은 것 같다.
            //그래서 이미 전역변수인 instance에 인스턴스가 존재한다면 자신을 삭제해준다.
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
