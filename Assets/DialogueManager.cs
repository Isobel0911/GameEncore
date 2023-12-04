using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class DialogueManager : MonoBehaviour
{
    public Image actorImage;
    public TextMeshProUGUI actorName;
    public TextMeshProUGUI messageText;
    public RectTransform backgroundBox;

    [HideInInspector] public bool talkedToJessica = false;

    Message[] currentMessages;
    Actor[] currentActors;
    int ActiveMEssage = 0;
    private PlayerInput playerInput;
    public static bool isActive = false;
    public void OpenDialogue(Message[] messages, Actor[] actors) {
        isActive = true;
        // gameObject.SetActive(true);

        currentMessages = messages;
        currentActors = actors;
        ActiveMEssage = 0;

        playerInput.DeactivateInput();

        DisplayMessage();
        backgroundBox.LeanScale(Vector3.one, 0.5f);
        
    }

    void DisplayMessage() 
    {
        Message messageToDisplay = currentMessages[ActiveMEssage];
        messageText.text = messageToDisplay.message;

        Actor actorToDisplay = currentActors[messageToDisplay.actorId];
        actorName.text = actorToDisplay.name;
        actorImage.sprite = actorToDisplay.sprite;
    }

    public void NextMessage() {
        ActiveMEssage++;
        if (ActiveMEssage < currentMessages.Length) {
            DisplayMessage();
        } else {
            // close dialogue
            isActive = false;
            playerInput.ActivateInput();
            backgroundBox.LeanScale(Vector3.zero, 0.5f).setEaseInOutExpo();

            if (currentActors.Length > 1 && currentActors[1].name == "Jessica")
            {
                // Debug.Log("talkedToJessica");
                talkedToJessica = true;
            }
        }
    }
    void Start()
    {
        backgroundBox.transform.localScale = Vector3.zero;
        playerInput = GameObject.Find("NestedParentArmature_Unpack/PlayerArmature").GetComponent<PlayerInput>();
    }
    void Update() 
    {
        // press space to proceed next message
        if (Input.GetKeyDown(KeyCode.Tab) && isActive) {
            NextMessage();
        }
    }

}

