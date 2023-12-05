using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PuzzleInteract : MonoBehaviour, IInteractable {
    public Message[] messages;
    public Actor[]   actors;
    public static Message[] messagesCaught;
    public static Actor[]   actorsCaught;
    // Start is called before the first frame update
    [SerializeField] private string _prompt;

    public string InteractionPrompt => _prompt;

    public static bool hasSolved = false;
    public static bool hasStarted = false;

    public void Awake() {
        
    }

    public bool Interact(Interactor interactor) {
        // Open puzzle scene
        if (!hasStarted && !hasSolved) {
            hasStarted = true;
            Cursor.visible = true;
            SceneManager.LoadScene("SlidingTilePuzzle", LoadSceneMode.Additive);
        } else if (hasStarted && hasSolved) {
            // show dialogue says you have decoded the documents.
            SceneManager.UnloadSceneAsync("SlidingTilePuzzle");
            FindObjectOfType<DialogueManager>().OpenDialogue(messages, actors);
        }
        return true;
    }
    public static void PuzzleCaught(string name, Sprite sprite) {
        SceneManager.UnloadSceneAsync("SlidingTilePuzzle");
        foreach (Actor actor in actorsCaught) {
            if (actor.name == "") {
                actor.name = name;
                actor.sprite = sprite;
            }
        }
        FindObjectOfType<DialogueManager>().OpenDialogue(messagesCaught, actorsCaught);
    }
    public static void loadScene() {
        
    }
}
