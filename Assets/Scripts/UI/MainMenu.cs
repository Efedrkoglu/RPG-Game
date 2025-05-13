using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject loadingScreen;

    public async void NewGameButton() {
        loadingScreen.SetActive(true);

        PlayerPrefs.SetFloat("playerXPos", -0.74f);
        PlayerPrefs.SetFloat("playerYPos", 0.25f);
        PlayerPrefs.SetFloat("playerZPos", -1.454f);

        var scene = SceneManager.LoadSceneAsync("Town");
        scene.allowSceneActivation = false;

        while(scene.progress < .9f) {
            await Task.Yield();
        }

        await Task.Delay(Random.Range(500, 1500));
        scene.allowSceneActivation = true;
    }
}
