using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcController : MonoBehaviour
{
    Animator animator;
    public GameObject MainCharacter;
    public int alertValue;
    public ProgressBar pb;
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        Vector3 MainCharacterPosition = MainCharacter.transform.position;
        Vector3 myPosition = transform.position;
        float distance = (myPosition - MainCharacterPosition).magnitude;
        if (distance <= 5) {
            alertValue = (int)((5 - distance) * 20);
            pb.BarValue = alertValue;
            
        }
    }
}
