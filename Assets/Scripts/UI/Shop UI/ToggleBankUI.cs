using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleBankUI : MonoBehaviour
{
    [SerializeField] private GameObject gameUI;
    [SerializeField] private GameObject bankUI;

    private bool listenInputs;

    private void Start() {
        listenInputs = false;
    }

    private void Update() {
        if (!listenInputs)
            return;

        if (Input.GetKeyDown(KeyCode.E)) {
            gameUI.GetComponent<GameScreen>().TogglePressEMessage(false);
            OpenBankUI();
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Player")) {
            listenInputs = true;
            gameUI.GetComponent<GameScreen>().TogglePressEMessage(true, "Open Bank");
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.gameObject.CompareTag("Player")) {
            listenInputs = false;
            gameUI.GetComponent<GameScreen>().TogglePressEMessage(false);
        }
    }

    public void OpenBankUI() {
        gameUI.SetActive(false);
        bankUI.SetActive(true);
        listenInputs = false;
        bankUI.GetComponent<BankUI>().setToggleBankUI(this);
    }

    public void OpenGameUI() {
        bankUI.SetActive(false);
        gameUI.SetActive(true);
        listenInputs = true;
        gameUI.GetComponent<GameScreen>().TogglePressEMessage(true, "Open Bank");
    }
}
