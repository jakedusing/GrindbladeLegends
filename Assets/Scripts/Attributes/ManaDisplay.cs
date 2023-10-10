using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace RPG.Attributes {
    public class ManaDisplay : MonoBehaviour
    {
        private const string PLAYER = "Player";
        Mana mana;

        private void Awake() {
            mana = GameObject.FindWithTag(PLAYER).GetComponent<Mana>();
        }

        private void Update() {
            GetComponent<Text>().text = String.Format("{0:0}/{1:0}", mana.GetMana(), mana.GetMaxMana());
        }
    }
}
