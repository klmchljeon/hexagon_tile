using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class IntListWrapper
{
    public List<int> intList;

    public IntListWrapper(List<int> list)
    {
        intList = list;
    }
}

public class StarManager : MonoBehaviour
{
    public static StarManager instance;

    private string filePath;

    public List<int> starList = new List<int>();

    public static StarManager _instance;

    public static StarManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindAnyObjectByType(typeof(StarManager)) as StarManager;

                if (_instance == null)
                {
                    if (applicationIsQuitting)
                    {
                        return null;
                    }

                    GameObject singletonObject = new GameObject();
                    _instance = singletonObject.AddComponent<StarManager>();
                    singletonObject.name = typeof(StarManager).ToString();
                    DontDestroyOnLoad(singletonObject);
                }
            }

            return _instance;
        }
    }

    private static bool applicationIsQuitting = false;
    public void OnDestroy()
    {
        applicationIsQuitting = true;
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        filePath = Path.Combine(Application.persistentDataPath, "starData.json");
        LoadData();
    }

    void LoadData()
    {
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            IntListWrapper deserializedWrapper = JsonUtility.FromJson<IntListWrapper>(json);
            starList = deserializedWrapper.intList;
        }
        else
        {
            starList = new List<int>();
        }
    }

    void SaveData()
    {
        IntListWrapper wrapper = new IntListWrapper(starList);
        string json = JsonUtility.ToJson(wrapper);
        File.WriteAllText(filePath, json);
    }

    public void Clear(int stageNum, int star)
    {
        while (starList.Count <= stageNum)
        {
            starList.Add(0);
        }

        starList[stageNum] = Mathf.Max(starList[stageNum], star);
        SaveData();
    }
}
