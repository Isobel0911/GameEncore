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
    public static bool convInProgress = false;

    private void Awake() {
        instance = this;
        initialStop = true; hasInitialized = false;
        conversationEnds = false;
        shouldWaitForFade = false;
        GameObject myGameObject;
        myGameObject = GameObject.Find("ConversationText");   conversation = myGameObject?.GetComponent<Conversation>();
        myGameObject = GameObject.Find("Conversation");       canvasGroup  = myGameObject?.GetComponent<CanvasGroup>();
        myGameObject = GameObject.Find("In-Game Transition"); sceneSounds  = myGameObject?.GetComponent<SceneSounds>();
    }
    
    private void Update() {
        if (initialStop && !hasInitialized) {
            hasInitialized = true;
            sceneSounds?.PlayInteractSound();
            OnConversation?.Invoke(this, new ConversationEventArgs(0, true, null, null));
            StartCoroutine(AppearConversation());
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
        } else {
            // more conversations begin here
            // for callbackFunction:
            //    1. if you don't have things to do at the beginning of conversation start,
            //      set null for both callbackParams, callbackFunction
            //    2. otherwise,
            //      set (for example, you params are (int a, float b, string c, bool d)):
            //      callbackParams = new object[] { 10, 20.5f, "Hello", true };
            //      callbackFunction = (object[] params) => {
            //          int a = params[0];
            //          float b = params[1];
            //          string c = params[2];
            //          bool d = params[3];
            //          ...
            //          your code here for doing something
            //      }
            //      ------------------------------------------------
            //      for no params, set:
            //      callbackParams = null
            //      callbackFunction = () => {
            //          ... your code here  for doing something
            //      }
            //    3. StartNewConversation(convTextIdx, callbackFunction, callbackParams);
            // Example below:
            if (true) return; // delete this code and below as you need
            int convTextIdx = 2;
            object[] callbackParams = new object[] { 10, 20.5f, "Hello", true };
            Action<object[]> callbackFunction = (parameters) => {
                // deal with object[] array
                foreach (var param in parameters) {
                    Console.WriteLine("deal with paran: " + param);
                    if (param is int intValue) {
                        // int type
                        Console.WriteLine("Integer: " + intValue);
                    } else if (param is float floatValue) {
                        // float type
                        Console.WriteLine("Float: " + floatValue);
                    } else if (param is string stringValue) {
                        // string type
                        Console.WriteLine("String: " + stringValue);
                    } else if (param is bool boolValue) {
                        // bool type
                        Console.WriteLine("Boolean: " + boolValue);
                    } else {
                        // unknown type
                        Console.WriteLine("Unknown Type: " + param);
                    }
                }
            };
            StartNewConversation(convTextIdx, callbackFunction, callbackParams);
            // your code starts here
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

