using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIPathNPC : MonoBehaviour {
    public GameObject[] waypoints;
    public int[] animationsChoices;
    private int currentWaypointIndex = 0;

    private NavMeshAgent navMeshAgent;
    private NavMeshObstacle navMeshObstacle;

    private float waitEndTime;
    private bool isWaiting;
    private float rotationStartTime;
    private const float rotationDuration = 0.3f;

    private Animator animator;

    void Start() {
        Rigidbody rb = gameObject.AddComponent<Rigidbody>();
        rb.useGravity = true;
        rb.isKinematic = false;
        rb.mass = 60.0f;
        rb.constraints = RigidbodyConstraints.FreezeRotationX |
                         RigidbodyConstraints.FreezeRotationY |
                         RigidbodyConstraints.FreezeRotationZ;
        
        CapsuleCollider capsuleCollider = gameObject.AddComponent<CapsuleCollider>();
        capsuleCollider.center = new Vector3(0, 0.93f, 0);
        capsuleCollider.radius = 0.20f;
        capsuleCollider.height = 1.83f;
        capsuleCollider.direction = 1;

        navMeshAgent = gameObject.AddComponent<NavMeshAgent>();
        navMeshAgent.height = 1.83f;
        navMeshAgent.radius = 0.20f;
        navMeshAgent.speed = 2f;
        navMeshAgent.angularSpeed = 270f;

        // navMeshObstacle = gameObject.AddComponent<NavMeshObstacle>();
        // navMeshObstacle.shape = NavMeshObstacleShape.Capsule;
        // navMeshObstacle.center = new Vector3(navMeshObstacle.center.x, 0.92f, navMeshObstacle.center.z);
        // navMeshObstacle.height = 1.83f;
        // navMeshObstacle.radius = 0.20f;
        // navMeshObstacle.carving = true;

        animator = GetComponent<Animator>();

        MoveToNextWaypoint();
    }

    void Update() {
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
        UpdateAnimatorSpeed();
    }

    void FaceTarget() {
        int x = currentWaypointIndex - 1; if (x < 0) x = waypoints.Length - 1;
        var turnTowardNavSteeringTarget = navMeshAgent.steeringTarget;
        Vector3 originalDirection = (turnTowardNavSteeringTarget - transform.position).normalized;
        float waypointYRotation = waypoints[x].transform.eulerAngles.y;
        Quaternion rotation = Quaternion.Euler(0, waypointYRotation, 0);
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
        navMeshAgent.SetDestination(waypoints[currentWaypointIndex].transform.position);
        currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
        rotationStartTime = Time.time + Mathf.Max(0, navMeshAgent.remainingDistance / navMeshAgent.speed - rotationDuration);
    }

    private void StartWaiting() {
        isWaiting = true;
        //navMeshAgent.enabled = false;
        //navMeshObstacle.enabled = true;
        playAnimation(2);
        waitEndTime = Time.time + Random.Range(12f, 15f);
    }

    private void StopWaiting() {
        isWaiting = false;
        MoveToNextWaypoint();
    }

    private void playAnimation(int mode) {
        // some animations
        
    }
}
