using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
	private static Player instance;

	private int maxHp;
	private int currentHp;
	private int damage;
	private int gold;
	private int inventorySize;
	private bool isInCombat;

	[SerializeField] private GameObject playerUnit;

    private void Awake() {
        if(instance != null && instance != this) {
			Destroy(gameObject);
		}
		else {
			instance = this;
			DontDestroyOnLoad(gameObject);
		}

		maxHp = 100;
		currentHp = maxHp;
		damage = 10;
		gold = 0;
		inventorySize = 10;
		isInCombat = false;
	}

	public static Player Instance {
		get {
			if(instance == null) {
				instance = FindObjectOfType<Player>();
			}

			return instance;
		}
	}

	public int MaxHp {
		get { return maxHp; }
	}

	public int CurrentHp {
		get { return currentHp; }
		set {
			currentHp = value;

			if (currentHp < 0)
				currentHp = 0;
			if (currentHp > maxHp)
				currentHp = maxHp;
		}
	}

	public int Damage {
		get { return damage; }
		set { damage = value; }
	}

	public int Gold {
		get { return gold; }
		set { gold = value; }
	}

	public int InventorySize {
		get{ return inventorySize; }
		set{ inventorySize = value; }
	}

	public bool IsInCombat {
		get { return isInCombat; }
        set { isInCombat = value; }
    }

	public GameObject PlayerUnit {
		get { return playerUnit; }
	}

}
