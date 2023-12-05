using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VaultDoorEnabled : MonoBehaviour, IInteractable {
    public static bool vaultEnabled = false;
    public bool vaultEnabledControl = false;
    private BackgroundFading fadingScript;
    private GameObject fadePanel;
    [SerializeField] private string _prompt;

    public string InteractionPrompt => _prompt;

    // Start is called before the first frame update
    void Awake() {
        vaultEnabled = false;
        vaultEnabledControl = false;
        fadePanel = GameObject.Find("FadePanel");
        fadingScript = fadePanel?.GetComponent<BackgroundFading>();
    }

    // Update is called once per frame
    void Update() {
        vaultEnabled = vaultEnabled || vaultEnabledControl;
    }

    public bool Interact(Interactor interactor) {
        if (vaultEnabled) {
            fadePanel?.SetActive(true);
            fadingScript.callbackFunction = () => {
                SceneManager.LoadScene("GameFinal");
            };
            fadingScript?.FadeTo(1f, 1f);
        }
        return true;
    }
}
