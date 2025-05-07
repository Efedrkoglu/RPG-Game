using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Gate : MonoBehaviour
{
    [SerializeField] private GameScreen gameScreen;
    [SerializeField] private Transform teleportPoint;
    [SerializeField] private GameObject player, teleportPlayer;
    [SerializeField] private TextMeshProUGUI currentLevelText;
    [SerializeField] private string textToBeDisplayed;

    private bool listenInputs;

    private void Start() {
        listenInputs = false;
    }

    private void LateUpdate() {
        if (!listenInputs) return;

        if (Input.GetKeyDown(KeyCode.E)) {
            listenInputs = false;
            StartCoroutine(TeleportPlayer());
        }
    }

    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.CompareTag("Player")) {
            gameScreen.TogglePressEMessage(true, "to Enter");
            listenInputs = true;
        }
    }

    private void OnTriggerExit(Collider other) {
        if(other.gameObject.CompareTag("Player")) {
            gameScreen.TogglePressEMessage(false);
            listenInputs = false;
        }
    }

    private IEnumerator TeleportPlayer() {
        teleportPlayer.GetComponent<Animator>().SetTrigger("Switch");
        yield return new WaitForSeconds(.6f);
        currentLevelText.text = textToBeDisplayed;
        player.transform.position = teleportPoint.position;
    }
}
