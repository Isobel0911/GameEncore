using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventManager : MonoBehaviour
{
    public static event EventHandler OnConversation;
    public static event EventHandler OnConversationEnd;

    public static EventManager instance;

    public bool conversationEnds = false;
    public bool shouldWaitForFade = false;
    private bool initialStop = true;
    private bool hasInitialized = false;
    private Conversation conversation;
    private CanvasGroup canvasGroup;
    private SceneSounds sceneSounds;

    private void Awake() {
        instance = this;
        initialStop = true; hasInitialized = false;
        conversationEnds = false;
        shouldWaitForFade = false;
        GameObject conversationGB = GameObject.Find("ConversationText");
        if (conversationGB != null) {
            conversation = conversationGB.GetComponent<Conversation>();
        }
        conversationGB = GameObject.Find("Conversation");
        if (conversationGB != null) {
            canvasGroup = conversationGB.GetComponent<CanvasGroup>();
        }
        conversationGB = GameObject.Find("In-Game Transition");
        if (conversationGB != null) {
            sceneSounds = conversationGB.GetComponent<SceneSounds>();
        }
    }

    private void Start() {
        OnConversation?.Invoke(this, EventArgs.Empty);
    }
    
    private void Update() {
        if (initialStop && !hasInitialized) {
            hasInitialized = true;
            if (sceneSounds != null) sceneSounds.PlayInteractSound();
            OnConversation?.Invoke(this, EventArgs.Empty);
            StartCoroutine(AppearConversation());
        }
        // Press Tab for next conversation sentence
        if (Input.GetKeyDown(KeyCode.Tab)  &&
            !shouldWaitForFade && !initialStop && 
            conversation.canProceedToNextConversation) {
            if (sceneSounds != null) sceneSounds.PlayInteractSound();
            if (conversationEnds) {
                OnConversationEnd?.Invoke(this, EventArgs.Empty);
            } else {
                OnConversation?.Invoke(this, EventArgs.Empty);
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
}

