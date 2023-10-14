using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameDevTV.Saving;
using RPG.Stats;
using RPG.Core;
using GameDevTV.Utils;
using UnityEngine.Events;

namespace RPG.Attributes {
    public class Health : MonoBehaviour, ISaveable 
    {
        [SerializeField] float regenerationPercentage = 70f;
        [SerializeField] UnityEvent<float> takeDamage;
        public UnityEvent onDie;

        private const string DIE = "die";

        LazyValue<float> healthPoints;
        
        private bool wasDeadLastFrame = false;

        private void Awake() {
            healthPoints = new LazyValue<float>(GetInitialHealth);
        }

        private float GetInitialHealth() {
            return GetComponent<BaseStats>().GetStat(Stat.Health);
        }

        private void Start() {
            //if no calls to healthPoints by this time this method sets it
            healthPoints.ForceInit();
        }

        private void OnEnable() {
            GetComponent<BaseStats>().onLevelUp += RegenerateHealth;
        }

        private void OnDisable() {
            GetComponent<BaseStats>().onLevelUp -= RegenerateHealth;
        }

        public bool IsDead() {
            return healthPoints.value <= 0;
        }


        public void TakeDamage(GameObject instigator, float damage) {

            healthPoints.value = Mathf.Max(healthPoints.value - damage, 0);

            if(IsDead()) {
                onDie.Invoke();
                AwardExperience(instigator); 
            } else {
                takeDamage.Invoke(damage);
            }
            UpdateState();
        }

        public void Heal(float healthToRestore) {
            healthPoints.value = Mathf.Min(healthPoints.value + healthToRestore, GetMaxHealthPoints());
            UpdateState();
        }

        public float GetHealthPoints() {
            return healthPoints.value;
        }

        public float GetMaxHealthPoints() {
            return GetComponent<BaseStats>().GetStat(Stat.Health);
        }

        public float GetPercentage() {
            return 100 * (GetFraction());
        }

        public float GetFraction() {
            return healthPoints.value / GetComponent<BaseStats>().GetStat(Stat.Health);
        }

        public void UpdateState() {
            Animator animator = GetComponent<Animator>();
            // if dead, set die trigger
            if (!wasDeadLastFrame && IsDead()) {
                animator.SetTrigger(DIE);
                GetComponent<ActionScheduler>().CancelCurrentAction();
            }
            // after dying, reset animator
            if (wasDeadLastFrame && !IsDead()) {
                animator.Rebind();
            }

            wasDeadLastFrame = IsDead();
        }

        private void AwardExperience(GameObject instigator) {
            Experience experience = instigator.GetComponent<Experience>();
            if (experience == null) return;

            experience.GainExperience(GetComponent<BaseStats>().GetStat(Stat.ExperienceReward));
        }

        private void RegenerateHealth() {
            float regenHealthPoints = GetComponent<BaseStats>().GetStat(Stat.Health) * 
                                                            (regenerationPercentage / 100);
            healthPoints.value = Mathf.Max(healthPoints.value, regenHealthPoints);
        }

        public object CaptureState()
        {
            return healthPoints.value;
        }

        public void RestoreState(object state)
        {
            healthPoints.value = (float)state;

            UpdateState();
        }
    }
}
