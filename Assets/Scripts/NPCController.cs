using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class NPCController : MonoBehaviour {
    Animator animator;
    public GameObject MainCharacter;
    public int alertValue;
    public ProgressBar pb;
    public AlertController ac;
    public string NPCName ;
    public Sprite sprite;
    public Sprite spritePlayer;
    private static Message[] messagesCaught;
    private static Actor[]   actorsCaught;
    private static bool hasCaught = false;

    void Start() {
        ac = MainCharacter.GetComponent<AlertController>();
        animator = GetComponent<Animator>();
        string additionStr = "";
        if (NPCName == "FBI") {
            additionStr = " and surrender right now!";
        } else {
            additionStr = " or I will call the police!";
        }
        messagesCaught = new Message[] { new Message(), new Message() };
        messagesCaught[0].actorId = 0; 
        messagesCaught[0].message = "Hey! What are you doing? Stop right there" + additionStr;
        messagesCaught[1].actorId = 1;
        messagesCaught[1].message = "Shit! I messed it up...";
        actorsCaught = new Actor[] { new Actor(), new Actor() };
        actorsCaught[0].name = NPCName;
        actorsCaught[0].sprite = sprite;
        actorsCaught[1].name = "You";
        actorsCaught[1].sprite = spritePlayer;
    }
        

    void Update() {
        if (ac.alert >= 80 && !hasCaught) {
            hasCaught = true;
            FindObjectOfType<DialogueManager>().OpenDialogue(messagesCaught, actorsCaught, true,
                (object[] parameters) => {
                    SceneManager.LoadScene("GameOverAlert");
                }, new object[0]);
            return;
        }
        float decreaseConstant = 0.002f; // this is changed based on different people, caution factor
        Vector3 MainCharacterPosition = MainCharacter.transform.Find("Skeleton/Hips/Spine/Chest/UpperChest/Neck/Head").position;
        Vector3 myPosition = transform.Find("Root/Hips/Spine_01/Spine_02/Spine_03/Neck/Head").position;

        float distance = (myPosition - MainCharacterPosition).magnitude;
        float angle = Vector3.Angle(transform.Find("Root/Hips/Spine_01/Spine_02/Spine_03/Neck/Head").forward, MainCharacterPosition - myPosition);
        float coverDistanceOnAngle = angle * (-0.0556f) + 10f;

        
        if (distance <= coverDistanceOnAngle && angle <= 90f) {
            if (CheckObstacle(myPosition, MainCharacterPosition, distance) == true) {
                ac.alert -= decreaseConstant;
                if (ac.alert < 0){
                    ac.alert = 0;
                }
            } else {
                // check if walking fbi_01
                // if (gameObject.name == "Walking_FBI_01") {
                //     print("now we are in the range of walking fbi");
                //     Scene currentScene = SceneManager.GetActiveScene();
                //     Debug.Log("Current Scene: " + currentScene.name);

                //     if (currentScene.name == "SlidingTilePuzzle") {
                //         print("you are solving puzzle!!");
                //     }
                // }
                if (PuzzleInteract.hasStarted && !PuzzleInteract.hasSolved && !hasCaught) {
                    ac.alert = 100;
                    hasCaught = true;
                    FindObjectOfType<DialogueManager>().OpenDialogue(messagesCaught, actorsCaught, true,
                        (object[] parameters) => {
                            SceneManager.LoadScene("GameOverAlert");
                        }, new object[0]);
                    return;
                }
                if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) {
                    float calculatedAlertValue = (50 * (1f - distance / coverDistanceOnAngle));
                    if (ac.alert < calculatedAlertValue) {
                        ac.alert = calculatedAlertValue;
                    } else if (ac.alert > calculatedAlertValue) {
                        ac.alert = Math.Max(ac.alert - decreaseConstant, calculatedAlertValue);
                    }
                } else {
                    float calculatedAlertValue = (30 * (1f - distance / coverDistanceOnAngle));
                    if (ac.alert < calculatedAlertValue) {
                        ac.alert = calculatedAlertValue;
                    } else if (ac.alert > calculatedAlertValue) {
                        ac.alert = Math.Max(ac.alert - decreaseConstant, calculatedAlertValue);
                    }
                }
            }
        } else {
            ac.alert -= decreaseConstant;
            if (ac.alert < 0){
                ac.alert = 0;
            }
        }
    }

    bool CheckObstacle(Vector3 from, Vector3 to, float distance) {
        RaycastHit hit;
        if (Physics.Raycast(from, to - from, out hit, distance)) {
            if (!hit.collider.CompareTag("MainCharacter")) {
                return true;
            }
        }
        return false;
    }
}
