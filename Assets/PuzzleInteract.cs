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
    public static bool hasStarted = false;

    public bool Interact(Interactor interactor) {
        // Open puzzle scene
        if (!hasSolved) {
            hasStarted = true;
            Debug.Log("Open Scene request");
            Cursor.visible = true;
            SceneManager.LoadScene("SlidingTilePuzzle", LoadSceneMode.Additive);
            return true;
        } else {
            // show dialogue says you have decoded the documents.
            SceneManager.UnloadSceneAsync("SlidingTilePuzzle");
            FindObjectOfType<DialogueManager>().OpenDialogue(messages, actors);
            return true;
        }
    }
}
