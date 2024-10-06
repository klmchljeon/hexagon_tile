using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundClick : MonoBehaviour
{
    private void OnEnable()
    {
        GetComponent<Button>().onClick.AddListener(Play);
    }

    private void OnDisable()
    {
        GetComponent<Button>().onClick.RemoveListener(Play);
    }

    protected virtual void Play()
    {
        SoundManager.instance.PlaySound(GetComponent<AudioSource>().clip, GetComponent<AudioSource>(), true);
    }
}
