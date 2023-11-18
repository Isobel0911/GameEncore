using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CashNote : MonoBehaviour, IInteractable
{
    [SerializeField] private string _prompt;

    public string InteractionPrompt => _prompt;

    public bool Interact(Interactor interactor)
    {
        var player = interactor.GetComponent<AlertController>();
        player.money += 100;
        // TODO: if any NPC alert value > 30
        // player.alert += 20;

        Destroy(this.gameObject);
        return true;
    }
}
