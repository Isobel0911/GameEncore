using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyPlant : MonoBehaviour, IInteractable
{
    [SerializeField] private string _prompt;

    public string InteractionPrompt => _prompt;

    public bool Interact(Interactor interactor)
    {
        var player = interactor.GetComponent<Inventory>();
        player.hasKey1 = true;
        // TODO:  提示玩家已经找到钥匙
        return true;
    }
}
