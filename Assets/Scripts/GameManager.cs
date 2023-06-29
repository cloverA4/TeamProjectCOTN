using System;
using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    private static GameManager instance = null;
    [SerializeField] GameObject _dropItem;
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
    }

    void CreateItem(Transform tf)
    {
        GameObject creatrItem = Instantiate(_dropItem, tf);
        //creatrItem.GetComponent<DropItem>().Init();
    }

    //게임 매니저 인스턴스에 접근할 수 있는 프로퍼티. static이므로 다른 클래스에서 맘껏 호출할 수 있다.
    public static GameManager Instance
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


    // Start is called before the first frame update
    void Start()
    {
        CreateNote();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            //스테이지1 시작을 가정하고 - 여기서 사운드오픈 및 노트생성 테스트
            StartCoroutine(Metronom());
        }
        if (Input.GetMouseButtonDown(1))
        {
            //스테이지1 시작을 가정하고 - 여기서 사운드오픈 및 노트생성 테스트
            resetNote();
            StopCoroutine(Metronom());
        }
    }

    #region 비트

    [SerializeField] float _bpm;
    [SerializeField] AudioSource _audio;
    [SerializeField] GameObject _note;
    [SerializeField] GameObject _notePool;
    List<GameObject> _activeNoteList = new List<GameObject>();
    List<GameObject> _pools = new List<GameObject>();

    IEnumerator Metronom()
    {
        float beatTime = 60 / _bpm;
        _audio.Play(); // 노래시작을 첫노트가 도착할때 하게끔 해야함
        while (_audio.isPlaying) // 여기 조건을 노래중이 아니라 다른거로 바꿔야함
        {
            foreach (GameObject prefab in _pools)
            {
                if (!prefab.activeSelf)
                {
                    prefab.GetComponent<Note>().PlayNote();
                    _activeNoteList.Add(prefab);
                    break;
                }
            }
            yield return new WaitForSeconds(beatTime);
        }
        
        //첫 노트가 판정선에 도착할때에 맞춰서 노래를 시작하게끔 어떻게 만들것인가?
        //판정시간과의 싱크는 어떻게 맞출것인가?
    }

    void CreateNote() // 풀링용 노트 미리만들기
    {
        for(int i = 0; i < 20; i++)
        {
            GameObject go = Instantiate(_note, _notePool.transform);
            go.GetComponent<Note>().Init();
            _pools.Add(go);
        }
    }

    void resetNote() // 노래중지, 노트전부 끄기
    {
        _audio.Stop();
        for (int i = 0; i < _pools.Count; i++)
        {
            _pools[i].SetActive(false);
        }
        _activeNoteList.Clear();
    }

    public void ActiveNoteRemove(GameObject note) //활성노트 리스트에서 제거
    {
        _activeNoteList.Remove(note);
    }

    public bool IsSuccess()
    {
        if (_activeNoteList.Count <= 0)
        {
            return false;
            Debug.Log("실패");
        }
        
        RectTransform rec = _activeNoteList[0].GetComponent<RectTransform>();
        if (rec != null && rec.anchoredPosition.x < 200)
        {
            Debug.Log("성공");
            return true;
        }
        return false;
        Debug.Log("실패");
    }

    #endregion
}


/*
public class Pulse : MonoBehaviour
{
    [SerializeField] float _pulseSize = 1.5f;
    [SerializeField] float _returnSpeed = 5f;
    private Vector3 _startSize;

    private void Start()
    {
        _startSize = transform.localScale;
    }

    void Update()
    { // for smooth pulse movement of the object (선형 보간)
        transform.localScale = Vector3.Lerp(transform.localScale, _startSize, Time.deltaTime * _returnSpeed);
    }

    public void PulseinSize()
    {  //changing size of the object
        transform.localScale = _startSize * _pulseSize;
    }
}
*/