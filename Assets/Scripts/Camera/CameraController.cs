using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
	private GameObject player;
    [SerializeField] private Vector3 cameraOffset;

	[SerializeField] private float X_rotation;

	private void Start()
	{
		player = GameObject.FindGameObjectWithTag("Player");

        if (player != null) {
            transform.position = player.transform.position + cameraOffset;
            transform.eulerAngles = new Vector3(X_rotation, transform.rotation.y, transform.rotation.z);
        }
    }

	private void Update()
	{
		transform.position = new Vector3(player.transform.position.x, player.transform.position.y + cameraOffset.y, transform.position.z);
	}
}
