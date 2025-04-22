using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStation : MonoBehaviour
{
    private void Start() {
        GameObject.FindGameObjectWithTag("Player").transform.position = gameObject.transform.position;
    }
}
