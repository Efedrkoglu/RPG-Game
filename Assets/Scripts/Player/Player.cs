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
	private bool isDead;

	private int levelPoints;
	private int vigorPoints;
	private int strPoints;

	private int actionCount;
	private int maxActionCount;
	private int effectsCount;

	public int RubyEquipped { get; set; }
    public int SapphireEquipped { get; set; }
    public int EmeraldEquipped { get; set; }

	public bool IsSapphireEffectActive { get; set; }
	public bool IsEmeraldEffectActive { get; set; }

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
        maxHp = 5;
        currentHp = maxHp;
        damage = 10;
        defPercent = 0;
        blockChance = 5;
		silverCoin = 0;
        goldCoin = 0;
        inventorySize = 20;
        isInCombat = false;
		isDead = false;
		levelPoints = 0;
		vigorPoints = 0;
		strPoints = 0;
        maxActionCount = 3;
        actionCount = maxActionCount;
		effectsCount = 3;

        RubyEquipped = 0;
        SapphireEquipped = 0;
        EmeraldEquipped = 0;
    }

    public void OnEquipmentEquipped() {
        EquipmentEquipped?.Invoke();
    }

    public void IncreaseExp(int exp) {
		bool levelUp = false;
        currentExp += exp;
        while (currentExp >= maxExp) {
			levelUp = true;
            level++;
            levelPoints++;
			LevelUp?.Invoke();
            currentExp -= maxExp;
        }

		if(levelUp) {
            currentHp = getMaxHp();
			OnHealthChanged?.Invoke();
        }

		if(level >= 5 && maxExp == 100) {
			maxExp += 50;
		}
		else if(level >= 10 && maxExp == 150) {
			maxExp += 50;
			maxActionCount += 1;
			actionCount = maxActionCount;
		}
		else if(level >= 15 && maxExp == 200) {
			maxExp += 100;
			maxActionCount += 1;
			actionCount = maxActionCount;
			effectsCount += 1;
		}
		else if(level >= 20 && maxExp == 300) {
			maxExp += 100;
			effectsCount += 1;
		}
		else if(level >= 25 && maxExp == 400) {
			maxExp += 150;
		}
		else if(level >= 30 && maxExp == 550) {
			maxExp += 150;
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
		get { return maxHp; }
		set { maxHp = value; }
	}

	public int CurrentHp {
		get { return currentHp; }
		set {
			currentHp = value;

			if (currentHp < 0)
				currentHp = 0;
			if (currentHp > getMaxHp())
				currentHp = getMaxHp();

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

	public bool IsDead {
		get { return isDead; }
		set { isDead = value; }
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

	public int getDamage() {
		return damage + (strPoints * 2);
	}

	public int getMaxHp() {
		return maxHp + (vigorPoints * 5);
	}
}
