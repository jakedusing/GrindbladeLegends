using System;
using System.Collections;
using System.Collections.Generic;
using RPG.Abilities;
using RPG.Control;
using UnityEngine;

namespace RPG.Abilities.Targeting {

    [CreateAssetMenu(fileName = "Delayed Click Targeting", menuName = "Abilities/Targeting/Delayed Click", order = 0)]
    public class DelayedClickTargeting : TargetingStrategy
    {
        [SerializeField] Texture2D cursorTexture;
        [SerializeField] Vector2 cursorHotspot;
        [SerializeField] LayerMask layerMask;
        [SerializeField] float areaAffectRadius;
        [SerializeField] Transform targetingPrefab;

        Transform targetingPrefabInstance = null;

        public override void StartTargeting(AbilityData data, Action finished)
        {
            PlayerController playerController = data.GetUser().GetComponent<PlayerController>();
            playerController.StartCoroutine(Targeting(data, playerController, finished));
        }

        private IEnumerator Targeting(AbilityData data, PlayerController playerController, Action finished) {
            playerController.enabled = false;
            if (targetingPrefabInstance == null) {
                targetingPrefabInstance = Instantiate(targetingPrefab);
            } else {
                targetingPrefabInstance.gameObject.SetActive(true);
            }
            targetingPrefabInstance.localScale = new Vector3(areaAffectRadius * 2, 1, areaAffectRadius * 2);
            while (!data.IsCancelled()) {
                Cursor.SetCursor(cursorTexture, cursorHotspot, CursorMode.Auto);
                RaycastHit raycastHit;
                if (Physics.Raycast(PlayerController.GetMouseRay(), out raycastHit, 1000, layerMask)) {

                    targetingPrefabInstance.position = raycastHit.point;

                    if (Input.GetMouseButton(0)) {
                        //Absorb whole mouse click, prevent player from moving yet
                        while (Input.GetMouseButton(0)) {
                            yield return null;
                        }
                        data.SetTargetedPoint(raycastHit.point);
                        data.SetTargets(GetGameObjectsInRadius(raycastHit.point));
                        break;
                    }
                }
                yield return null;
            }
            targetingPrefabInstance.gameObject.SetActive(false);
            playerController.enabled = true;
            finished();
        }

        private IEnumerable<GameObject> GetGameObjectsInRadius(Vector3 point)
        {
            RaycastHit[] hits = Physics.SphereCastAll(point, areaAffectRadius, Vector3.up, 0);
            foreach (var hit in hits)
            {
                yield return hit.collider.gameObject;
            }
            
        }
    }
}