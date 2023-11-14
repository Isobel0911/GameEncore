using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCcontroller : MonoBehaviour
{
    Animator animator;
    public GameObject MainCharacter;
    public float alertValue;

    void Start()
    {
        animator = GetComponent<Animator>();
        MainCharacter = GameObject.Find("MainCharacterPlayerArmature");
        alertValue = 0;
    }

    void Update()
    {
        Vector3 MainCharacterPosition = MainCharacter.transform.position;
        Vector3 myPosition = transform.position;
        float distance = (myPosition - MainCharacterPosition).magnitude;
        if (distance <= 5) {
            alertValue = (5 - distance) * 20;
        }
    }
}
