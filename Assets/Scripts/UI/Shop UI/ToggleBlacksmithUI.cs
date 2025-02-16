using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleBlacksmithUI : MonoBehaviour
{
    [SerializeField] private GameObject gameUI;
    [SerializeField] private GameObject blacksmithUI;

    private bool listenInputs;

    private void Start() {
        listenInputs = false;
    }

    private void Update() {
        if (!listenInputs)
            return;

        if (Input.GetKeyDown(KeyCode.E)) {
            gameUI.GetComponent<GameScreen>().TogglePressEMessage(false);
            OpenShopUI();
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Player")) {
            listenInputs = true;
            gameUI.GetComponent<GameScreen>().TogglePressEMessage(true, "to Open Blackmisth");
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.gameObject.CompareTag("Player")) {
            listenInputs = false;
            gameUI.GetComponent<GameScreen>().TogglePressEMessage(false);
        }
    }

    public void OpenShopUI() {
        gameUI.SetActive(false);
        blacksmithUI.SetActive(true);
        listenInputs = false;
        blacksmithUI.GetComponent<BlacksmithUI>().setToggleBlacksmithUI(this);
    }

    public void OpenGameUI() {
        blacksmithUI.SetActive(false);
        gameUI.SetActive(true);
        listenInputs = true;
        gameUI.GetComponent<GameScreen>().TogglePressEMessage(true, "to Open Blacksmith");
    }
}
