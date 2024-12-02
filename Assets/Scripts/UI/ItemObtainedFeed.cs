using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemObtainedFeed : MonoBehaviour
{
    [SerializeField] private ItemObtainedFeedRow itemObtainedFeedRowPrefab;
    private RectTransform rectTransform;
    
    void Start() {
        Player.Instance.gameObject.GetComponent<Inventory>().getInventorySO().OnAddItem += OnAddItem;
        rectTransform = gameObject.GetComponent<RectTransform>();
    }

    private void OnDestroy() {
        Player.Instance.gameObject.GetComponent<Inventory>().getInventorySO().OnAddItem -= OnAddItem;
    }

    public void OnAddItem(Item item, int amount) {
        ItemObtainedFeedRow itemObtainedFeedRow = Instantiate(itemObtainedFeedRowPrefab, Vector3.zero, Quaternion.identity);
        itemObtainedFeedRow.transform.SetParent(rectTransform);
        itemObtainedFeedRow.Initialize(item.GetItem.itemImage, item.GetItem.itemName, amount);
    }
}
