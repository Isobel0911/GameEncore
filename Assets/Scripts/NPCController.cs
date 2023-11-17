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
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        float decreaseConstant = 0.01f; // this is changed based on different people, caution factor
        Vector3 MainCharacterPosition = MainCharacter.transform.Find("Skeleton/Hips/Spine/Chest/UpperChest/Neck/Head").position;
        Vector3 myPosition = transform.Find("Root/Hips/Spine_01/Spine_02/Spine_03/Neck/Head").position;

        float distance = (myPosition - MainCharacterPosition).magnitude;
        float angle = Vector3.Angle(transform.Find("Root/Hips/Spine_01/Spine_02/Spine_03/Neck/Head").forward, MainCharacterPosition - myPosition);
        float coverDistanceOnAngle = angle * (-0.0556f) + 10f;

        
        if (distance <= 10 && angle <= 90f) {
            if (distance > coverDistanceOnAngle) {
                pb.BarValue -= decreaseConstant;
                if (pb.BarValue < 0){
                    pb.BarValue = 0;
                }
            }
            else {
                if (CheckObstacle(myPosition, MainCharacterPosition, distance) == true) {
                    pb.BarValue -= decreaseConstant;
                    if (pb.BarValue < 0){
                        pb.BarValue = 0;
                    }
                }
                else {
                    if (Input.GetKey(KeyCode.E)) {
                        float calculatedAlertValue = (100 * (1f - distance / coverDistanceOnAngle));
                        if (pb.BarValue < calculatedAlertValue) {
                            pb.BarValue = calculatedAlertValue;
                        } else if (pb.BarValue > calculatedAlertValue) {
                            pb.BarValue = Math.Max(pb.BarValue - decreaseConstant, calculatedAlertValue);
                        }
                    }
                    else if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
                    {
                        float calculatedAlertValue = (50 * (1f - distance / coverDistanceOnAngle));
                        if (pb.BarValue < calculatedAlertValue) {
                            pb.BarValue = calculatedAlertValue;
                        } else if (pb.BarValue > calculatedAlertValue) {
                            pb.BarValue = Math.Max(pb.BarValue - decreaseConstant, calculatedAlertValue);
                        }
                    }
                    else {
                        float calculatedAlertValue = (30 * (1f - distance / coverDistanceOnAngle));
                        if (pb.BarValue < calculatedAlertValue) {
                            pb.BarValue = calculatedAlertValue;
                        } else if (pb.BarValue > calculatedAlertValue) {
                            pb.BarValue = Math.Max(pb.BarValue - decreaseConstant, calculatedAlertValue);
                        }
                    }
                }
            }
        }
        else {
            pb.BarValue -= decreaseConstant;
            if (pb.BarValue < 0){
                pb.BarValue = 0;
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
