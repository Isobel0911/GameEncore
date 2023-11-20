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
        var player = interactor.GetComponent<InventorySelf>();
        player.hasKey1 = true;
        Debug.Log("NPC Interact");


        var playerAlert = interactor.GetComponent<AlertController>();
        playerAlert.alert = Mathf.Min(100, playerAlert.alert * 2);
        MainCharacterPickUp script = GameObject.Find("NestedParentArmature_Unpack/PlayerArmature").GetComponent<MainCharacterPickUp>();
        script.Collect(transform.position.y);

        FindObjectOfType<DialogueManager>().OpenDialogue(messages, actors);
        return true;
    }
}
