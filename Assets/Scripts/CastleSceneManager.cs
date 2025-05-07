using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CastleSceneManager : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private Vector3 defaultPlayerPosition;
    
    private void Start() {
        player.transform.position = new Vector3(
                PlayerPrefs.GetFloat("playerXPos", defaultPlayerPosition.x),
                PlayerPrefs.GetFloat("playerYPos", defaultPlayerPosition.y),
                PlayerPrefs.GetFloat("playerZPos", defaultPlayerPosition.z)
            );
    }
}
