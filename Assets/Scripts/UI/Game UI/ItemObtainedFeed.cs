using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemObtainedFeed : MonoBehaviour
{
    [SerializeField] private ItemObtainedFeedRow itemObtainedFeedRowPrefab;
    private RectTransform rectTransform;
    
    void Start() {
        Player.Instance.gameObject.GetComponent<Inventory>().getInventorySO().OnAddItem += OnAddItem;
        rectTransform = gameObject.GetComponent<RectTransform>();
    }

    private void OnDestroy() {
        if(Player.Instance.gameObject != null) {
            Player.Instance.gameObject.GetComponent<Inventory>().getInventorySO().OnAddItem -= OnAddItem;
        }
    }

    public void OnAddItem(ItemSO item, int amount) {
        ItemObtainedFeedRow itemObtainedFeedRow = Instantiate(itemObtainedFeedRowPrefab, Vector3.zero, Quaternion.identity);
        itemObtainedFeedRow.transform.SetParent(rectTransform);
        itemObtainedFeedRow.Initialize(item.itemImage, item.itemName, amount);
    }

    public void OnAddCoin(Sprite coinSprite, string coinName, int amount) {
        ItemObtainedFeedRow itemObtainedFeedRow = Instantiate(itemObtainedFeedRowPrefab, Vector3.zero, Quaternion.identity);
        itemObtainedFeedRow.transform.SetParent(rectTransform);
        itemObtainedFeedRow.Initialize(coinSprite, coinName, amount);
    }
}
