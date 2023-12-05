using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.Events;

public class DialogueManager : MonoBehaviour {
    public Image actorImage;
    public TextMeshProUGUI actorName;
    public TextMeshProUGUI messageText;
    public RectTransform backgroundBox;
    
    [HideInInspector] public bool talkedToJessica = false;
    public bool talkedToCard = false;

    Message[] currentMessages;
    Actor[] currentActors;
    int ActiveMessage = 0;
    private PlayerInput playerInput;
    public static bool isActive = false;
    private static bool isCaught = false;
    private Action<object[]> callbackFunction;
    private object[] callbackParams;

    // multi-function controller
    private bool multiFunc = false;
    private Action<object[]>[] multiFunction;
    private object[][] multiParams;
    public static bool lockMulti = false;

    private SceneSounds sceneSounds;

    void Awake() {
        playerInput = GameObject.Find("NestedParentArmature_Unpack/PlayerArmature")?.GetComponent<PlayerInput>();
        sceneSounds = GameObject.Find("In-Game Transition")?.GetComponent<SceneSounds>();
    }

    void Start() {
        backgroundBox.transform.localScale = Vector3.zero;
    }
    
    void Update()  {
        // press space to proceed next message
        if (isActive && Input.GetKeyDown(KeyCode.Tab) &&
            (!multiFunc || !lockMulti)) {
            NextMessage();
        }
    }


    public void OpenDialogue(Message[] messages, Actor[] actors) {
        if (isActive) return;
        isActive = true;
        isCaught = false;
        callbackFunction = null;
        callbackParams = null;
        // gameObject.SetActive(true);

        currentMessages = messages;
        currentActors = actors;
        ActiveMessage = 0;

        playerInput?.DeactivateInput();

        DisplayMessage();
        backgroundBox.LeanScale(Vector3.one, 0.5f);
    }

    public void OpenDialogue(Message[] messages, Actor[] actors,
                             bool isCaughtCurr,
                             Action<object[]> callbackFunction, object[] callbackParams) {
        if (isActive && isCaught || isActive && !isCaughtCurr) return;
        bool prevActive = isActive;
        isActive = true;
        this.callbackFunction = callbackFunction;
        this.callbackParams = callbackParams;
        isCaught = isCaughtCurr;
        // gameObject.SetActive(true);

        currentMessages = messages;
        currentActors = actors;
        ActiveMessage = 0;

        playerInput?.DeactivateInput();

        DisplayMessage();
        if (!prevActive) backgroundBox.LeanScale(Vector3.one, 0.5f);
    }

    public void OpenDialogue(Message[] messages, Actor[] actors,
                             bool isCaughtCurr,
                             Action<object[]>[] callbackFunction, object[][] callbackParams) {
        if (isActive && isCaught || isActive && !isCaughtCurr) return;
        bool prevActive = isActive;
        isActive = true;
        multiFunc = true;
        this.multiFunction = callbackFunction;
        this.multiParams = callbackParams;
        isCaught = isCaughtCurr;
        // gameObject.SetActive(true);

        currentMessages = messages;
        currentActors = actors;
        ActiveMessage = 0;

        playerInput?.DeactivateInput();

        DisplayMessage();
        if (!prevActive) backgroundBox.LeanScale(Vector3.one, 0.5f);
    }
    

    void DisplayMessage()  {
        sceneSounds?.PlayInteractSound();
        lockMulti = true;
        Message messageToDisplay = currentMessages[ActiveMessage];
        messageText.text = messageToDisplay.message;

        Actor actorToDisplay = currentActors[messageToDisplay.actorId];
        actorName.text = actorToDisplay.name;
        actorImage.sprite = actorToDisplay.sprite;
        if (multiFunc) {
            if (multiFunction != null &&
                multiFunction.Length > ActiveMessage &&
                multiFunction[ActiveMessage] != null)
                multiFunction[ActiveMessage](multiParams[ActiveMessage]);
        }
    }

    public void NextMessage() {
        ActiveMessage++;
        if (ActiveMessage < currentMessages.Length) {
            DisplayMessage();
        } else {
            // close dialogue
            Debug.Log("close");
            isActive = false;
            backgroundBox.LeanScale(Vector3.zero, 0.5f)
                         .setEaseInOutExpo()
                         .setOnComplete(OnAnimationComplete);

            if (currentActors.Length > 1 && currentActors[1].name == "Jessica") {
                // Debug.Log("talkedToJessica");
                talkedToJessica = true;
            }
            if (currentActors.Length > 1 && currentActors[1].name == "ID Card") {
                Debug.Log("talkedToCard");
                talkedToCard = true;
                EventManager.OnConversation += FindObjectOfType<Conversation>().ConversationStarts;
            }
        }
    }

    private void OnAnimationComplete() {
        if (callbackFunction != null) {
            callbackFunction(callbackParams);
        } else {
            playerInput?.ActivateInput();
        }
    }
}

