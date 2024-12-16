using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class InventoryPanel : MonoBehaviour
{
    [SerializeField] private GameObject inventoryPanel;
    [SerializeField] private Animator inventoryAnimator;

    public static event Action OnInventoryToggleOpen, OnInventoryToggleClose;

    public void ToggleInventory() {
        if(!inventoryPanel.activeInHierarchy) {
            inventoryPanel.SetActive(true);
            OnInventoryToggleOpen?.Invoke();
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
        OnInventoryToggleClose?.Invoke();
    }
}
