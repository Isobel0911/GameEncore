using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CashNote : MonoBehaviour, IInteractable
{
    [SerializeField] private string _prompt;

    public string InteractionPrompt => _prompt;

    public int value;

    public bool Interact(Interactor interactor)
    {
        var player = interactor.GetComponent<AlertController>();
        player.money += value;
        player.alert = Mathf.Min(100, player.alert * 2);


        MainCharacterPickUp script = GameObject.Find("NestedParentArmature_Unpack/PlayerArmature").GetComponent<MainCharacterPickUp>();
        script.Collect(transform.position.y);

        Destroy(this.gameObject);
        return true;
    }

}
