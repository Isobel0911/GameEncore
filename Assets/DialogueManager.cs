using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class DialogueManager : MonoBehaviour {
    public Image actorImage;
    public TextMeshProUGUI actorName;
    public TextMeshProUGUI messageText;
    public RectTransform backgroundBox;
    private CanvasGroup canvasGroup = null;
    [HideInInspector] public bool talkedToJessica = false;

    Message[] currentMessages;
    Actor[] currentActors;
    int ActiveMEssage = 0;
    public static bool isActive = false;

    private IEnumerator TransitionToNextScene(float value, bool target) {
        // Optionally, wait for a short duration if needed
        yield return StartCoroutine(FadeOutCanvasGroup(canvasGroup, value, 0.5f, target));
    }

    private IEnumerator FadeOutCanvasGroup(CanvasGroup canvasGroup, float value, float duration, bool target) {
        float startAlpha = canvasGroup.alpha;
        float time = 0;

        while (time < duration) {
            time += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(startAlpha, value, time / duration);
            yield return null;
        }

        if (target) {
            canvasGroup.alpha = value;
        } else {
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
            canvasGroup.alpha = value;
            isActive = false;
        }
    }

    public void OpenDialogue(Message[] messages, Actor[] actors) {
        isActive = true;
        // gameObject.SetActive(true);

        currentMessages = messages;
        currentActors = actors;
        ActiveMEssage = 0;

        DisplayMessage();
        backgroundBox.LeanScale(Vector3.one, 0.5f);
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
        StartCoroutine(TransitionToNextScene(1f, true));
    }

    void DisplayMessage() {
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
        } else {        // close dialogue

            // enable interacting with plants after interacted with Jessica
            if (currentActors.Length > 1 && currentActors[1].name == "Jessica")
            {
                // Debug.Log("this is jess");
                talkedToJessica = true;
            }
            // else
            // {
            //     Debug.Log("this is not jess");
            // }
            
            backgroundBox.LeanScale(Vector3.zero, 0.5f).setEaseInOutExpo();
            StartCoroutine(TransitionToNextScene(0f, false));
        }
    }

    void Awake() {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    void Start() {
        backgroundBox.transform.localScale = Vector3.zero;
    }

    void Update()  {
        // press space to proceed next message
        if (Input.GetKeyDown(KeyCode.Tab) && isActive) {
            NextMessage();
        }
    }
}

