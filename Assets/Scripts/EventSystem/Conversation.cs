using System;
using System.Collections;
using System.Collections.Generic;
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
    private List<string> conversation;

    // ========== Conversation Contents ==========
    // To add a sentence, simply append it to list, no need to change other places.
    
    // === Conversation Conversation Sentences ===
    public List<string> introConversation = new()
    {
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
    public List<string> instructionConversation = new()
    {
        "Good. You are here. Let's measure your capability first.",
        "Go grab some valuables. Some doors may be locked, so you must find the keys yourself.",
        "Stay low and don't attract any attention, or say hello to jail and say goodbye to your family."
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
        
    }

    // private void OnDestroy()
    // {
    //     // Unsubscribe from events to avoid memory leaks
    //     EventManager.OnConversation -= ConversationStarts;
    //     EventManager.OnConversationEnd -= ConversationEnds;
    // }

    public void ConversationStarts(object sender, EventArgs e)
    {
        ToggleConversationPanel();
        if (sceneName == "Home")
        {
            conversation = introConversation;
        }
        else if (sceneName == "MainGame")
        {
            conversation = instructionConversation;
        }
        conversationText.text = conversation[lineCounter];
        // Debug.Log(conversation[lineCounter]);
        lineCounter++;
        if (lineCounter == conversation.Count)
        {
            EventManager.OnConversation -= ConversationStarts;
            EventManager.instance.conversationEnds = true;
            lineCounter = 0;
        }
    }

    public void ConversationEnds(object sender, EventArgs e)
    {
        ToggleConversationPanel();
        EventManager.OnConversationEnd -= ConversationEnds;
        
    }

    private void ToggleConversationPanel()
    {
        
        if (EventManager.instance.conversationEnds) {
            // inputs.cursorLocked = true;
            // inputs.cursorInputForLook = true;
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
            canvasGroup.alpha = 0f;
        }
        else
        {
            // inputs.cursorLocked = false;
            // inputs.cursorInputForLook = false;
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;
            canvasGroup.alpha = 1f;
        }
    }
}
