using System.Collections;
using System.Collections.Generic;
using RPG.Control;
using UnityEngine;

namespace RPG.Dialogue {
    public class AIConversant : MonoBehaviour, IRaycastable
    {
        [SerializeField] Dialogue dialogue;
        public CursorType GetCursorType()
        {
            return CursorType.Dialogue;
        }

        public bool HandleRaycast(PlayerController callingController)
        {
            if (dialogue == null) {
                return false;
            }
            PlayerConversant convo = callingController.GetComponent<PlayerConversant>();
            if (Input.GetMouseButton(0)) {
                convo.StartDialogue(this, dialogue);
                Debug.Log("convo started");
            }
            return true;
        }
    }
}