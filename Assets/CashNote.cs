using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CashNote : MonoBehaviour, IInteractable
{
    [SerializeField] private string _prompt;

    public string InteractionPrompt => _prompt;

    public int value;

    public bool Interact(Interactor interacdtor)
    {
        var player = interactor.GetComponent<AlertController>();
        player.money += value;
        // TODO: if any NPC alert value > 30
        // player.alert += 20;

        Destroy(this.gameObject);
        return true;
    }

}
