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

    public bool conversationEnds = false;

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
    }

    private void Update()
    {
        // Press Tab for next conversation sentence
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (conversationEnds)
            {
                OnConversationEnd?.Invoke(this, EventArgs.Empty);
            }
            else{
                OnConversation?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}
