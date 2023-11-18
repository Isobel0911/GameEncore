using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jewelery : MonoBehaviour, IInteractable
{
    [SerializeField] private string _prompt;

    public string InteractionPrompt => _prompt;

    public bool Interact(Interactor interactor)
    {
        var player = interactor.GetComponent<AlertController>();
        player.money += 500;

        Destroy(this.gameObject);
        return true;
    }
}
