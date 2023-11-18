using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour, IInteractable
{
    [SerializeField] private Animator _doorAnim;
    private bool isClosed;
    private bool isOpened;

    [SerializeField] private string _prompt;

    public string InteractionPrompt => _prompt;

    // Specify the rotation axis (the hinge axis)
    public Vector3 rotationAxis = Vector3.up;
    // Specify the rotation speed in degrees per second
    public float rotationSpeed = 90f;
    // Specify the distance from the hinge to the edge of the door
    public float distanceFromHinge = 0.5f;

    void Start()
    {
        isClosed = true;
        isOpened = false;
    }

    public bool Interact(Interactor interactor)
    {
        var player = interactor.GetComponent<Inventory>();
        if (isClosed) 
        {
            _doorAnim.SetBool("isClosed", true);
            _doorAnim.SetBool("isOpened", false);
            isClosed = false;
            isOpened = true;

        } else if (isOpened) 
        {
            _doorAnim.SetBool("isOpened", true);
            _doorAnim.SetBool("isClosed", false);
            isOpened = false;
            isClosed = true;
        }


        if (player.hasKey1)
        {
            

            return true;
            // TODO: open the door
            //// Calculate the pivot point
            //Vector3 pivot = transform.position - transform.TransformDirection(rotationAxis) * distanceFromHinge;

            //// Calculate the target rotation based on the rotation speed and time
            //Quaternion targetRotation = transform.rotation * Quaternion.Euler(rotationAxis * 90f);
            //// Rotate the door gradually over time
            //while (transform.rotation != targetRotation)
            //{
            //    transform.RotateAround(pivot, rotationAxis, rotationSpeed * Time.deltaTime);
            //}
        } else
        {
            return false;
        }

        return true;
    }
}
