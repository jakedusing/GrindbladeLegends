using System.Collections;
using System.Collections.Generic;
using RPG.Stats;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.UI {
    public class TraitRowUI : MonoBehaviour
    {
        [SerializeField] Trait trait;
        [SerializeField] TextMeshProUGUI valueText;
        [SerializeField] Button minusButton;
        [SerializeField] Button plusButton;

        TraitStore playerTraitStore = null;
        private const string PLAYER = "Player";

        private void Start() {
            playerTraitStore = GameObject.FindGameObjectWithTag(PLAYER).GetComponent<TraitStore>();
            minusButton.onClick.AddListener(() => Allocate(-1));
            plusButton.onClick.AddListener(() => Allocate(+1));
        }

        private void Update() {
            minusButton.interactable = playerTraitStore.CanAssignPoints(trait, -1);
            plusButton.interactable = playerTraitStore.CanAssignPoints(trait, +1);

            valueText.text = playerTraitStore.GetPoints(trait).ToString();
        }

        public void Allocate(int points) {
            playerTraitStore.AssignPoints(trait, points);
        }
    }
}