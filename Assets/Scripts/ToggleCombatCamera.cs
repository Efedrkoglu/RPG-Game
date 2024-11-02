using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleCombatCamera : MonoBehaviour
{
    [SerializeField] private GameObject mainCamera;
    [SerializeField] private GameObject combatCamera;
    [SerializeField] private GameObject combatScreen;

    private Animator switchAnim;

    private void Start() {
        switchAnim = GetComponent<Animator>(); 
    }

    public void TriggerSwitchAnimation() {
        switchAnim.SetTrigger("SwitchCamera");
    }

    public void SwitchCamera() {
        if(mainCamera.active == true) {
            mainCamera.SetActive(false);
            combatScreen.SetActive(true);
            combatCamera.SetActive(true);
        }
        else {
            combatCamera.SetActive(false);
            combatScreen.SetActive(false);
            mainCamera.SetActive(true);
        }
    }

}
