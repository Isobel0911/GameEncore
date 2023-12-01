using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class NPCController : MonoBehaviour
{
    Animator animator;
    public GameObject MainCharacter;
    public int alertValue;
    public ProgressBar pb;
    public AlertController ac;

    void Start()
    {
        ac = MainCharacter.GetComponent<AlertController>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        float decreaseConstant = 0.002f; // this is changed based on different people, caution factor
        Transform tmp = transform.Find("Root/Hips/Spine_01/Spine_02/Spine_03/Neck/Head");
        if (tmp == null) { tmp = transform; }
        Vector3 MainCharacterPosition = MainCharacter.transform.Find("Skeleton/Hips/Spine/Chest/UpperChest/Neck/Head").position;
        Vector3 myPosition = tmp.position;

        float distance = (myPosition - MainCharacterPosition).magnitude;
        float angle = Vector3.Angle(tmp.forward, MainCharacterPosition - myPosition);
        float coverDistanceOnAngle = angle * (-0.0556f) + 10f;

        
        if (distance <= coverDistanceOnAngle && angle <= 90f) {
            if (CheckObstacle(myPosition, MainCharacterPosition, distance) == true) {
                ac.alert -= decreaseConstant;
                if (ac.alert < 0){
                    ac.alert = 0;
                }
            }
            else {
                // if (Input.GetKey(KeyCode.E)) {
                //     float calculatedAlertValue = (100 * (1f - distance / coverDistanceOnAngle));
                //     if (ac.alert < calculatedAlertValue) {
                //         ac.alert = calculatedAlertValue;
                //     } else if (ac.alert > calculatedAlertValue) {
                //         ac.alert = Math.Max(ac.alert - decreaseConstant, calculatedAlertValue);
                //     }
                // }
                if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
                {
                    float calculatedAlertValue = (50 * (1f - distance / coverDistanceOnAngle));
                    if (ac.alert < calculatedAlertValue) {
                        ac.alert = calculatedAlertValue;
                    } else if (ac.alert > calculatedAlertValue) {
                        ac.alert = Math.Max(ac.alert - decreaseConstant, calculatedAlertValue);
                    }
                }
                else {
                    float calculatedAlertValue = (30 * (1f - distance / coverDistanceOnAngle));
                    if (ac.alert < calculatedAlertValue) {
                        ac.alert = calculatedAlertValue;
                    } else if (ac.alert > calculatedAlertValue) {
                        ac.alert = Math.Max(ac.alert - decreaseConstant, calculatedAlertValue);
                    }
                }
            }
        }
        else {
            ac.alert -= decreaseConstant;
            if (ac.alert < 0){
                ac.alert = 0;
            }
        }
    }

        bool CheckObstacle(Vector3 from, Vector3 to, float distance)
    {
        RaycastHit hit;
        if (Physics.Raycast(from, to - from, out hit, distance))
        {
            if (!hit.collider.CompareTag("MainCharacter"))
            {
                return true;
            }
        }
        return false;
    }
}
