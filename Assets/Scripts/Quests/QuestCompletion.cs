using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Quests {
    public class QuestCompletion : MonoBehaviour
    {
        [SerializeField] Quest quest;
        [SerializeField] string objective;
        private const string PLAYER = "Player";
        
        public void CompleteObjective() {
            QuestList questList = GameObject.FindGameObjectWithTag(PLAYER).GetComponent<QuestList>();
            questList.CompleteObjective(quest, objective);
        }
    }
}