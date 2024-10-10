using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class FrameManager : MonoBehaviour
{
    public static FrameManager instance;

    public static FrameManager _instance;

    public static FrameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindAnyObjectByType(typeof(FrameManager)) as FrameManager;

                if (_instance == null)
                {
                    if (applicationIsQuitting)
                    {
                        return null;
                    }

                    GameObject singletonObject = new GameObject();
                    _instance = singletonObject.AddComponent<FrameManager>();
                    singletonObject.name = typeof(FrameManager).ToString();
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
    }

    private void Start()
    {
        Application.targetFrameRate = 60;
    }
}