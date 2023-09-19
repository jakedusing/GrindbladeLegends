using System.Collections;
using System.Collections.Generic;
using RPG.Control;
using RPG.Core;
using UnityEngine;
using UnityEngine.Playables;

namespace RPG.Cinematics {
    
    public class CinematicControlRemover : MonoBehaviour {

        public PlayableDirector director;
        private const string PLAYER = "Player";
        private GameObject player;

        private void Awake() {
            player = GameObject.FindWithTag(PLAYER);
        }

        private void OnEnable() {
            GetComponent<PlayableDirector>().played += DisableControl;
            GetComponent<PlayableDirector>().stopped += EnableControl;
        }

        private void OnDisable() {
            GetComponent<PlayableDirector>().played -= DisableControl;
            GetComponent<PlayableDirector>().stopped -= EnableControl;
        }

        private void DisableControl(PlayableDirector director) {
            
            player.GetComponent<ActionScheduler>().CancelCurrentAction();
            player.GetComponent<PlayerController>().enabled = false;
        }

        private void EnableControl(PlayableDirector director) {
            player.GetComponent<PlayerController>().enabled = true;
        }
    }
}
