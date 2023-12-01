using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Round {
    public Message[] messages;
    public Actor[] actors;
    public Round(Message[] messages, Actor[] actors) {
        this.messages = messages;
        this.actors = actors;
    }
}

public class NPCInteract : MonoBehaviour, IInteractable {
    public Round[] rounds;
    [SerializeField] private string _prompt;

    public string InteractionPrompt => _prompt;
    public bool loopable = false;
    public bool randomized = false;
    private int idx = 0;
    private System.Random random = null;

    public bool Interact(Interactor interactor) {
        Debug.Log("NPC Interact");
        if (randomized) {
            idx = random.Next(0, rounds.Length);
            FindObjectOfType<DialogueManager>().OpenDialogue(rounds[idx].messages, rounds[idx].actors);
        } else {
            FindObjectOfType<DialogueManager>().OpenDialogue(rounds[idx].messages, rounds[idx++].actors);
            if (idx == rounds.Length) {
                if (loopable) {
                    idx = 0;
                } else {
                    idx--;
                }
            }
        }
        return true;
    }

    void Awake() {
        if (randomized) random = new System.Random();
    }
 }
