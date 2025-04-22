using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] private string sceneToBeLoaded;
    [SerializeField] private string description;
    [SerializeField] private GameScreen gameScreen;

    private bool listenInput;

    private void Start() {
        listenInput = false;
    }

    private void Update() {
        if(Input.GetKeyDown(KeyCode.E) && listenInput) {
            LoadScene();
        }
    }

    private void LoadScene() {
        SceneManager.LoadScene(sceneToBeLoaded);
    }

    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.CompareTag("Player")) {
            gameScreen.TogglePressEMessage(true, description);
            listenInput = true;
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.gameObject.CompareTag("Player")) {
            gameScreen.TogglePressEMessage(false);
            listenInput = false;
        }
    }
}
