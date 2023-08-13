using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Data : MonoBehaviour
{
    #region ��������

    SaveDataList _saveDataList;

    
    #endregion

    private static Data instance = null;
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

    //���� �Ŵ��� �ν��Ͻ��� ������ �� �ִ� ������Ƽ. static�̹Ƿ� �ٸ� Ŭ�������� ���� ȣ���� �� �ִ�.
    public static Data Instance
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

    private void Start()
    {
        StartCoroutine(LoadGame());
    }


    public Item SearchItem(int itemID)
    {
        Item _tempItem;
        return null;
    } // ������ Ǯ���� ������ID�� ã�Ƽ� ������ ���� �޾ƿ��� �Լ�

    #region GameDataLoad

    public event EventHandler LoadingEnd;
    AsyncOperation asyncOperation;

    public IEnumerator LoadGame()
    {
        // ���� ���࿡ �ʿ��� ���� ������ �Լ� ������� �ε�


        // �������� �ʿ��� ��� ���ӵ����͸� �ε尡�Ǹ� gmaeScene�ε�
        asyncOperation = SceneManager.LoadSceneAsync("GameScene");
        asyncOperation.allowSceneActivation = false;
        
        while(true)
        {
            if(asyncOperation.progress >= 0.9f)
            {
                yield return new WaitForSeconds(3f);
                LoadingEnd?.Invoke(this, EventArgs.Empty);
                break;
            }
            yield return null;
        }
    }

    public void SceneChange()
    {
        StopCoroutine(LoadGame());
        asyncOperation.allowSceneActivation = true;
    }

    public void LoadItemData() // loadingScene���� ȣ��
    {
        // ������ ������ json���� ȣ��
    }

    void LoadSaveData()
    {
        // ���̺� ����Ÿ�� ������� ���̺� ����Ÿ ȣ�� �� ���ε�

        // ���̺� ����Ÿ�� ���� ��� �⺻�� �������ִ� �Լ� ȣ�� �� �� �ε�
    }
   

    #endregion

}

#region Data Ŭ������ ����

public class StageStartPosition
{
    public Vector3 LobbyPosition = new Vector3(-28, 100, 0);
    public Vector3 Stage1F1 = new Vector3(-0, 99, 0);
    public Vector3 Stage1F2 = new Vector3(55, 108, 0);
    public Vector3 Stage1F3 = new Vector3(111, 89, 0);
    public Vector3 Stage1FBoss = new Vector3(169, 86, 0);
}

[Serializable]
public class Item
{
    int _ItemID;

    public int ItemID
    {
        get { return _ItemID; }
        set
        {
            _ItemID = value;
        }
    }

    string _Name;
    public string Name
    {
        get { return _Name; }
        set
        {
            _Name = value;
        }
    }

    Sprite _itemImage;
    public ItemType _itemType = ItemType.Null;
}

public class SaveDataList
{
    List<SaveData> _saveList = new List<SaveData>();
}
public class SaveData
{
    //�÷��̾� ����
    float _nowHP;
    float _maxHP;
    Vector3 _nowPosition = Vector3.zero;

    public SaveData(float NowHP, float MaxHP, Vector3 vec)
    {
        _nowHP = NowHP;
        _maxHP = MaxHP;
        _nowPosition = vec;
    }
}

#endregion

#region Enum����
public enum ItemType
{
    Null,
    Shovel,
    Weapon,
    Armor,
    Potion,
}

public enum CharacterState
{
    Live,
    Death,
}

public enum Stage
{
    Lobby = 0,
    Stage1 = 1,
    Stage2 = 2,
}

public enum floor
{
    f1 = 0,
    f2 = 1,
    f3 = 2,    
    fBoss = 3,
}

#endregion


