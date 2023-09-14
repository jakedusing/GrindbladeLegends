using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace RPG.Attributes {
    public class HealthDisplay : MonoBehaviour
    {
        private const string PLAYER = "Player";
        Health health;

        private void Awake() {
            health = GameObject.FindWithTag(PLAYER).GetComponent<Health>();
        }

        private void Update() {
            GetComponent<Text>().text = String.Format("{0:0.0}%", health.GetPercentage());
        }
    }
}