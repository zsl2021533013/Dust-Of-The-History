using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeCanvasManager : Singleton<FadeCanvasManager>
{
    CanvasGroup canvasGroup;

    public float fadeInTime;
    public float fadeOutTime;

    protected override void Awake()
    {
        base.Awake();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public IEnumerator FadeInAndOut()
    {
        yield return FadeIn(fadeInTime);
        yield return FadeOut(fadeOutTime);
    }

    public IEnumerator FadeOutAndIn()
    {
        yield return FadeOut(fadeOutTime);
        yield return FadeIn(fadeInTime);
    }

    public IEnumerator FadeIn(float time)
    {
        while(canvasGroup.alpha > 0)
        {
            canvasGroup.alpha -= Time.deltaTime / time;
            yield return null;
        }
    }

    public IEnumerator FadeOut(float time)
    {
        while (canvasGroup.alpha < 1)
        {
            canvasGroup.alpha += Time.deltaTime / time;
            yield return null;
        }
    }
}
