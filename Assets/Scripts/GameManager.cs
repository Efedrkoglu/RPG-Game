using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;

	private Player player;
	[SerializeField] private GameObject playerUnit;

	private void Awake() {
		if(instance != null && instance != this) {
			Destroy(gameObject);
		}
		else {
			instance = this;
			DontDestroyOnLoad(gameObject);
		}
	}

	private void Start() {
		player = new Player(100, 10 ,0);
	}

	public static GameManager Instance {
		get {
			if (instance == null) {
				instance = FindObjectOfType<GameManager>();
				if (instance == null) {
					GameObject singletonObject = new GameObject("GameManager");
					instance = singletonObject.AddComponent<GameManager>();
					DontDestroyOnLoad(singletonObject);
				}
			}
			return instance;
		}
	}

	public Player Player {
		get { return player; }
	}

	public GameObject PlayerUnit {
		get { return playerUnit; }
	}
}
