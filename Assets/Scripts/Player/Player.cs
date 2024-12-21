using System;
using UnityEngine;

public class Player : MonoBehaviour
{
	private static Player instance;
	
	private int level;
	private int maxExp;
	private int currentExp;
	private int maxHp;
	private int currentHp;
	private int damage;
	private int defPercent;
	private int blockChance;
	private int luck;
	private int gold;
	private int inventorySize;
	private bool isInCombat;

	private int actionCount;
	private int maxActionCount;

	[SerializeField] private GameObject playerUnit;

	public event Action OnHealthChanged, EquipmentEquipped;

    private void Awake() {
        if(instance != null && instance != this) {
			Destroy(gameObject);
		}
		else {
			instance = this;
			DontDestroyOnLoad(gameObject);
		}

		InitializePlayer();
	}

	private void InitializePlayer() {
        level = 1;
        maxExp = 100;
        currentExp = 0;
        maxHp = 50;
        currentHp = maxHp;
        damage = 10;
        defPercent = 0;
        blockChance = 0;
		luck = 5;
        gold = 0;
        inventorySize = 10;
        isInCombat = false;
        maxActionCount = 3;
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

	public void OnEquipmentEquipped() {
		EquipmentEquipped?.Invoke();
	}

	public int Level {
		get { return level; }
		set { level = value; }
	}

	public int MaxExp {
		get { return maxExp; }
	}

	public int CurrentExp {
		get { return currentExp; }
		set {
			currentExp = value;

			if (currentExp > maxExp) {
				level++;
				//level up
				currentExp = currentExp % maxExp;
			}
		}
	}

	public int MaxHp {
		get { return maxHp; }
		set { maxHp = value; }
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

	public int DefPercent {
		get { return defPercent; }
		set { defPercent = value; }
	}

	public int BlockChance {
		get { return blockChance; }
		set { blockChance = value; }
	}

	public int Luck {
		get { return luck; }
		set { luck = value; }
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
