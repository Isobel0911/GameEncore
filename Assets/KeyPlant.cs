using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyPlant : MonoBehaviour, IInteractable
{
    public Message[] messages;
    public Actor[] actors;

    [SerializeField] private string _prompt;
    public string InteractionPrompt => _prompt;

    public bool Interact(Interactor interactor)
    {   
        Debug.Log("found the key");
        var player = interactor.GetComponent<Inventory>();
        player.hasKey1 = true;
        Debug.Log("NPC Interact");
        FindObjectOfType<DialogueManager>().OpenDialogue(messages, actors);
        return true;
    }
}
