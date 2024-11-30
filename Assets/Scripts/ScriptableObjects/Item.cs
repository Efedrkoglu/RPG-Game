using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField] private ItemSO item;
    [SerializeField] private int amount;

    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.CompareTag("Player")) {
            int result = other.gameObject.GetComponent<Inventory>().AddItem(this);
            if(result == 0) {
                Destroy(gameObject);
            }
            else {
                amount = result;
            }
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
