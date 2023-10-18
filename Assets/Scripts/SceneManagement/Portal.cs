using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AI;
using RPG.Control;
using RPG.Attributes;


namespace RPG.SceneManagement {

    public class Portal : MonoBehaviour
    {
        enum DestinationIdentifier {
            A, B, C, D, E
        }


        [SerializeField] int sceneToLoad = -1;
        [SerializeField] Transform spawnPoint;
        [SerializeField] DestinationIdentifier destination;
        [SerializeField] float fadeOutTime = 1f;
        [SerializeField] float fadeInTime = 2f;
        [SerializeField] float fadeWaitTime = 0.5f;

        private const string PLAYER = "Player";

        private void OnTriggerEnter(Collider other) {
            if (other.tag == PLAYER) {
                StartCoroutine(Transition());
            }
        }

        private IEnumerator Transition() {

            if (sceneToLoad < 0) {
                Debug.LogError("Scene to load is not set.");
                yield break;
            }

            DontDestroyOnLoad(gameObject);

            Fader fader = FindObjectOfType<Fader>();
            SavingWrapper wrapper = FindObjectOfType<SavingWrapper>();
            PlayerController playerController = GameObject.FindWithTag(PLAYER).GetComponent<PlayerController>();
            playerController.enabled = false;

            yield return fader.FadeOut(fadeOutTime);
            
            wrapper.Save();
            
            yield return SceneManager.LoadSceneAsync(sceneToLoad);
            PlayerController newPlayerController = GameObject.FindWithTag(PLAYER).GetComponent<PlayerController>();
            newPlayerController.enabled = false;
            

            // load current level
            wrapper.Load();
            
            Portal otherPortal = GetOtherPortral();
            UpdatePlayer(otherPortal);
            
            wrapper.Save();

            yield return new WaitForSeconds(fadeWaitTime);
            fader.FadeIn(fadeInTime);

            newPlayerController.enabled = true;
            Destroy(gameObject);
        }

        private void UpdatePlayer(Portal otherPortal)
        {
            GameObject player = GameObject.FindWithTag(PLAYER);
            player.GetComponent<NavMeshAgent>().Warp(otherPortal.spawnPoint.position);
            player.transform.rotation = otherPortal.spawnPoint.rotation;
            Respawner respawner = player.GetComponent<Respawner>();
            respawner.respawnLocation = otherPortal.spawnPoint;
        }

        private Portal GetOtherPortral()
        {
            foreach (Portal portal in FindObjectsOfType<Portal>()) {
                
                if (portal == this) continue;
                if (portal.destination != destination) continue;

                return portal;
            }

            return null;
        }
    }
}