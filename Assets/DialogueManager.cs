using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public Image actorImage;
    public TextMeshProUGUI actorName;
    public TextMeshProUGUI messageText;
    public RectTransform backgroundBox;

    Message[] currentMessages;
    Actor[] currentActors;
    int ActiveMEssage = 0;
    public static bool isActive = false;
    public void OpenDialogue(Message[] messages, Actor[] actors) {
        currentMessages = messages;
        currentActors = actors;
        ActiveMEssage = 0;

        Debug.Log("Started Conversation Loaded messages: " + messages.Length);
        DisplayMessage();
    }

    void DisplayMessage() 
    {
        Message messageToDisplay = currentMessages[ActiveMEssage];
        messageText.text = messageToDisplay.message;

        Actor actorToDisplay = currentActors[messageToDisplay.actorId];
        actorName.text = actorToDisplay.name;
        actorImage.sprite = actorToDisplay.sprite;
    }

    public void NextMessage() {
        ActiveMEssage++;
        if (ActiveMEssage < currentMessages.Length) {
            DisplayMessage();
        } else {
            // close dialogue
            Debug.Log("Conversation ends");
        }
    }
    void Update() 
    {

    }

}

