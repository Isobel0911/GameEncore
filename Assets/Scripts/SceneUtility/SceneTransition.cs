using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour {
    private BackgroundFading fadingScript;
    private GameObject fadePanel;
    private SceneSounds sceneSoundsScript;
    public static bool isCreditsEnabled = false;
    public static bool isTutorialEnabled = false;
    public static bool isTutorialFul = false;
    public GameObject tutorialPanel;
    private CanvasGroup tutorialFadingScript;

    void Awake() {
        init();
    }

    public void init() {
        tutorialPanel = GameObject.Find("TutorialMainPanel");
        tutorialFadingScript = tutorialPanel?.GetComponent<CanvasGroup>();
        tutorialPanel?.SetActive(false);
        GameObject sceneSoundsGB = GameObject.Find("In-Game Transition");
        if (sceneSoundsGB == null) sceneSoundsGB = new GameObject("In-Game Transition");
        sceneSoundsScript = sceneSoundsGB.GetComponent<SceneSounds>();
        if (sceneSoundsScript == null) sceneSoundsScript = sceneSoundsGB.AddComponent<SceneSounds>();
        fadePanel = GameObject.Find("FadePanel");
        if (fadePanel != null) fadingScript = fadePanel.GetComponent<BackgroundFading>();
    }

    public void init(SceneSounds sceneSoundsScript, GameObject fadePanel, BackgroundFading fadingScript) {
        this.sceneSoundsScript = sceneSoundsScript;
        this.fadePanel = fadePanel;
        this.fadingScript = fadingScript;
    }

    // Triggered by clicking scene transition button
    public void TransitScene(string sceneName) {
        fadePanel?.SetActive(true);
        if (fadingScript != null){
            fadingScript.callbackFunction = () => {
                if (sceneName == "CreditPause") this.sceneChangedAsync(sceneName);
                else this.sceneChanged(sceneName);
            };
            if (sceneName == "MainGame") {
                fadingScript.callFuncDuringProcess = (progress) => {
                    sceneSoundsScript?.SetBGMSoundVolume(1f - progress);
                };
            }
        }
        // from 0% to 100% in 1 second with original color
        fadingScript?.FadeTo(1f, 1f);
    }

    public void sceneChanged(string sceneName) {
        SceneManager.LoadScene(sceneName);
        Time.timeScale = 1f;
    }

    public void sceneChangedAsync(string sceneName) {
        if (sceneName == "CreditPause") {
            isCreditsEnabled = true;
            SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        } else {
            SceneManager.LoadScene(sceneName);
        }
        Time.timeScale = 1f;
        if (sceneName == "CreditPause") {
            StartCoroutine(WaitForCredit(sceneName));
        }
    }

    IEnumerator WaitForCredit(string sceneName) {
        float timePassed = 0f;
        float waitTime = 45.0f;
        bool isMidEnd = false;
        while (timePassed < waitTime) {
            if (Input.anyKeyDown) {
                SceneManager.UnloadSceneAsync(sceneName);
                isMidEnd = true;
                break;
            }
            yield return null;
            timePassed += Time.deltaTime;
        }
        Time.timeScale = 1f;
        if (isMidEnd) {
            fadingScript.callbackFunction = () => {
                fadePanel?.SetActive(false);
                isCreditsEnabled = false;
            };
        } else {
            SceneManager.UnloadSceneAsync(sceneName);
            fadingScript.callbackFunction = () => {
                fadePanel?.SetActive(false);
                isCreditsEnabled = false;
            };
        }
        fadingScript?.FadeTo(0f, 1f);
    }

    public void sceneUnloadAsync(string sceneName) {
        SceneManager.LoadSceneAsync(sceneName);
    }

    public void tutorialOpen() {
        if (isTutorialEnabled) return;
        isTutorialEnabled = true;
        tutorialPanel?.SetActive(true);
        StartCoroutine(LerpCanvasAlpha(0f, 1f, 1.5f, () => {
            isTutorialFul = true;
        }));
    }

    public void tutorialClose() {
        StartCoroutine(LerpCanvasAlpha(1f, 0f, 1.5f, () => {
            tutorialPanel?.SetActive(false);
            isTutorialEnabled = false;
        }));
    }

    IEnumerator LerpCanvasAlpha(float source, float target, float duration, Action callback) {
        float time = 0;

        while (time < duration) {
            if (tutorialFadingScript != null) tutorialFadingScript.alpha = Mathf.Lerp(source, target, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        if (tutorialFadingScript != null) tutorialFadingScript.alpha = target;
        callback?.Invoke();
    }
}
