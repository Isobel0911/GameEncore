using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour {
    private BackgroundFading fadingScript;
    private GameObject fadePanel;
    private SceneSounds sceneSoundsScript;

    void Awake() {
        init();
    }

    public void init() {
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
                this.sceneChanged(sceneName);
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
}
