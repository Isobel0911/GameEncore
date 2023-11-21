using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PuzzleInteract : MonoBehaviour, IInteractable
{
    public Message[] messages;
    public Actor[] actors;
    // Start is called before the first frame update
    [SerializeField] private string _prompt;

    public string InteractionPrompt => _prompt;

    public static bool hasSolved = false;
    public bool Interact(Interactor interactor) {
        // Open puzzle scene
        if (!hasSolved) {
            Debug.Log("Open Scene request");
            Cursor.visible = true;
            SceneManager.LoadScene("SlidingTilePuzzle", LoadSceneMode.Additive);
            return true;
        } else {
            // show dialogue says you have decoded the documents.
            FindObjectOfType<DialogueManager>().OpenDialogue(messages, actors);
            return true;
        }
    }
}
