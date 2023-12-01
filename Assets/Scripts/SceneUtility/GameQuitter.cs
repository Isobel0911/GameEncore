using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameQuitter : MonoBehaviour {
    private BackgroundFading fadingScript;
    private GameObject fadePanel;

    void Start() {
        findFadePanel();
    }

    void findFadePanel() {
        fadePanel = GameObject.Find("FadePanel");
        fadingScript = fadePanel.GetComponent<BackgroundFading>();
        if (fadingScript == null) fadingScript = fadePanel.AddComponent<BackgroundFading>();
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetKeyUp(KeyCode.X)) {
            QuitGame();
        }
    }

    public void QuitGame() {
        fadePanel.SetActive(true);
        fadingScript.callbackFunction = () => {
            quit();
        };
        fadingScript.FadeTo(1f, 1f);
    }

    private void quit() {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}
