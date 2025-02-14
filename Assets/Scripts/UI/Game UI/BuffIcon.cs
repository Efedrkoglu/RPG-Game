using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BuffIcon : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Sprite[] buffSprites;
    [SerializeField] private GameObject buffDescription;
    [SerializeField] private TextMeshProUGUI duration;

    public void SetBuffIcon(int index, int _duration, string description) {
        GetComponent<Image>().sprite = buffSprites[index];
        duration.text = _duration.ToString();
        buffDescription.GetComponentInChildren<TextMeshProUGUI>().text = description;
    }

    public void OnPointerEnter(PointerEventData eventData) {
        buffDescription.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData) {
        buffDescription.SetActive(false);
    }
}
