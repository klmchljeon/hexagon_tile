using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneChange : MonoBehaviour
{
    [SerializeField]
    protected string sceneName;

    protected virtual void OnEnable()
    {
        GetComponent<Button>().onClick.AddListener(SceneChangeFunc);
    }
    protected virtual void OnDisable()
    {
        GetComponent<Button>().onClick.RemoveListener(SceneChangeFunc);
    }

    protected virtual void SceneChangeFunc()
    {
        SceneManager.LoadScene(sceneName);
    }
}
