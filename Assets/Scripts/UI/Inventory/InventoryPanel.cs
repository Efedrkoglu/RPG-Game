using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class InventoryPanel : MonoBehaviour
{
    [SerializeField] private GameObject inventoryPanel;
    [SerializeField] private Animator inventoryAnimator;

    private void Update() {
        if(Input.GetKeyDown(KeyCode.I)) {
            ToggleInventory();
        }
    }

    public void ToggleInventory() {
        if(!inventoryPanel.activeInHierarchy) {
            inventoryPanel.SetActive(true);
            inventoryAnimator.SetTrigger("Open");
        }
        else {
            StartCoroutine(CloseInventory());
        }
    }

    private IEnumerator CloseInventory() {
        inventoryAnimator.SetTrigger("Close");
        yield return new WaitForSeconds(.4f);
        inventoryPanel.SetActive(false);
    }
}
