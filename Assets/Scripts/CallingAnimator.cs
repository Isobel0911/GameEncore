using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CallingAnimator : MonoBehaviour
{
    private Animator animator;
    public int sitNum;

    public bool readyToGame = false;
    private SceneTransition sceneTransition;
    public Button start;
    
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        EventManager.OnConversationEnd += GoToGame;
        start?.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetInteger("Sit", sitNum);
        if (readyToGame)
        {
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                EventManager.OnConversationEnd += GoToGame;
            }
        }
    }

    public void GoToGame(object sender, EventArgs e) {
        start?.gameObject.SetActive(true);
        EventManager.OnConversationEnd -= GoToGame;
    }
}
