using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ErrorText : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI errorText;
    [SerializeField] private GameObject parent;

    public void SetErrorText(string errorText) {
        this.errorText.text = errorText;
    }

    public void Disappear() {
        Destroy(parent.gameObject);
    }
}
