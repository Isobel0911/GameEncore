using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ConversationPlayerInputController : MonoBehaviour {

    // private CanvasGroup canvasGroup;
    private PlayerInput playerInput;

    void Awake() {
        // Check if NestedParentArmature_Unpack exists
        GameObject nestedParentArmature = GameObject.Find("NestedParentArmature_Unpack");
        if (nestedParentArmature == null) { return; }

        // Get the Player Input component on PlayerArmature
        GameObject playerArmature = nestedParentArmature.transform.Find("PlayerArmature")?.gameObject;
        if (playerArmature != null) {
            playerInput = playerArmature?.GetComponent<PlayerInput>();
        }

        // // Check if Canvas/SafeAreaPanel/Conversation exists
        // GameObject canvasGroupObject = GameObject.Find("Canvas/SafeAreaPanel/Conversation");
        // if (canvasGroupObject != null) {
        //     canvasGroup = canvasGroupObject.GetComponent<CanvasGroup>();
        // }

        EventManager.OnConversation += DeactivateInputOnConversation;
        EventManager.OnConversationEnd += ActivateInputOnConversationEnd;
    }

    // void Update() {
    //     if (playerInput == null || canvasGroup == null) { return; }

    //     // Check if we are within the initial 1.5 seconds or if canvasGroup is interactable
    //     if (Time.time < initialControlTime || DialogueManager.isActive || (canvasGroup.interactable && canvasGroup.blocksRaycasts)) {
    //         // Disable the Player Input component
    //         playerInput.DeactivateInput();
    //     } else {
    //         // Enable the Player Input component
    //         playerInput.ActivateInput();
    //     }
    // }

    public void DeactivateInputOnConversation(object sender, EventArgs e) {
        if (playerInput != null && playerInput.inputIsActive) playerInput.DeactivateInput();
    }
    public void ActivateInputOnConversationEnd(object sender, EventArgs e) {
        if (playerInput != null && !playerInput.inputIsActive) playerInput.ActivateInput();
    }
}
