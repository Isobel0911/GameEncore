using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[System.Serializable]
public class Waypoint {
    public GameObject waypoint;
    public float endTurnAngle;
    public float waitingTime;
    public int animatioWaitingTime;
    public int animationMode;
    public float audioWaitingTime;
    public AudioClip[] audioClips;

    public Waypoint(GameObject waypoint, float endTurnAngle, float waitingTime,
                    int animatioWaitingTime, int animationMode,
                    float audioWaitingTime, AudioClip[] audioClips) {
        this.waypoint = waypoint;
        this.endTurnAngle = endTurnAngle;
        this.waitingTime = waitingTime;
        this.animatioWaitingTime = animatioWaitingTime;
        this.animationMode = animationMode;
        this.audioWaitingTime = audioWaitingTime;
        this.audioClips = audioClips;
    }
}

public class AIPathNPC : MonoBehaviour {
    public int preset;
    public bool hasPlayer = true;
    public Waypoint[] waypoints;
    private AudioSource audioSource;
    private int currentWaypointIdx = 0;
    private int prevWaypointIdx;

    private NavMeshAgent navMeshAgent;
    private NavMeshObstacle navMeshObstacle;

    private float waitEndTime;
    private bool isWaiting;
    private float rotationStartTime;
    private const float rotationDuration = 0.3f;

    private Transform playerTransform;
    private Animator animator;
    private NPCController npcController;
    private bool pathInvalid = false;

    void Start() {
        npcController = GetComponent<NPCController>();
        if (hasPlayer) playerTransform = GameObject.Find("NestedParentArmature_Unpack/PlayerArmature").transform;
        prevWaypointIdx = waypoints.Length - 1;
        audioSource = gameObject.AddComponent<AudioSource>();
        AISounds aiSounds = gameObject.AddComponent<AISounds>();
        Rigidbody rb = gameObject.GetComponent<Rigidbody>();
        if (rb == null) rb = gameObject.AddComponent<Rigidbody>();
        rb.useGravity = true;
        rb.isKinematic = false;
        rb.mass = 60.0f;
        rb.constraints = RigidbodyConstraints.FreezeRotationX |
                         RigidbodyConstraints.FreezeRotationY |
                         RigidbodyConstraints.FreezeRotationZ;

        if (preset == 0) {
            CapsuleCollider capsuleCollider = gameObject.GetComponent<CapsuleCollider>();
            if (capsuleCollider == null) capsuleCollider = gameObject.AddComponent<CapsuleCollider>();
            capsuleCollider.center = new Vector3(0, 0.93f, 0);
            capsuleCollider.radius = 0.20f;
            capsuleCollider.height = 1.83f;
            capsuleCollider.direction = 1;

            navMeshAgent = gameObject.GetComponent<NavMeshAgent>();
            if (navMeshAgent == null) navMeshAgent = gameObject.AddComponent<NavMeshAgent>();
            navMeshAgent.height = 1.83f;
            navMeshAgent.radius = 0.20f;
            navMeshAgent.speed = 2f;
            navMeshAgent.angularSpeed = 270f;

            MoveToNextWaypoint();
        } else {
            rb.useGravity = false;
            rb.isKinematic = true;
            navMeshObstacle = gameObject.AddComponent<NavMeshObstacle>();
            navMeshObstacle.shape = NavMeshObstacleShape.Capsule;
            navMeshObstacle.center = new Vector3(navMeshObstacle.center.x, 0.92f, navMeshObstacle.center.z);
            navMeshObstacle.height = 1.83f;
            navMeshObstacle.radius = 0.20f;
            navMeshObstacle.carving = true;

            // navMeshObstacle = gameObject.AddComponent<NavMeshObstacle>();
            // navMeshObstacle.shape = NavMeshObstacleShape.Capsule;
            // navMeshObstacle.center = new Vector3(navMeshObstacle.center.x, 0.92f, navMeshObstacle.center.z);
            // navMeshObstacle.height = 1.83f;
            // navMeshObstacle.radius = 0.20f;
            // navMeshObstacle.carving = true;
        }

        animator = GetComponent<Animator>();
        isWaiting = false;
    }

    void Update() {
        if (hasPlayer) updateSoundVolume();
        if (preset > 0) {
            presetting();
            return;
        }
        resetNPCAnimator();
        if (waypoints.Length == 0) return;
        if (!isWaiting) {
            if (!navMeshAgent.pathPending && navMeshAgent.remainingDistance < 0.7f) {
                if (Time.time >= rotationStartTime) {
                    FaceTarget();
                }
            }
            if (!navMeshAgent.pathPending && navMeshAgent.remainingDistance < 0.1f) {
                StartWaiting();
            }
        } else {
            if (Time.time > waitEndTime) {
                StopWaiting();
            }
        }
        if (pathInvalid || !pathCheck()) {
            setNextValidPath();
        }
        UpdateAnimatorSpeed();
    }

    void FaceTarget() {
        var turnTowardNavSteeringTarget = navMeshAgent.steeringTarget;
        Vector3 originalDirection = (turnTowardNavSteeringTarget - transform.position).normalized;
        Quaternion rotation = Quaternion.Euler(0, waypoints[prevWaypointIdx].endTurnAngle, 0);
        Vector3 newDirection = rotation * originalDirection;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(newDirection.x, 0, newDirection.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5);
    }

    private void UpdateAnimatorSpeed() {
        animator.SetFloat("Speed", navMeshAgent.velocity.magnitude);
    }

    private void MoveToNextWaypoint() {
        if (waypoints.Length == 0) return;
        //navMeshObstacle.enabled = false;
        //navMeshAgent.enabled = true;
        setNextValidPath();
        rotationStartTime = Time.time + Mathf.Max(0, navMeshAgent.remainingDistance / navMeshAgent.speed - rotationDuration);
    }

    private void StartWaiting() {
        isWaiting = true;
        //navMeshAgent.enabled = false;
        //navMeshObstacle.enabled = true;
        StartCoroutine(WaitAndPlayNPCAnimation(prevWaypointIdx));
        StartCoroutine(WaitAndPlayNPCAudio(prevWaypointIdx));
        waitEndTime = Time.time + waypoints[prevWaypointIdx].waitingTime;
    }

    private IEnumerator WaitAndPlayNPCAnimation(int idx) {
        if (waypoints[idx].animatioWaitingTime > 0) yield return new WaitForSeconds(waypoints[idx].animatioWaitingTime);
        if (preset < 7) playNPCAnimation(waypoints[idx].animationMode);

    }
    private IEnumerator WaitAndPlayNPCAudio(int idx) {
        if (waypoints[idx].audioClips.Length == 0) yield break;
        if (waypoints[idx].audioWaitingTime > 0) yield return new WaitForSeconds(waypoints[idx].audioWaitingTime);
        audioSource.PlayOneShot(waypoints[idx].audioClips[Random.Range(0, waypoints[idx].audioClips.Length)]);
    }
    private IEnumerator presetWaiting(int idx) {
        if (isWaiting || waypoints.Length == 0) yield break;
        isWaiting = true;
        if (waypoints[idx].waitingTime > 0) yield return new WaitForSeconds(waypoints[idx].waitingTime);
        isWaiting = false;
        StartCoroutine(WaitAndPlayNPCAudio(idx));
        if (preset == 7) {
            animator.SetBool("Type", !animator.GetBool("Type"));
            npcController.enabled = !animator.GetBool("Type");
        }
    }

    private void StopWaiting() {
        isWaiting = false;
        MoveToNextWaypoint();
    }
    private void playNPCAnimation(int mode) {
        // some animations
        switch (mode) {    
            case 0:
                animator.SetTrigger("Relax");
                animator.SetInteger("RelaxIdx", 0);
                break;
            case 1:
                animator.SetTrigger("Relax");
                animator.SetInteger("RelaxIdx", 1);
                break;
        }
    }
    private void resetNPCAnimator() {
        animator.ResetTrigger("Relax");
        animator.SetInteger("RelaxIdx", -1);
    }

    private void presetting() {
        switch (preset) {    
            case 1:
            case 2:
            case 3:
            case 4:
                animator.SetInteger("Sit", preset);
                break;
            case 5:
            case 6:
                animator.SetInteger("Death", preset - 4);
                break;
            case 7:
                animator.SetInteger("Sit", 1);
                break;
        }
        if (!isWaiting){
            StartCoroutine(presetWaiting(currentWaypointIdx));
            if (waypoints.Length != 0) currentWaypointIdx = (currentWaypointIdx + 1) % waypoints.Length;
        }
    }

    private void updateSoundVolume() {
        float distance = Vector3.Distance(transform.position, playerTransform.position);
        distance = Mathf.Clamp(distance, 0.01f, 10.0f);

        audioSource.volume = 1 - ((distance - 01f) / (10.0f - 01f));
    }

    private bool pathCheck() {
        return !(navMeshAgent.pathStatus == NavMeshPathStatus.PathInvalid ||
                 navMeshAgent.pathStatus == NavMeshPathStatus.PathPartial);
    }

    private void setNextValidPath() {
        int count = 0;
        do {
            navMeshAgent.SetDestination(waypoints[currentWaypointIdx].waypoint.transform.position);
            currentWaypointIdx = (currentWaypointIdx + 1) % waypoints.Length;
            prevWaypointIdx = (prevWaypointIdx + 1) % waypoints.Length;
            count++;
            if (count > waypoints.Length + 3) {
                pathInvalid = true;
                navMeshAgent.SetDestination(transform.position);
                return;
            }
        } while (!pathCheck());
        pathInvalid = false;
    }
}
