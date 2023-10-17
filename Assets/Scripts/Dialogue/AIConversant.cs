using System.Collections;
using System.Collections.Generic;
using RPG.Attributes;
using RPG.Control;
using UnityEngine;

namespace RPG.Dialogue {
    public class AIConversant : MonoBehaviour, IRaycastable
    {
        [SerializeField] Dialogue dialogue;
        [SerializeField] string conversantName;
        public CursorType GetCursorType()
        {
            return CursorType.Dialogue;
        }

        public bool HandleRaycast(PlayerController callingController)
        {
            if (!enabled) return false;
            if (dialogue == null) {
                return false;
            }

            Health health = GetComponent<Health>();
            if (health && health.IsDead()) return false;

            PlayerConversant convo = callingController.GetComponent<PlayerConversant>();
            if (Input.GetMouseButton(0)) {
                convo.StartDialogue(this, dialogue);
            }
            return true;
        }

        public string GetName() {
            return conversantName;
        }
    }
}