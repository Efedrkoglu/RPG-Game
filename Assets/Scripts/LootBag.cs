using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootBag : MonoBehaviour
{
    private List<Loot> loots;
    private bool listenInputs;
    private LootMenu lootMenu;

    private void Start() {
        listenInputs = false;
        lootMenu = GameObject.FindGameObjectWithTag("UI").GetComponent<LootMenu>();
    }

    private void Update() {
        if (!listenInputs)
            return;

        if(Input.GetKeyDown(KeyCode.E)) {
            lootMenu.ShowMenu(this);
        }
    }

    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.CompareTag("Player")) {
            listenInputs = true;
        }
    }

    private void OnTriggerExit(Collider other) {
        if(other.gameObject.CompareTag("Player")) {
            listenInputs = false;
        }
    }

    public bool IsEmpty() {
        bool result = true;

        foreach (var item in loots) {
            if (!item.IsEmpty()) {
                result = false;
            }
        }
        return result;
    }

    public void OnLootBagLooted() {
        Destroy(gameObject);
    }

    public List<Loot> Loots {
        get { return loots; }
        set { loots = value; }
    }

    public bool ListenInputs {
        get { return listenInputs; }
        set { listenInputs = value; }
    }
    
}
