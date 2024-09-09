using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundClose : SoundClick
{
    public GameObject panel;
    
    protected override void Play()
    {
        SoundManager.instance.PlaySound(GetComponent<AudioSource>().clip, GetComponent<AudioSource>(), true);

        Invoke("wrapper", 0.2f);
    }

    private void wrapper()
    {
        panel.SetActive(false);
    }
}
