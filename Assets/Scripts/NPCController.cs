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
    private Message[] messagesCaught;
    private Actor[]   actorsCaught;
    private static bool hasCaught = false;
    private BackgroundFading fadingScript;
    private GameObject fadePanel;
    public static bool hasHaltAlert = false;

    void Awake() {
        fadePanel = GameObject.Find("FadePanel");
        fadingScript = fadePanel?.GetComponent<BackgroundFading>();
        if (fadingScript == null) fadingScript = fadePanel?.AddComponent<BackgroundFading>();
        ac = MainCharacter.GetComponent<AlertController>();
        animator = GetComponent<Animator>();
    }

    void Start() {
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
        if (hasHaltAlert) return;
        if (ac.alert >= 80 && !hasCaught) {
            hasCaught = true;
            FindObjectOfType<DialogueManager>().OpenDialogue(messagesCaught, actorsCaught, true,
                (object[] parameters) => {
                    GameObject fadePanel = (GameObject)parameters[0];
                    BackgroundFading fadingScript = (BackgroundFading)parameters[1];
                    fadePanel.SetActive(true);
                    fadingScript.callbackFunction = () => {
                        SceneManager.LoadScene("GameOverAlert");
                    };
                    fadingScript.FadeTo(1f, 1f);
                }, new object[] {fadePanel, fadingScript});
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
                // check if talked to jessica
                GameObject gameObjectA = GameObject.Find("Canvas/SafeAreaPanel/DialogueBox");
                DialogueManager scriptA = gameObjectA.GetComponent<DialogueManager>();
                bool check = scriptA.talkedToJessica;
                if (gameObject.name == "FrontDesk_Boss" && check == true) {
                    ac.alert = 0;
                    enabled = false;
                    return;
                }
                if (PuzzleInteract.hasStarted && !PuzzleInteract.hasSolved && !hasCaught) {
                    ac.alert = 100;
                    hasCaught = true;
                    FindObjectOfType<DialogueManager>().OpenDialogue(messagesCaught, actorsCaught, true,
                        (object[] parameters) => {
                            GameObject fadePanel = (GameObject)parameters[0];
                            BackgroundFading fadingScript = (BackgroundFading)parameters[1];
                            fadePanel.SetActive(true);
                            fadingScript.callbackFunction = () => {
                                SceneManager.LoadScene("GameOverAlert");
                            };
                            fadingScript.FadeTo(1f, 1f);
                        }, new object[] {fadePanel, fadingScript});
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
