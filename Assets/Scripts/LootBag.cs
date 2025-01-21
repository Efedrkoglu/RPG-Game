using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootBag : MonoBehaviour
{
    private List<Loot> loots;
    private bool listenInputs;

    private void Start() {
        listenInputs = false;
    }

    private void Update() {
        if (!listenInputs)
            return;

        if(Input.GetKeyDown(KeyCode.E)) {
            PrintLoots();
        }
    }

    public void PrintLoots() {
        foreach(var loot in loots) {
            Debug.Log(loot.Item.itemName + ", " + loot.Amount);
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

    public List<Loot> Loots {
        get { return loots; }
        set { loots = value; }
    }
}
