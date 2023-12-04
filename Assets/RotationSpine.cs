using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationSpine : MonoBehaviour
{
    // Start is called before the first frame update
    private Vector3 rawRotation;
    private Transform spine;
    void Start()
    {
        spine = transform.Find("Root/Hips/Spine_01");
        rawRotation = spine.rotation.eulerAngles;
        // float rotationY = transform.rotation.eulerAngles.y;
        // float rotationZ = transform.rotation.eulerAngles.z;
        // rawRotation = spine.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        
        Vector3 currentRotation = spine.rotation.eulerAngles;
        print(currentRotation);
        //spine.rotation.eulerAngles.x = -120f;

        // Set the new X value
        currentRotation.x = -120f;

        // Apply the modified rotation back to the spine
        spine.rotation = Quaternion.Euler(currentRotation);
        currentRotation = spine.rotation.eulerAngles;
        print(currentRotation);
    }
}
