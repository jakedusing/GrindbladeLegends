using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace RPG.Stats {
    public class LevelDisplay : MonoBehaviour
    {
        private const string PLAYER = "Player";
        BaseStats baseStats;

        private void Awake() {
            baseStats = GameObject.FindWithTag(PLAYER).GetComponent<BaseStats>();
        }

        private void Update() {
            GetComponent<Text>().text = String.Format("{0:0}", baseStats.GetLevel());
        }
    }
}