using System.Threading;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] private string sceneToBeLoaded;
    [SerializeField] private string description;
    [SerializeField] private GameScreen gameScreen;
    [SerializeField] private GameObject loadingScreen;

    private bool listenInput;

    private void Start() {
        listenInput = false;
    }

    private void Update() {
        if(Input.GetKeyDown(KeyCode.E) && listenInput) {
            LoadScene();
            listenInput = false;
        }
    }

    public async void LoadScene() {
        loadingScreen.SetActive(true);
        if (SceneManager.GetActiveScene().name == "Castle") {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            PlayerPrefs.SetFloat("playerXPos", player.transform.position.x);
            PlayerPrefs.SetFloat("playerYPos", player.transform.position.y);
            PlayerPrefs.SetFloat("playerZPos", player.transform.position.z);
        }

        var scene = SceneManager.LoadSceneAsync(sceneToBeLoaded);
        scene.allowSceneActivation = false;

        do {
            await Task.Yield();
        } while(scene.progress < .9f);

        await Task.Delay(Random.Range(500, 1500));
        scene.allowSceneActivation = true;
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
