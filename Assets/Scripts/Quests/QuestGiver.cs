using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace RPG.Quests {
    public class QuestGiver : MonoBehaviour
    {
        [SerializeField] Quest quest;

        private const string PLAYER = "Player";

        public void GiveQuest() {
            QuestList questList = GameObject.FindGameObjectWithTag(PLAYER).GetComponent<QuestList>();
            questList.AddQuest(quest);
        }
    }
}