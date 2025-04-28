using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleBankUI : MonoBehaviour
{
    [SerializeField] private GameObject jewelryPanel, bankPanel, buttonsPanel;

    public void OpenJewelryPanel() {
        buttonsPanel.SetActive(false);
        bankPanel.SetActive(false);
        jewelryPanel.SetActive(true);
    }

    public void OpenBankPanel() {
        buttonsPanel.SetActive(false);
        jewelryPanel.SetActive(false);
        bankPanel.SetActive(true);
    }

    public void CloseButton() {
        bankPanel.SetActive(false);
        jewelryPanel.SetActive(false);
        buttonsPanel.SetActive(true);
        gameObject.SetActive(false);
    }
}
