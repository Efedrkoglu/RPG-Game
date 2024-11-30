using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField] private ItemSO item;
    [SerializeField] private int amount;

    private Inventory playerInventory;

    private void Start() {
        playerInventory = null;
    }

    public void InitializeItem(ItemSO _item, int _amount) {
        item = _item;
        amount = _amount;
        GetComponentInChildren<SpriteRenderer>().sprite = item.itemImage;
    }

    private void Update() {
        if (playerInventory == null)
            return;

        if(Input.GetKeyDown(KeyCode.E)) {
            int result = playerInventory.AddItem(this);
            if (result == 0) {
                Destroy(gameObject);
            } else {
                amount = result;
            }
        }
    }

    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.CompareTag("Player")) {
            playerInventory = other.gameObject.GetComponent<Inventory>();
        }
    }

    private void OnTriggerExit(Collider other) {
        if(other.gameObject.CompareTag("Player")) {
            playerInventory = null;
        }
    }

    public ItemSO GetItem {
        get { return item; }
    }

    public int Amount {
        get { return amount; }
        set { amount = value; }
    }
}
