using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Conversation : MonoBehaviour {
    private string sceneName;
    private int lineCounter = 0;
    private CanvasGroup canvasGroup;
    // public StarterAssets.StarterAssetsInputs inputs;
    private Text conversationText;
    private List<string> conversation;
    private BackgroundFading fadingScript;
    private GameObject fadePanel;
    private SceneSounds sceneSoundsScript;
    public bool canProceedToNextConversation = true;
    public int conversationStartMode = 0;
    private int conversationEndMode = 0;
    private bool fadingPanelStart = true;
    private bool fadingPanelEnd = false;

    // ========== Conversation Contents ==========
    // To add a sentence, simply append it to list, no need to change other places.
    
    // === Conversation Conversation Sentences ===
    public List<string> introConversation = new() {
        "You know who I am.",
        "[Ironic chuckling] How's your night in casino yesterday? Did you got enough to pay me back?",
        "Three more days? No, a deadline is a deadline. I mean it, D-E-A-D.",
        "[Evil giggling] Look how pathetic you are when you beg me." +
                "You know what, let's forget about your debts.",
        "You don't have money, but banks do." +
                "If you wanna keep you and your family safe, go to the bank tomorrow." +
                "You'll get more instructions there.",
        "Don't play tricks. Don't call the cops. You're not running away."
    };

    // === Instruction Conversation Sentences ===
    public List<string> instructionConversation = new() {
        "Good. You are here. Let's measure your capability first.",
        "Go grab some valuables. Some doors may be locked, so you must find the keys yourself.",
        "Stay low and don't attract any attention, or say hello to jail and say goodbye to your family."
    };

    void Awake() {
        sceneName = SceneManager.GetActiveScene().name;
        canvasGroup = transform.parent.GetComponent<CanvasGroup>();
        conversationText = GetComponent<Text>();
        if (canvasGroup == null) {
            Debug.LogError("PauseMenuToggle: CanvasGroup component not found!");
        }
        fadePanel = GameObject.Find("FadePanel");
        fadingScript = fadePanel.GetComponent<BackgroundFading>();
        if (fadingScript == null) fadingScript = fadePanel.AddComponent<BackgroundFading>();
        GameObject sceneSoundsGB = GameObject.Find("In-Game Transition");
        if (sceneSoundsGB == null) sceneSoundsGB = new GameObject("In-Game Transition");
        sceneSoundsScript = sceneSoundsGB.GetComponent<SceneSounds>();
        if (sceneSoundsScript == null) sceneSoundsScript = sceneSoundsGB.AddComponent<SceneSounds>();
    }

    private void Start() {
        // Subscribe to the conversation events
        EventManager.OnConversation += ConversationStarts;
        EventManager.OnConversationEnd += ConversationEnds;
    }

    private void OnDestroy() {
        // Unsubscribe from events to avoid memory leaks
        EventManager.OnConversation -= ConversationStarts;
        EventManager.OnConversationEnd -= ConversationEnds;
    }

    public void ConversationStarts(object sender, ConversationEventArgs e) {
        if (!canProceedToNextConversation) return;
        ToggleConversationPanel();

        bool isStart;
        int convTextIdx;
        Action<object[]> callbackFunction;
        object[] callbackParams;

        if (e == null || e == EventArgs.Empty) {
            isStart = false;
            convTextIdx = 0;
            callbackFunction = null;
            callbackParams = null;
        } else {
            isStart = e.isStart;
            convTextIdx = e.convTextIdx;
            callbackFunction = e.callbackFunction;
            callbackParams = e.callbackParams;
        }

        if (isStart){
            EventManager.convInProgress = true;
            lineCounter = 0;
            callbackFunction(callbackParams);
            if (sceneName == "Home") {
                conversation = introConversation;
                fadingPanelStart = true;
                fadingPanelEnd = false;
                conversationEndMode = 0;
            } else if (sceneName == "MainGame") {
                switch(convTextIdx) {
                    case 0:
                        conversation = instructionConversation;
                        fadingPanelStart = true;
                        fadingPanelEnd = false;
                        conversationEndMode = 1;
                        break;
                    case 1:
                        break;
                    default:
                        break;
                }
            }
        }
        if (lineCounter < conversation.Count) {
            conversationText.text = conversation[lineCounter];
            // Debug.Log(conversation[lineCounter]);
        }
        lineCounter++;
        if (lineCounter == 2) {
            // first behavior of fading panel
            if (fadingPanelStart) {
                fadingScript.callbackFunction = () => {
                    fadePanel.SetActive(false);
                    EventManager.instance.shouldWaitForFade = false;
                    sceneSoundsScript.PlayBGMSound(conversationStartMode);
                };
                fadingScript.FadeTo(0f, 1f);
                EventManager.instance.shouldWaitForFade = true;
            }
        } else if (lineCounter == conversation.Count) {
            // last
            EventManager.instance.conversationEnds = true;
        }
        if (lineCounter <= conversation.Count) {
            // not last conversation
            canProceedToNextConversation = false; // stop conversation
            // StartCoroutine(WaitAndAllowNextConversation(1f)); // for 1 seconds
            StartCoroutine(WaitAndAllowNextConversation(0f));
        }
    }

    IEnumerator WaitAndAllowNextConversation(float waitTime) {
        yield return new WaitForSeconds(waitTime);
        canProceedToNextConversation = true; // enable continue talking
    }

    public void ConversationEnds(object sender, EventArgs e) {
        ToggleConversationPanel();
        EventManager.OnConversationEnd -= ConversationEnds;
    }

    private void ToggleConversationPanel() {
        
        if (EventManager.instance.conversationEnds) {
            // inputs.cursorLocked = true;
            // inputs.cursorInputForLook = true;
            StartCoroutine(TransitionToNextScene());
        } else {
            // inputs.cursorLocked = false;
            // inputs.cursorInputForLook = false;
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;
        }
    }

    public IEnumerator TransitionToNextScene() {
        // Optionally, wait for a short duration if needed
        EventManager.instance.shouldWaitForFade = true;
        if (fadingPanelEnd) {
            fadingScript.callbackFunction = () => {
                fadePanel.SetActive(false);
                StartCoroutine(FadeOutCanvasGroup(canvasGroup, 0.5f));
            };
            fadingScript.FadeTo(0f, 1f);
            yield return null;
        } else {
            yield return StartCoroutine(FadeOutCanvasGroup(canvasGroup, 0.5f));
        }
    }

    private IEnumerator FadeOutCanvasGroup(CanvasGroup canvasGroup, float duration) {
        float startAlpha = canvasGroup.alpha;
        float time = 0;

        while (time < duration) {
            time += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(startAlpha, 0, time / duration);
            yield return null;
        }

        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
        canvasGroup.alpha = 0;
        EventManager.convInProgress = false;
        switch(conversationEndMode) {
            case 0:
                GameObject startButton = GameObject.Find("Canvas/SafeAreaPanel/StartButton");
                startButton.SetActive(true);
                startButton.GetComponent<SceneTransition>().init(sceneSoundsScript, fadePanel, fadingScript);
                EventManager.instance.shouldWaitForFade = true; // to disable Tab since we don't need to use it anymore
                break;
            default:
                EventManager.instance.shouldWaitForFade = true;
                break;
        }
    }
}
