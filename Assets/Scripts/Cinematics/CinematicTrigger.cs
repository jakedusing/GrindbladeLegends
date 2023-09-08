using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

namespace RPG.Cinematics {

    public class CinematicTrigger : MonoBehaviour {

        private bool hasBeenPlayed = false;
        private const string PLAYER = "Player";

        private void OnTriggerEnter(Collider other) {
            if (other.tag == PLAYER && hasBeenPlayed == false) {
                GetComponent<PlayableDirector>().Play();
                hasBeenPlayed = true;
            }
        }
    }
}


