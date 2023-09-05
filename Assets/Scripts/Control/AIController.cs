using System.Collections;
using System.Collections.Generic;
using RPG.Combat;
using RPG.Core;
using RPG.Movement;
using UnityEngine;

namespace RPG.Control {
    public class AIController : MonoBehaviour {

        private const string PLAYER = "Player";

        private Fighter fighter;
        private Health health;
        private Mover mover;
        private GameObject player;

        private Vector3 guardPosition;
        private float timeSinceLastSawPlayer = Mathf.Infinity;

        [SerializeField] float chaseDistance = 5f;
        [SerializeField] float suspicionTime = 3f;

        private void Start() {
            fighter = GetComponent<Fighter>();
            health = GetComponent<Health>();
            mover = GetComponent<Mover>();
            player = GameObject.FindWithTag(PLAYER);

            guardPosition = transform.position;
        }

        private void Update() {

            if (health.IsDead()) return;
            
            if (InAttackRangeOfPlayer() && fighter.CanAttack(player)) {
                timeSinceLastSawPlayer = 0;
                AttackBehaviour();
            } else if (timeSinceLastSawPlayer < suspicionTime){
                //Suspicion State
                SuspicionBehaviour();
            } else {
                GuardBehaviour();
            }

            timeSinceLastSawPlayer += Time.deltaTime;
        }

        private void GuardBehaviour() {
            mover.StartMoveAction(guardPosition);
        }

        private void SuspicionBehaviour() {
            GetComponent<ActionScheduler>().CancelCurrentAction();
        }

        private void AttackBehaviour() {
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
