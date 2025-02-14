using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MouseFollower : MonoBehaviour
{
    private Canvas canvas;
    private InventorySlotUI slot;

    void Start() {
        canvas = transform.root.GetComponent<Canvas>();
        slot = GetComponentInChildren<InventorySlotUI>();
        gameObject.SetActive(false);
    }

    void Update() {
        Vector2 position;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            (RectTransform)canvas.transform,
            Input.mousePosition,
            canvas.worldCamera,
            out position
        );

        transform.position = canvas.transform.TransformPoint(position);
    }

    public void SetData(Sprite sprite, int itemAmount, bool isStackable) {
        slot.SetSlotItem(sprite, itemAmount, isStackable);
    }

    public void Toggle(bool val) {
        gameObject.SetActive(val);
    }
}
