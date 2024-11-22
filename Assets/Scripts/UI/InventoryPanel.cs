using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class InventoryPanel : MonoBehaviour
{
    [SerializeField] private GameObject inventoryPanel;

    void Update() {
        ToggleInventory();
    }

    private void ToggleInventory() {
        if(Input.GetKeyDown(KeyCode.I)) {
            if(!inventoryPanel.activeInHierarchy) {
                inventoryPanel.SetActive(true);
            }
            else {
                inventoryPanel.SetActive(false);
            }
        }
    }
}
