using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class AlertController : MonoBehaviour {
    public ProgressBar pb;
    public float alert = 0;

    public TMP_Text moneyText;
    public int money = 0;
    // Start is called before the first frame update
    private Conversation conversationScript;
    [HideInInspector]public bool triggeredJessica;
    public EventManager eventManager;


    void Start() {
        conversationScript = GameObject.FindObjectOfType<Conversation>();
        triggeredJessica = false;
    }

    // Update is called once per frame
    void Update() {
        moneyText.text = money.ToString();
        pb.BarValue = (int) alert;

        // trigger event
        if (money >= 2000 && !triggeredJessica) {
            triggeredJessica = true;
        }
    }
}
