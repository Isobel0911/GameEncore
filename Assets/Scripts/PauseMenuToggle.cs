using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))] 
public class PauseMenuToggle : MonoBehaviour
{
    private CanvasGroup canvasGroup;
    public StarterAssets.StarterAssetsInputs inputs;
    void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            Debug.LogError("PauseMenuToggle: CanvasGroup component not found!");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp (KeyCode.Escape)) {
            if (canvasGroup.interactable) {         //disable in-game menu, enable game
                inputs.cursorLocked = true;
                inputs.cursorInputForLook = true;
                canvasGroup.interactable = false;
                canvasGroup.blocksRaycasts = false;
                canvasGroup.alpha = 0f;
                Time.timeScale = 1f;
            }
            else                                    //enable in-game menu, disable game
            {
                inputs.cursorLocked = false;
                inputs.cursorInputForLook = false;
                canvasGroup.interactable = true;
                canvasGroup.blocksRaycasts = true;
                canvasGroup.alpha = 1f;
                Time.timeScale = 0f;
            }
        }
    }
}
