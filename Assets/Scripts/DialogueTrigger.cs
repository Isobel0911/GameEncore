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
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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