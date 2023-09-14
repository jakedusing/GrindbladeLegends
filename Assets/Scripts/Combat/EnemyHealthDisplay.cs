using System;
using System.Collections;
using System.Collections.Generic;
using RPG.Attributes;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Combat {
    public class EnemyHealthDisplay : MonoBehaviour
    {
        private const string PLAYER = "Player";
        Fighter fighter;

        private void Awake() {
            fighter = GameObject.FindWithTag(PLAYER).GetComponent<Fighter>();
        }

        private void Update() {
            if (fighter.GetTarget() == null) {
                GetComponent<Text>().text = "N/A";
                return;   
            }
            Health health = fighter.GetTarget();
            GetComponent<Text>().text = String.Format("{0:0.0}%", health.GetPercentage());
        }
    }
}
