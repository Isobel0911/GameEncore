using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventManager : MonoBehaviour
{
    public static event EventHandler OnConversation;
    public static event EventHandler OnConversationEnd;

    public static EventManager instance;

    // [HideInInspector]public bool conversationEnds = false;
    [HideInInspector]public bool convInProgress = false;
    private AlertController alert;
    [HideInInspector]public bool invokedJessica = false;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        OnConversation?.Invoke(this, EventArgs.Empty);
        alert = GameObject.FindObjectOfType<AlertController>();
    }

    private void Update()
    {
        // Press Tab for next conversation sentence
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (convInProgress)
            {
                // Debug.Log("conv in prog");
                // Debug.Log(OnConversation.GetInvocationList());
                OnConversation?.Invoke(this, EventArgs.Empty);
            }
            else{
                OnConversationEnd?.Invoke(this, EventArgs.Empty);
            }
        }

        if (alert != null && !invokedJessica && alert.triggeredJessica)
        {
            invokedJessica = true;
            // Debug.Log(convInProgress);
            OnConversation?.Invoke(this, EventArgs.Empty);
        }
    }
}
