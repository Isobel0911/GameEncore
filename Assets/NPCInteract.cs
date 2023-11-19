using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCInteract : MonoBehaviour, IInteractable
{
    public Message[] messages;
    public Actor[] actors;
    [SerializeField] private string _prompt;

    public string InteractionPrompt => _prompt;

    public bool Interact(Interactor interactor)
    {
        Debug.Log("NPC Interact");
        FindObjectOfType<DialogueManager>().OpenDialogue(messages, actors);
        return true;
    }
 }
