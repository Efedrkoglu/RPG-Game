using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchCamera : MonoBehaviour
{
    [SerializeField] private GameObject mainCamera;
    [SerializeField] private GameObject combatCamera;

    private Animator animator;

    private void Start() {
        animator = GetComponent<Animator>();
    }

    public void TriggerSwitchAnimation() {
        animator.SetTrigger("Switch");
    }

    public void ToggleCombatCamera() {
        if(mainCamera.active == true) {
            mainCamera.SetActive(false);
            combatCamera.SetActive(true);
        }
        else {
            combatCamera.SetActive(false);
            mainCamera.SetActive(true);
        }
    }
}
