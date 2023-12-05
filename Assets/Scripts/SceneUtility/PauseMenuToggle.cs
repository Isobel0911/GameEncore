using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(CanvasGroup))] 
public class PauseMenuToggle : MonoBehaviour
{
    private CanvasGroup canvasGroup;
    public StarterAssets.StarterAssetsInputs inputs;
    private PlayerInput playerInput;
    public GameObject originalOne, volumeOne;
    private GameQuitter quitter;
    private SceneTransition sceneTransition;
    void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        sceneTransition = GetComponent<SceneTransition>();
        if (canvasGroup == null)
        {
            Debug.LogError("PauseMenuToggle: CanvasGroup component not found!");
        }
        playerInput = GameObject.Find("NestedParentArmature_Unpack/PlayerArmature").GetComponent<PlayerInput>();
        quitter = GetComponent<GameQuitter>();
    }
    void Start() {
        originalOne.SetActive(true);
        volumeOne.SetActive(false);
    }

    // Update is called once per frame
    void Update() {
        if (SceneTransition.isTutorialEnabled && SceneTransition.isTutorialFul && Input.anyKeyDown) {
            SceneTransition.isTutorialFul = false;
            sceneTransition?.tutorialClose();
            return;
        }
        if (Input.GetKeyUp (KeyCode.Escape)) {
            if (canvasGroup.interactable && !SceneTransition.isCreditsEnabled) { //disable in-game menu, enable game
                // inputs.cursorLocked = true;
                // inputs.cursorInputForLook = true;
                NPCController.hasHaltAlert = false;
                AIPathNPC.hasHaltAlert = false;
                canvasGroup.interactable = false;
                canvasGroup.blocksRaycasts = false;
                canvasGroup.alpha = 0f;
                // Time.timeScale = 1f;
                playerInput.ActivateInput();
                originalOne.SetActive(false);
                volumeOne.SetActive(false);
            } else if (!canvasGroup.interactable) { //enable in-game menu, disable game
                // inputs.cursorLocked = false;
                // inputs.cursorInputForLook = false;
                NPCController.hasHaltAlert = true;
                AIPathNPC.hasHaltAlert = true;
                canvasGroup.interactable = true;
                canvasGroup.blocksRaycasts = true;
                canvasGroup.alpha = 1f;
                // Time.timeScale = 0f;
                playerInput.DeactivateInput();
                originalOne.SetActive(true);
                volumeOne.SetActive(false);
            }
        }
        if (canvasGroup.interactable && Input.GetKeyUp (KeyCode.Q)) {
            quitter.QuitGame();
        }
    }
}
