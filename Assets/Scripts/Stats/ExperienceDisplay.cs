using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace RPG.Stats {
    public class ExperienceDisplay : MonoBehaviour
    {
        private const string PLAYER = "Player";
        Experience experience;

        private void Awake() {
            experience = GameObject.FindWithTag(PLAYER).GetComponent<Experience>();
        }

        private void Update() {
            GetComponent<Text>().text = String.Format("{0:0}", experience.GetPoints());
        }
    }
}