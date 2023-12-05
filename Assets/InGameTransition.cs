using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameTransition : MonoBehaviour {
    private bool hasStarted = false;
    private BackgroundFading fadingScript;
    private GameObject fadePanel;
    private SceneSounds sceneSounds;
    public int mode = 0;
    public Sprite jackSprite;
    public Sprite youSprite;


    void Start() {
        findFadePanel();
    }

    void findFadePanel() {
        fadePanel = GameObject.Find("In-Game Transition");
        sceneSounds = fadePanel?.GetComponent<SceneSounds>();
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
            case 2:
                if (!hasStarted) {
                    hasStarted = true;
                    fadingScript.callbackFunction = () => {
                        disableFadePanel();
                        sceneSounds.PlayBGMSound(2);
                        finalAnimation();
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

    void finalAnimation() {
        int numMessage = 6;
        float waitingTime = 1f;
        Message[] messages = new Message[numMessage];
        Actor[] actors = new Actor[2];
        actors[0] = new Actor(); actors[0].name = "You"; actors[0].sprite = youSprite;
        actors[1] = new Actor(); actors[1].name = "Stranger"; actors[1].sprite = jackSprite;
        for (int i = 0; i < messages.Length; i++) {
            messages[i] = new Message();
        }
        Action<object[]>[] funcs = new Action<object[]>[numMessage];
        object[][] funcParams = new object[numMessage][];
        
        messages[0].actorId = 0; messages[0].message = "(Shocked) Wait, what was happened here?";
        funcs[0] = (parameters) => {
            SceneSounds sceneSounds = (SceneSounds) parameters[0];
            sceneSounds?.PlayBGMOnce(2);
            StartCoroutine(WaitOneSecondCoroutine((float) parameters[1], false));
        };
        funcParams[0] = new object[] {sceneSounds, waitingTime};

        messages[1].actorId = 1; messages[1].message = "Have you got the money, buddy?";
        funcs[1] = (parameters) => {
            StartCoroutine(WaitOneSecondCoroutine((float) parameters[0], false));
        }; funcParams[1] = new object[] {waitingTime};

        messages[2].actorId = 0; messages[2].message = "Yes... I have entered the vault.";
        funcs[2] = (parameters) => {
            StartCoroutine(WaitOneSecondCoroutine((float) parameters[0], false));
        }; funcParams[2] = new object[] {waitingTime};

        messages[3].actorId = 1; messages[3].message = "Nice job! Now, get out of that hall before someone found you!";
        funcs[3] = (parameters) => {
            StartCoroutine(WaitOneSecondCoroutine((float) parameters[0], false));
        }; funcParams[3] = new object[] {waitingTime};

        messages[4].actorId = 0; messages[4].message = "(Murmur) I have to leave now before it is too late!";
        funcs[4] = (parameters) => {
            SceneSounds sceneSounds = (SceneSounds) parameters[0];
            sceneSounds?.PlayBGMOnce(3);
            StartCoroutine(WaitOneSecondCoroutine(2.3f, true));
        }; funcParams[4] = new object[] {sceneSounds};

        FindObjectOfType<DialogueManager>()?.OpenDialogue(messages, actors, false, funcs, funcParams);
    }

    public IEnumerator WaitOneSecondCoroutine(float sec, bool isEnd) {
        yield return new WaitForSeconds(sec);
        DialogueManager.lockMulti = isEnd;
        if (isEnd) {
            fadePanel.SetActive(true);
            fadingScript.callbackFunction = () => {
                // SceneManager.DropTable("GameOverFinal");
            };
            fadingScript.FadeTo(1f, 2f);
        }
    }
}
