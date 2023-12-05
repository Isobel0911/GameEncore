using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameTransition : MonoBehaviour {
    private bool hasStarted = false;
    private BackgroundFading fadingScript;
    private GameObject fadePanel;
    public int mode = 0;

    void Start() {
        findFadePanel();
    }

    void findFadePanel() {
        fadePanel = GameObject.Find("FadePanel");
        fadingScript = fadePanel?.GetComponent<BackgroundFading>();
        if (fadingScript == null) fadingScript = fadePanel?.AddComponent<BackgroundFading>();
    }

    // Update is called once per frame
    void Update() {
        switch(mode) {
            case 0:
                if (!hasStarted) {
                    hasStarted = true;
                    fadingScript.callbackFunction = () => {
                        disableFadePanel();
                    };
                    fadingScript.FadeTo(0f, 3f);
                }
                break;
            case 1:
                if (!hasStarted) {
                    hasStarted = true;
                    fadingScript.callbackFunction = () => {
                        disableFadePanel();
                    };
                    fadingScript.FadeTo(0f, 3f);
                }
                break;
            default:
                break;
        }
    }

    void disableFadePanel() {
        fadePanel.SetActive(false);
    }
}
