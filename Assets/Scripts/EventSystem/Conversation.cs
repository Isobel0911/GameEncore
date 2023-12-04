using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Conversation : MonoBehaviour

{
    private string sceneName;
    private int lineCounter = 0;
    private CanvasGroup canvasGroup;
    // public StarterAssets.StarterAssetsInputs inputs;
    private Text conversationText;
    public Camera jessCam;
    public Camera mainCam;
    [HideInInspector]public List<string> conversation;

    // [HideInInspector]public bool canProceedToNextConversation = true;

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
    // Task 1 instruction - test ability
    public List<string> moneyInstruction = new() {
        "Good. You are here. Let's measure your capability first.",
        "Go find some valueables worth 1000 dollars. Cash, jewelry, whatever.",
        "Bank employees and guards are vigilant, so don't risk in front of them.",
        "If you're caught, cry for your family in jail. You only have this one chance."
    };

    // Task 2 instruction - talk to jessica
    public List<string> jessicaInstruction = new() {
        "Woah, I gotta say you are pretty talented at crime.",
        "Now, see the lady at the front desk? She's our friend. Ask her about Lost & Found and try to find HER key with BLUE eyes."
    };

    void Awake()
    {
        sceneName = SceneManager.GetActiveScene().name;
        canvasGroup = transform.parent.GetComponent<CanvasGroup>();
        conversationText = GetComponent<Text>();
        if (canvasGroup == null)
        {
            Debug.LogError("PauseMenuToggle: CanvasGroup component not found!");
        }
    }

    private void Start()
    {
        // Subscribe to the conversation events
        EventManager.OnConversation += ConversationStarts;
        EventManager.OnConversationEnd += ConversationEnds;
        if (sceneName == "Home")
        {
            conversation = introConversation;
        }
        else if (sceneName == "MainGame")
        {
            // Default Conversation
            conversation = moneyInstruction;

            // Other conversation could be changed in other places
            // eg. in AlertController.cs when player.input > 1500
        }
    }

    // private void OnDestroy()
    // {
    //     // Unsubscribe from events to avoid memory leaks
    //     EventManager.OnConversation -= ConversationStarts;
    //     EventManager.OnConversationEnd -= ConversationEnds;
    // }

    public void ConversationStarts(object sender, EventArgs e)
    {
        // if (!canProceedToNextConversation) return;
        EventManager.instance.convInProgress = true;
        ToggleConversationPanel();
        conversationText.text = conversation[lineCounter];

        // camera focus on jessica to help player identify her
        if (conversation == jessicaInstruction && lineCounter == 1)
        {
            if (jessCam != null && mainCam != null)
            {
                mainCam.enabled = false;
                jessCam.enabled = true;
                Debug.Log("Camera");
            }
        }
        lineCounter++;
        if (lineCounter == conversation.Count)
        {
            EventManager.instance.convInProgress = false;
            lineCounter = 0;
            EventManager.OnConversation -= ConversationStarts;
        }
        // EventManager.OnConversation += NextMessage;
        // EventManager.OnConversation -= ConversationStarts;
        
    }

    public void ConversationEnds(object sender, EventArgs e)
    {
        ToggleConversationPanel();
        EventManager.OnConversationEnd -= ConversationEnds;
        if (jessCam != null && mainCam != null)
        {
            jessCam.enabled = false;
            mainCam.enabled = true;
        }
    }

    private void ToggleConversationPanel()
    {
        if (EventManager.instance.convInProgress) {
            // inputs.cursorLocked = false;
            // inputs.cursorInputForLook = false;
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;
            canvasGroup.alpha = 1f;
        }
        else
        {
            // inputs.cursorLocked = true;
            // inputs.cursorInputForLook = true;
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
            canvasGroup.alpha = 0f;
        }

    }
}
