using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyPlant : MonoBehaviour, IInteractable
{
    public Message[] messages;
    public Actor[] actors;

    [SerializeField] private string _prompt;
    public string InteractionPrompt => _prompt;
    private GameObject dialogue;

    // give prompts when interacting with all plants
    public bool Interact(Interactor interactor) { 
        var player = interactor.GetComponent<InventorySelf>();
        dialogue = GameObject.Find("Canvas/SafeAreaPanel/DialogueBox");
        if (player == null || player.hasKey1 || dialogue == null) return true;
        bool canSearchPlant = dialogue.GetComponent<DialogueManager>().talkedToJessica;
        if (!canSearchPlant) return true;
        // only update hasKey1 if player found the correct plant
        if (gameObject.tag == "keyplant")
        {
            player.hasKey1 = true;
        }


        var playerAlert = interactor.GetComponent<AlertController>();
        playerAlert.alert = Mathf.Min(100, playerAlert.alert * 2);
        MainCharacterPickUp script = GameObject.Find("NestedParentArmature_Unpack/PlayerArmature").GetComponent<MainCharacterPickUp>();
        script.Collect(transform.position.y);

        FindObjectOfType<DialogueManager>().OpenDialogue(messages, actors);
        return true;
    }
}
