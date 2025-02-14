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
	private int silverCoin;
	private int goldCoin;
	private int inventorySize;
	private bool isInCombat;

	private int levelPoints;
	private int vigorPoints;
	private int strPoints;

	private int actionCount;
	private int maxActionCount;
	private int effectsCount;

	[SerializeField] private GameObject playerUnit;

	public event Action OnHealthChanged, EquipmentEquipped, UpdateCoins, LevelUp;

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
        level = 0;
        maxExp = 100;
        currentExp = 0;
        maxHp = 50;
        currentHp = maxHp;
        damage = 225;
        defPercent = 0;
        blockChance = 0;
		silverCoin = 500;
        goldCoin = 50;
        inventorySize = 10;
        isInCombat = false;
		levelPoints = 0;
		vigorPoints = 0;
		strPoints = 0;
        maxActionCount = 3;
        actionCount = maxActionCount;
		effectsCount = 3;
    }

    public void OnEquipmentEquipped() {
        EquipmentEquipped?.Invoke();
    }

    public void IncreaseExp(int exp) {
        currentExp += exp;
        while (currentExp >= maxExp) {
            level++;
            levelPoints++;
			LevelUp?.Invoke();
            currentExp -= maxExp;
        }
    }

    public static Player Instance {
		get {
			if(instance == null) {
				instance = FindObjectOfType<Player>();
			}

			return instance;
		}
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
	}

	public int MaxHp {
		get { return maxHp + vigorPoints * 5; }
		set { maxHp = value; }
	}

	public int CurrentHp {
		get { return currentHp; }
		set {
			currentHp = value;

			if (currentHp < 0)
				currentHp = 0;
			if (currentHp > MaxHp)
				currentHp = MaxHp;

			OnHealthChanged?.Invoke();
		}
	}

	public int Damage {
		get { return damage + strPoints * 5; }
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

	public int SilverCoin {
		get { return silverCoin; }
		set { 
			silverCoin = value;
			UpdateCoins?.Invoke();
		}
	}

	public int GoldCoin {
		get { return goldCoin; }
		set {
			goldCoin = value;
            UpdateCoins?.Invoke();
        }
	}

	public int InventorySize {
		get{ return inventorySize; }
		set{ inventorySize = value; }
	}

	public bool IsInCombat {
		get { return isInCombat; }
        set { isInCombat = value; }
	}

	public int LevelPoints {
		get { return levelPoints; }
		set { levelPoints = value; }
	}

	public int VigorPoints {
		get { return vigorPoints; }
		set { vigorPoints = value; }
	}

	public int StrPoints {
		get { return strPoints; }
		set { strPoints = value; }
	}

	public int EffectsCount {
		get { return effectsCount; }
		set { effectsCount = value; }
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
