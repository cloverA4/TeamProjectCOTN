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
    }

    void CreateItem(Transform tf)
    {
        GameObject creatrItem = Instantiate(_dropItem, tf);
        //creatrItem.GetComponent<DropItem>().Init();
    }

    //���� �Ŵ��� �ν��Ͻ��� ������ �� �ִ� ������Ƽ. static�̹Ƿ� �ٸ� Ŭ�������� ���� ȣ���� �� �ִ�.
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
            //��������1 ������ �����ϰ� - ���⼭ ������� �� ��Ʈ���� �׽�Ʈ
            StartCoroutine(Metronom());
        }
        if (Input.GetMouseButtonDown(1))
        {
            //��������1 ������ �����ϰ� - ���⼭ ������� �� ��Ʈ���� �׽�Ʈ
            resetNote();
            StopCoroutine(Metronom());
        }
    }

    #region ��Ʈ

    [SerializeField] float _bpm;
    [SerializeField] AudioSource _audio;
    [SerializeField] GameObject _note;
    [SerializeField] GameObject _notePool;
    List<GameObject> _activeNoteList = new List<GameObject>();
    List<GameObject> _pools = new List<GameObject>();

    IEnumerator Metronom()
    {
        float beatTime = 60 / _bpm;
        _audio.Play(); // �뷡������ ù��Ʈ�� �����Ҷ� �ϰԲ� �ؾ���
        while (_audio.isPlaying) // ���� ������ �뷡���� �ƴ϶� �ٸ��ŷ� �ٲ����
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
        
        //ù ��Ʈ�� �������� �����Ҷ��� ���缭 �뷡�� �����ϰԲ� ��� ������ΰ�?
        //�����ð����� ��ũ�� ��� ������ΰ�?
    }

    void CreateNote() // Ǯ���� ��Ʈ �̸������
    {
        for(int i = 0; i < 20; i++)
        {
            GameObject go = Instantiate(_note, _notePool.transform);
            go.GetComponent<Note>().Init();
            _pools.Add(go);
        }
    }

    void resetNote() // �뷡����, ��Ʈ���� ����
    {
        _audio.Stop();
        for (int i = 0; i < _pools.Count; i++)
        {
            _pools[i].SetActive(false);
        }
        _activeNoteList.Clear();
    }

    public void ActiveNoteRemove(GameObject note) //Ȱ����Ʈ ����Ʈ���� ����
    {
        _activeNoteList.Remove(note);
    }

    public bool IsSuccess()
    {
        if (_activeNoteList.Count <= 0)
        {
            return false;
            Debug.Log("����");
        }
        
        RectTransform rec = _activeNoteList[0].GetComponent<RectTransform>();
        if (rec != null && rec.anchoredPosition.x < 200)
        {
            Debug.Log("����");
            return true;
        }
        return false;
        Debug.Log("����");
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
    { // for smooth pulse movement of the object (���� ����)
        transform.localScale = Vector3.Lerp(transform.localScale, _startSize, Time.deltaTime * _returnSpeed);
    }

    public void PulseinSize()
    {  //changing size of the object
        transform.localScale = _startSize * _pulseSize;
    }
}
*/