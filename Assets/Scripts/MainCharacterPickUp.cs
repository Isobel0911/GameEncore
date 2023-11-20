using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCharacterPickUp : MonoBehaviour
{
    private AlertController ac;
    private Animator animator;
    void Start()
    {
        //ac = GetComponent<AlertController>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        if (stateInfo.IsName("GrabHigh") || stateInfo.IsName("GrabMedium") ||stateInfo.IsName("GrabLow")) {
            animator.speed = 5.0f;
        }
        else {
            animator.speed = 1.0f;
        }
    }

    public void Collect(float height) {
        //float eyeHeight = 
        //Vector3 MainCharacterPosition = MainCharacter.transform.Find("Skeleton/Hips/Spine/Chest/UpperChest/Neck/Head").position;
        float eyeHeight = transform.Find("Skeleton/Hips/Spine/Chest/UpperChest/Neck/Head").position.y;
        print(eyeHeight);
        print(height);
        // consider different situations
        if (eyeHeight - height < 0) {
            animator.SetTrigger("GrabHigh");
        }
        else if (eyeHeight - height < 1.1f) {
            animator.SetTrigger("GrabMid");
        }
        else {
            animator.SetTrigger("GrabLow");
        }
        
    }
}
