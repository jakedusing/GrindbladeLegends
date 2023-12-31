using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using RPG.Core;
using GameDevTV.Saving;
using RPG.Attributes;

namespace RPG.Movement
{
    public class Mover : MonoBehaviour, IAction, ISaveable
{
    [SerializeField] private Transform target;
    [SerializeField] private float maxSpeed = 6f;
    [SerializeField] float maxNavPathLength = 40f;

    private NavMeshAgent navMeshAgent;
    private Health health;

    private const string FORWARD_SPEED = "forwardSpeed";

    private void Awake() {
        navMeshAgent = GetComponent<NavMeshAgent>();
        health = GetComponent<Health>();
    }

    void Update()
    {
        navMeshAgent.enabled = !health.IsDead();

        UpdateAnimator();
    }

    public void StartMoveAction(Vector3 destination, float speedFraction) {
        GetComponent<ActionScheduler>().StartAction(this);
        MoveTo(destination, speedFraction);
    }

    public bool CanMoveTo(Vector3 destination) {
        // calculate whether path length is longer than maxNavPathLength
        NavMeshPath path = new NavMeshPath();
        bool hasPath = NavMesh.CalculatePath(transform.position, destination, NavMesh.AllAreas, path);
        if (!hasPath) return false;
        if (path.status != NavMeshPathStatus.PathComplete) return false;
        if (GetPathLength(path) > maxNavPathLength) return false;

        return true;
    }

    public void MoveTo(Vector3 destination, float speedFraction) {
        navMeshAgent.destination = destination;
        navMeshAgent.speed = maxSpeed * Mathf.Clamp01(speedFraction);
        navMeshAgent.isStopped = false;
    }

    public void Cancel() {
        navMeshAgent.isStopped = true;
    }

    private void UpdateAnimator() {
        Vector3 velocity = navMeshAgent.velocity;
        Vector3 localVelocity = transform.InverseTransformDirection(velocity);
        float speed = localVelocity.z;
        GetComponent<Animator>().SetFloat(FORWARD_SPEED, speed);
    }

    private float GetPathLength(NavMeshPath path)
    {
        float total = 0f;
        if (path.corners.Length < 2) return total;

        for (int i = 0; i < path.corners.Length - 1; i++) {
            total += Vector3.Distance(path.corners[i], path.corners[i + 1]);
        }
        return total;
    }

    public object CaptureState()
    {
        return new SerializableVector3(transform.position);
    }

    public void RestoreState(object state)
    {
        SerializableVector3 position = (SerializableVector3)state;
        navMeshAgent.enabled = false;
        transform.position = position.ToVector();
        navMeshAgent.enabled = true;
        GetComponent<ActionScheduler>().CancelCurrentAction();
    }
}
}
