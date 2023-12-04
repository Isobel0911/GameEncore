using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour, IInteractable
{
    public Message[] messages;
    public Actor[] actors;

    [SerializeField] private string _prompt;

    public string InteractionPrompt => _prompt;

    public bool Interact(Interactor interactor)
    {
        Debug.Log("Interact with NPC");
        // FindObjectOfType<DialogueManager>().OpenDialogue(messages, actors);
        return true;
    }
}
[System.Serializable]
public class Message {
    public int actorId;
    public string message;
}
[System.Serializable]
public class Actor {
    public string name;
    public Sprite sprite;
}