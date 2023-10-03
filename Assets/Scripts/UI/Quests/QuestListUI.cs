using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using RPG.Quests;
using UnityEngine;

public class QuestListUI : MonoBehaviour
{

    [SerializeField] QuestItemUI questPrefab;
    QuestList questList;
    private const string PLAYER = "Player";

    // Start is called before the first frame update
    void Start()
    {
        questList = GameObject.FindGameObjectWithTag(PLAYER).GetComponent<QuestList>();
        questList.onUpdate += Redraw;
        Redraw();
    }

    private void Redraw()
    {
        transform.DetachChildren();
        foreach (QuestStatus status in questList.GetStatuses())
        {
            QuestItemUI uiInstance = Instantiate<QuestItemUI>(questPrefab, transform);
            uiInstance.Setup(status);
        }
    }
}
