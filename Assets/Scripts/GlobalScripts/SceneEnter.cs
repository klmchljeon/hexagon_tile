using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneEnter : SceneChange
{
    protected override void OnEnable()
    {

    }

    protected override void OnDisable()
    {

    }

    protected override void Start()
    {
        TransitionToScene();
    }

    protected override void TransitionToScene()
    {
        StartCoroutine(FadeInAndEnterScene());
    }
}
