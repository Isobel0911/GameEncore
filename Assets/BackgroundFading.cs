using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class BackgroundFading : MonoBehaviour {
    public Action callbackFunction;
    public Action<float> callFuncDuringProcess;
    private Image fadePanel;

    // Start is called before the first frame update
    void Awake() {
        fadePanel = GetComponent<Image>();
        if (fadePanel == null) {
            fadePanel = this.gameObject.AddComponent<Image>();
        }
    }

    public void FadeTo(Color targetColor, float targetAlpha, float duration) {
        StartCoroutine(DoFade(targetColor, fadePanel.color.a, targetAlpha, duration));
    }

    public void FadeTo(float targetAlpha, float duration) {
        StartCoroutine(DoFade(fadePanel.color, fadePanel.color.a, targetAlpha, duration));
    }

    IEnumerator DoFade(Color targetColor, float startAlpha, float endAlpha, float duration) {
        float time = 0;
        Color startColor = fadePanel.color;

        while (time < duration) {
            time += Time.deltaTime;
            float alpha = Mathf.Lerp(startAlpha,
                                     endAlpha,
                                     time / duration);
            fadePanel.color = Color.Lerp(startColor,
                                         new Color(targetColor.r,
                                                   targetColor.g,
                                                   targetColor.b,
                                                   alpha),
                                         time / duration);
            callFuncDuringProcess?.Invoke(time / duration);
            yield return null;
        }

        // set final color to make sure it was set into precise color value
        fadePanel.color = new Color(targetColor.r, targetColor.g, targetColor.b, endAlpha);

        // Call the callback function if exist
        callbackFunction?.Invoke();
    }
}
