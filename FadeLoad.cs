using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FadeLoad : MonoBehaviour
{
    public bool startFade = true;
    public float fadeTime = 2.0f;
    public Color fadeColor;
    private Renderer rend;
    // Start is called before the first frame update
    void Start()
    {
        rend = GetComponent<Renderer>();
        if(SceneManager.GetActiveScene().buildIndex == 0)
        {
            StartCoroutine(LoadIntro());
        }
        else if (startFade)
            FadeIn();
    }

    private IEnumerator LoadIntro()
    {
        yield return new WaitForSeconds(3);
        if (startFade)
            FadeIn();
    }

    public void FadeIn()
    {
        Fade(1, 0);
    }

    public void FadeOut()
    {
        Fade(0, 1);
    }
    public void Fade(float ain,  float aout)
    {
        StartCoroutine(FadeR(ain, aout));
    }

    public IEnumerator FadeR(float ain, float aout)
    {
        float timer = 0;
        while (timer <= fadeTime)
        {
            Color newColor = fadeColor;
            newColor.a = Mathf.Lerp(ain, aout, timer / fadeTime);
            rend.material.SetColor("_Color", newColor);
            timer += Time.deltaTime;
            yield return null;
        }
        Color newColor2 = fadeColor;
        newColor2.a = aout;
        rend.material.SetColor("_Color", newColor2);
    }
}
