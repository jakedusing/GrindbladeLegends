using System;
using System.Collections;
using System.Collections.Generic;
using GameDevTV.Saving;
using UnityEngine;

namespace RPG.SceneManagement {

    public class SavingWrapper : MonoBehaviour
    {

        [SerializeField] float fadeInTime = 0.2f;
        const string defaultSaveFile = "save";

        private void Awake() {
            StartCoroutine(LoadLastScene());
        }

        IEnumerator LoadLastScene() {
            yield return GetComponent<SavingSystem>().LoadLastScene(defaultSaveFile);
            Fader fader = FindObjectOfType<Fader>();
            fader.FadeOutImmediate();
            yield return fader.FadeIn(fadeInTime);
        }
        
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.L)) {
                Load();
            }
            if (Input.GetKeyDown(KeyCode.S)) {
                Save();
            }
            if (Input.GetKeyDown(KeyCode.Delete)) {
                Delete();
            }
        }

        public void Save()
        {
            GetComponent<SavingSystem>().Save(defaultSaveFile);
        }

        public void Load()
        {
            // call to saving system and tell it to load
            StartCoroutine(GetComponent<SavingSystem>().LoadLastScene(defaultSaveFile));
        }

        public void Delete() {
            GetComponent<SavingSystem>().Delete(defaultSaveFile);
        }
    }
}