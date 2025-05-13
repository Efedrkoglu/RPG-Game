using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LeaveDungeon : MonoBehaviour
{
    [SerializeField] private GameObject confirmationPanel;

    public void LeaveDungeonButton() {
        if(!confirmationPanel.activeInHierarchy) confirmationPanel.SetActive(true);
    }

    public void NoButton() {
        confirmationPanel.SetActive(false);
    }
}
