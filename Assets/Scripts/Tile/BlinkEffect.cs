using UnityEngine;

public class BlinkEffect : MonoBehaviour
{
    private Renderer rend;
    private Color color;
    private float alpha;
    private bool fadingOut = true;
    public float fadeSpeed = 0.6f;

    void OnEnable()
    {
        rend = GetComponent<Renderer>();
        color = rend.material.color;
        alpha = 80f / 255f;

        color.a = alpha;
        rend.material.color = color;
    }

    void Update()
    {
        if (fadingOut)
        {
            alpha += fadeSpeed * Time.deltaTime;
            if (alpha >= 150f / 255f) 
            {
                alpha = 150f / 255f;
                fadingOut = false;
            }
        }
        else
        {
            alpha -= fadeSpeed * Time.deltaTime;
            if (alpha <= 80f / 255f) 
            {
                alpha = 80f / 255f;
                fadingOut = true;
            }
        }

        color.a = alpha;
        rend.material.color = color;
    }
}
