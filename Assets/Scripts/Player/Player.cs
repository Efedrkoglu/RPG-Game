using System;
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

	private int actionCount;
	private int maxActionCount;

	[SerializeField] private GameObject playerUnit;

	public event Action OnHealthChanged;

    private void Awake() {
        if(instance != null && instance != this) {
			Destroy(gameObject);
		}
		else {
			instance = this;
			DontDestroyOnLoad(gameObject);
		}

		maxHp = 100;
		currentHp = maxHp - 50;
		damage = 10;
		gold = 0;
		inventorySize = 10;
		isInCombat = false;
		maxActionCount = 5;
		actionCount = maxActionCount;
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

			OnHealthChanged?.Invoke();
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

	public int ActionCount {
		get { return actionCount; }
		set {
			actionCount = value;
			if (actionCount > maxActionCount)
				actionCount = maxActionCount;
			if (actionCount < 0)
				actionCount = 0;
		}
	}

	public int MaxActionCount {
		get { return maxActionCount; }
	}

	public GameObject PlayerUnit {
		get { return playerUnit; }
	}

}
