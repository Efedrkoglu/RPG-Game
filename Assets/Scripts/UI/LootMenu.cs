using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootMenu : MonoBehaviour
{
    private LootBag currentLootBag;
    [SerializeField] private LootMenuRow[] rows;
    [SerializeField] private GameObject lootMenu;

    private void Start() {
        CloseLootMenu();
    }

    public void ShowMenu(LootBag lootBag) {
        currentLootBag = lootBag;
        for(int i = 0; i < currentLootBag.Loots.Count; i++) {
            rows[i].SetRow(currentLootBag.Loots[i].Item, currentLootBag.Loots[i].Amount);
        }

        lootMenu.SetActive(true);

        for(int i = 0; i < currentLootBag.Loots.Count; i++) {
            rows[i].gameObject.SetActive(true);
        }
    }

    public void Grab() {
        Debug.Log("Grab");
    }

    public void GrabAll() {
        Debug.Log("Grab All");
    }

    public void CloseLootMenu() {
        currentLootBag = null;
        for (int i = 0; i < rows.Length; i++) {
            rows[i].gameObject.SetActive(false);
        }
        lootMenu.SetActive(false);
    }
}
