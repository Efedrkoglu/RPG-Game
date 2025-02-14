using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleShopUI : MonoBehaviour
{
    [SerializeField] private GameObject gameUI;
    [SerializeField] private GameObject shopUI;

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
            gameUI.GetComponent<GameScreen>().TogglePressEMessage(true, "Open Shop");
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
        shopUI.SetActive(true);
        listenInputs = false;
        shopUI.GetComponent<ShopUI>().setToggleShopUI(this);
    }

    public void OpenGameUI() {
        shopUI.SetActive(false);
        gameUI.SetActive(true);
        listenInputs = true;
        gameUI.GetComponent<GameScreen>().TogglePressEMessage(true, "Open Shop");
    }
}
