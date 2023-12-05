using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ConversationEventArgs : EventArgs {
    public int convTextIdx { get; set; }
    public bool isStart { get;set; }
    public Action<object[]> callbackFunction { get;set; }
    public object[] callbackParams { get;set; }

    public ConversationEventArgs(int convTextIdx, bool isStart,
                                 Action<object[]> callbackFunction,
                                 object[] callbackParams) {
        this.convTextIdx = convTextIdx;
        this.isStart = isStart;
        this.callbackFunction = callbackFunction;
        this.callbackParams = callbackParams;
    }
}


public class EventManager : MonoBehaviour {
    public static event EventHandler<ConversationEventArgs> OnConversation;
    public static event EventHandler OnConversationEnd;

    public static EventManager instance;

    public bool conversationEnds = false;
    public bool shouldWaitForFade = false;
    private bool initialStop = true;
    private bool hasInitialized = false;
    private Conversation conversation;
    private CanvasGroup canvasGroup;
    private SceneSounds sceneSounds;
    // [HideInInspector]public bool conversationEnds = false;
    public static bool convInProgress = false;
    private AlertController alert;
    private DialogueManager dialogue;
    public bool invokedJessica = false;
    public bool invokedCard = false;

    private void Awake() {
        instance = this;
        initialStop = true; hasInitialized = false;
        conversationEnds = false;
        shouldWaitForFade = false;
        GameObject myGameObject;
        myGameObject = GameObject.Find("ConversationText");   conversation = myGameObject?.GetComponent<Conversation>();
        myGameObject = GameObject.Find("Conversation");       canvasGroup  = myGameObject?.GetComponent<CanvasGroup>();
        myGameObject = GameObject.Find("In-Game Transition"); sceneSounds  = myGameObject?.GetComponent<SceneSounds>();
        alert = FindObjectOfType<AlertController>();
        dialogue = FindObjectOfType<DialogueManager>();
    }
    
    private void Update() {

        if (initialStop && !hasInitialized) {
            hasInitialized = true;
            sceneSounds?.PlayInteractSound();
            OnConversation?.Invoke(this, new ConversationEventArgs(0, true, null, null));
            StartCoroutine(AppearConversation());
        }
        if (alert != null && !invokedJessica && alert.triggeredJessica) {
            invokedJessica = true;
            conversationEnds = false;
            sceneSounds?.PlayInteractSound();
            OnConversation?.Invoke(this, new ConversationEventArgs(1, true, null, null));
            return;
        }
        if (dialogue != null && !invokedCard && dialogue.talkedToCard) {
            invokedCard = true;
            conversationEnds = false;
            sceneSounds?.PlayInteractSound();
            
            OnConversation?.Invoke(this, new ConversationEventArgs(2, true, null, null));
            return;
        }
        // Press Tab for next conversation sentence
        if (convInProgress) {
            if (Input.GetKeyDown(KeyCode.Tab)  &&
                !shouldWaitForFade && !initialStop && 
                conversation.canProceedToNextConversation) {
                sceneSounds?.PlayInteractSound();
                if (conversationEnds) {
                    OnConversationEnd?.Invoke(this, EventArgs.Empty);
                } else {
                    OnConversation?.Invoke(this, new ConversationEventArgs(0, false, null, null));
                }
            }
        }
    }

    IEnumerator AppearConversation() {
        float time = 0;

        while (time < 2f) {
            time += Time.deltaTime;
            if (time < 1f) canvasGroup.alpha = Mathf.Lerp(0f, 1f, time / 1f);
            else canvasGroup.alpha = 1f;
            yield return null;
        }
        canvasGroup.alpha = 1f;
        initialStop = false;
    }

    public void StartNewConversation(int convTextIdx, Action<object[]> callbackFunction, object[] callbackParams) {
        sceneSounds?.PlayInteractSound();
        OnConversation?.Invoke(this, new ConversationEventArgs(convTextIdx, true, callbackFunction, callbackParams));
        StartCoroutine(AppearConversation());
    }
}

