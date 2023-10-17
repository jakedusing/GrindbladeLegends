using System.Collections;
using System.Collections.Generic;
using RPG.Inventories;
using TMPro;
using UnityEngine;

namespace RPG.UI {
    public class PurseUI : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI balanceField;

        Purse playerPurse = null;
        private const string PLAYER = "Player";

        // Start is called before the first frame update
        void Start()
        {
            playerPurse = GameObject.FindGameObjectWithTag(PLAYER).GetComponent<Purse>();

            if (playerPurse != null) {
                playerPurse.onChange += RefreshUI;
            }

            RefreshUI();
        }

        private void RefreshUI() {
            balanceField.text = $"${playerPurse.GetBalance():N2}";
        }
    }
}