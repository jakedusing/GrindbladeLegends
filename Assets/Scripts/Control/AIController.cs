using System;
using System.Collections;
using System.Collections.Generic;
using RPG.Combat;
using RPG.Core;
using RPG.Movement;
using UnityEngine;

namespace RPG.Control {
    public class AIController : MonoBehaviour {

        [SerializeField] float chaseDistance = 5f;
        [SerializeField] float suspicionTime = 3f;
        [SerializeField] PatrolPath patrolPath;
        [SerializeField]  float waypointTolerance = 1f;
        [SerializeField] float waypointDwellTime = 3f;

        private const string PLAYER = "Player";

        private Fighter fighter;
        private Health health;
        private Mover mover;
        private GameObject player;

        private Vector3 guardPosition;
        private float timeSinceLastSawPlayer = Mathf.Infinity;
        private float timeSinceArrivedAtWaypoint = Mathf.Infinity;
        private int currentWaypointIndex = 0;


        private void Start() {
            fighter = GetComponent<Fighter>();
            health = GetComponent<Health>();
            mover = GetComponent<Mover>();
            player = GameObject.FindWithTag(PLAYER);

            guardPosition = transform.position;
        }

        private void Update()
        {

            if (health.IsDead()) return;

            if (InAttackRangeOfPlayer() && fighter.CanAttack(player))
            {
                AttackBehaviour();
            }
            else if (timeSinceLastSawPlayer < suspicionTime)
            {
                //Suspicion State
                SuspicionBehaviour();
            }
            else
            {
                PatrolBehaviour();
            }

            UpdateTimers();
        }

        private void UpdateTimers()
        {
            timeSinceLastSawPlayer += Time.deltaTime;
            timeSinceArrivedAtWaypoint += Time.deltaTime;
        }

        private void PatrolBehaviour() {
            Vector3 nextPosition = guardPosition;

            if (patrolPath != null) {
                if (AtWaypoint()) {
                    timeSinceArrivedAtWaypoint = 0;
                    CycleWaypoint();
                }
                nextPosition = GetCurrentWaypoint();
            }
            if (timeSinceArrivedAtWaypoint > waypointDwellTime) {
                mover.StartMoveAction(nextPosition);
            }
        }

        private bool AtWaypoint() {
            float distanceToWaypoint = Vector3.Distance(transform.position, GetCurrentWaypoint());
            return distanceToWaypoint < waypointTolerance;
        }

        private void CycleWaypoint() {
            currentWaypointIndex = patrolPath.GetNextIndex(currentWaypointIndex);
        }

        private Vector3 GetCurrentWaypoint() {
            return patrolPath.GetWayPoint(currentWaypointIndex);
        }

        private void SuspicionBehaviour() {
            GetComponent<ActionScheduler>().CancelCurrentAction();
        }

        private void AttackBehaviour() {
            timeSinceLastSawPlayer = 0;
            fighter.Attack(player);
        }

        private bool InAttackRangeOfPlayer() {
            GameObject player = GameObject.FindWithTag(PLAYER);
            float distanceToPlayer =  Vector3.Distance(player.transform.position, transform.position);
            return distanceToPlayer < chaseDistance;
        }

        // Called by Unity
        private void OnDrawGizmosSelected() {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, chaseDistance);
        }

    }
}
