using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Unity.AI.Navigation;

[RequireComponent(typeof(NavMeshSurface))]
public class Door : MonoBehaviour, IInteractable {
    [SerializeField] private Animator _doorAnim;
    private bool isClosed;
    private bool isOpened;
    private NavMeshSurface navMeshSurface;

    private AudioSource audSrc;

    [SerializeField] private AudioClip openSound, closeSound;

    [SerializeField] private string _prompt;

    public string InteractionPrompt => _prompt;

    // Specify the rotation axis (the hinge axis)
    public Vector3 rotationAxis = Vector3.up;
    // Specify the rotation speed in degrees per second
    public float rotationSpeed = 90f;
    // Specify the distance from the hinge to the edge of the door
    public float distanceFromHinge = 0.5f;

    private int layerIndex = 0;
    private bool hasChecked = false;
    public bool[] hasFirst;

    void Start() {
        isClosed = true;
        isOpened = false;

        audSrc = GetComponent<AudioSource>();
        GameObject navMeshUpdater = GameObject.Find("NavMeshUpdater");
        navMeshSurface = navMeshUpdater.GetComponent<NavMeshSurface>();
        hasFirst = new bool[2] {false, false};
    }

    public bool Interact(Interactor interactor) {
        var player = interactor.GetComponent<InventorySelf>();
        if (isClosed && player.hasKey1)  {
            _doorAnim.SetBool("isClosed", true);
            _doorAnim.SetBool("isOpened", false);
            isClosed = false;
            isOpened = true;
            if (this.gameObject.name == "SM_Env_Door_11" && !hasFirst[0]) {
                hasFirst[0] = true;
                _doorAnim.SetBool("isClosed", false);
                _doorAnim.SetBool("isOpened", true);
                isClosed = true;
                isOpened = false;
                audSrc.PlayOneShot(closeSound);
                return true;
            }
            if (this.gameObject.name == "SM_Env_Door_12" && !hasFirst[1]) {
                hasFirst[1] = true;
                _doorAnim.SetBool("isClosed", false);
                _doorAnim.SetBool("isOpened", true);
                isClosed = true;
                isOpened = false;
                audSrc.PlayOneShot(closeSound);
                return true;
            }
            audSrc.PlayOneShot(openSound);
        } else if (isOpened)  {
            _doorAnim.SetBool("isOpened", true);
            _doorAnim.SetBool("isClosed", false);
            isOpened = false;
            isClosed = true;
            audSrc.PlayOneShot(closeSound);
        }


        // if (player.hasKey1) {
        //     return true;
        //     // TODO: open the door
        //     //// Calculate the pivot point
        //     //Vector3 pivot = transform.position - transform.TransformDirection(rotationAxis) * distanceFromHinge;

        //     //// Calculate the target rotation based on the rotation speed and time
        //     //Quaternion targetRotation = transform.rotation * Quaternion.Euler(rotationAxis * 90f);
        //     //// Rotate the door gradually over time
        //     //while (transform.rotation != targetRotation)
        //     //{
        //     //    transform.RotateAround(pivot, rotationAxis, rotationSpeed * Time.deltaTime);
        //     //}
        // } else
        // {
        //     return false;
        // }

        return true;
    }

    private void Update() {
        if (_doorAnim.IsInTransition(layerIndex)) {hasChecked = false; return;}
        AnimatorStateInfo stateInfo = _doorAnim.GetCurrentAnimatorStateInfo(layerIndex);

        if (!stateInfo.IsName("Closed") && stateInfo.normalizedTime >= 1.0f && !hasChecked) {
            UpdateNavMesh();
            hasChecked = true;
        } else if (stateInfo.IsName("Closed") || stateInfo.normalizedTime < 1.0f) {
            hasChecked = false;
        }
    }
    public void UpdateNavMesh() {
        navMeshSurface.BuildNavMesh();
    }
}
