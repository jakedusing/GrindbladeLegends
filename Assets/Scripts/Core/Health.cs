using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Core {
    public class Health : MonoBehaviour 
    {
        private const string DIE = "die";

        [SerializeField] float healthPoints = 100f;
        
        private bool isDead = false;

        public bool IsDead() {
            return isDead;
        }


        public void TakeDamage(float damage) {
            healthPoints = Mathf.Max(healthPoints - damage, 0);
            if(healthPoints == 0) {
                Die(); 
            }
        }

        public void Die() {
            if (isDead) return;

            isDead = true;
            GetComponent<Animator>().SetTrigger(DIE);
            GetComponent<ActionScheduler>().CancelCurrentAction();
        }
    }
}
