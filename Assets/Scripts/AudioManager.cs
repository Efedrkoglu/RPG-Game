using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    private static AudioManager instance;
    [SerializeField] private AudioSource audioSourcePrefab;
    [SerializeField] private AudioClip click, cancel;

    private AudioSource audioSource;

    private void Awake() {
        if(instance == null) {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else {
            Destroy(gameObject);
        }
    }

    private void Start() {
        audioSource = gameObject.GetComponent<AudioSource>();
    }

    private void Update() {
        if(Input.GetMouseButtonDown(0)) {
            PlayClickSound();
        }
    }

    public static AudioManager Instance {
        get { return instance; }
    }

    public void PlayAudioClip(AudioClip audioClip, Transform transform) {
        AudioSource audioSource = Instantiate(audioSourcePrefab, transform.position, Quaternion.identity);
        audioSource.clip = audioClip;
        audioSource.volume = PlayerPrefs.GetFloat("SFXvolume", 1f);
        audioSource.Play();
        float clipLength = audioSource.clip.length;
        Destroy(audioSource.gameObject, clipLength);
    }

    public void PlayClickSound() {
        audioSource.clip = click;
        audioSource.Play();
    }
}
