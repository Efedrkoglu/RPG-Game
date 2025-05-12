using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CombatSystem : MonoBehaviour
{
	private enum BattleState {
		STARTED,
		PLAYERTURN,
		ENEMYTURN,
		LOST,
		WON
	}

	private Player player;
	private Enemy enemy;
	private GameObject playerUnit;
	private GameObject enemyUnit;
	private BattleState state;

	private int enemyDamageInfo;

	[SerializeField] private Transform playerCombatStation;
	[SerializeField] private Transform enemyCombatStation;
	[SerializeField] private TextMeshProUGUI playerHealth;
	[SerializeField] private TextMeshProUGUI playerDamage;
	[SerializeField] private TextMeshProUGUI enemyHealth;
	[SerializeField] private TextMeshProUGUI enemyDamage;
	[SerializeField] private TextMeshProUGUI enemyName;
	[SerializeField] private TextMeshProUGUI info;
	[SerializeField] private SwitchCamera switchCamera;
	[SerializeField] private GameObject combatScreen;
	[SerializeField] private BuffIcon[] buffIcons;

	[SerializeField] private Animator playerVfx, enemyVfx;

    public static List<Buff> activeBuffs;

    public static event Action<bool> CombatEnded;

	private void Awake() {
		PlayerController.CombatTriggered += OnCombatTriggered;
		EnemyController.CombatTriggered += OnCombatTriggered;
		Player.Instance.gameObject.GetComponent<Inventory>().OnInventoryItemUsed += InventoryItemUsed;
		activeBuffs = new List<Buff>(Player.Instance.EffectsCount);
	}

	private void OnDestroy() {
		PlayerController.CombatTriggered -= OnCombatTriggered;
        EnemyController.CombatTriggered -= OnCombatTriggered;
        Player.Instance.gameObject.GetComponent<Inventory>().OnInventoryItemUsed -= InventoryItemUsed;
    }

	private void CombatEndedEvent(bool combatResult) {
		Player.Instance.IsInCombat = false;
        CombatEnded?.Invoke(combatResult);
	}

	private IEnumerator ToggleCombatScreen() {
        yield return new WaitForSeconds(.4f);

        if (combatScreen.activeInHierarchy == false) 
			combatScreen.SetActive(true);
		else
			combatScreen.SetActive(false);
	}

	private void OnCombatTriggered(Enemy enemy, int turn) {
        Player.Instance.IsInCombat = true;
        state = BattleState.STARTED;
		activeBuffs = new List<Buff>();
		for(int i = 0; i < buffIcons.Length; i++) {
			buffIcons[i].gameObject.SetActive(false);
		}
		
		Player.Instance.IsSapphireEffectActive = false;
		Player.Instance.IsEmeraldEffectActive = false;

        playerUnit = Instantiate(Player.Instance.PlayerUnit, playerCombatStation.position, playerCombatStation.rotation);
		playerUnit.GetComponent<PlayerUnit>().OnEnemyHurt += HurtEnemy;

		enemy.setVfxAnimator(enemyVfx);

		enemyUnit = Instantiate(enemy.Unit, enemyCombatStation.position, enemyCombatStation.rotation);
		enemyUnit.GetComponent<EnemyUnit>().OnPlayerHurt += HurtPlayer;
		enemyUnit.GetComponent<EnemyUnit>().setEnemy(enemy);

		player = Player.Instance;
		this.enemy = enemy;

        InitCombatScreen(player, this.enemy);
		switchCamera.TriggerSwitchAnimation();
		StartCoroutine(ToggleCombatScreen());

		if(turn == 0) StartCoroutine(PlayerTurn());
		else if(turn == 1) StartCoroutine(EnemyTurn(false));
		else StartCoroutine(PlayerTurn());
    }

	private void InitCombatScreen(Player player, Enemy enemy) {
		playerHealth.text = player.CurrentHp + "/" + player.getMaxHp();
		playerDamage.text = player.getDamage().ToString();
		playerUnit.GetComponent<HpBar>().InitHpBar(player.getMaxHp(), player.CurrentHp);

        enemyHealth.text = enemy.CurrentHp + "/" + enemy.MaxHp;
		enemyDamage.text = enemy.Damage.ToString();
		enemyName.text = enemy.EnemyName;
		enemyUnit.GetComponent<HpBar>().InitHpBar(enemy.MaxHp, enemy.CurrentHp);

        info.text = "Battle against " + enemy.EnemyName;
        combatScreen.GetComponent<ActionsCounter>()?.UpdateActionsIndicator();
    }

	private void UpdateCombatScreen() {
		playerHealth.text = player.CurrentHp + "/" + player.getMaxHp();
		playerDamage.text = player.getDamage().ToString();
		playerUnit.GetComponent<HpBar>().UpdateHpBar(player.CurrentHp);

		enemyHealth.text = enemy.CurrentHp + "/" + enemy.MaxHp;
		enemyDamage.text = enemy.Damage.ToString();
		enemyUnit.GetComponent<HpBar>().UpdateHpBar(enemy.CurrentHp);

		combatScreen.GetComponent<ActionsCounter>()?.UpdateActionsIndicator();
	}

	private void ExitBattleStation() {
        switchCamera.TriggerSwitchAnimation();
        StartCoroutine(ToggleCombatScreen());

		playerUnit.GetComponent<PlayerUnit>().OnEnemyHurt -= HurtEnemy;
        enemyUnit.GetComponent<EnemyUnit>().OnPlayerHurt -= HurtPlayer;

        Destroy(playerUnit);
		Destroy(enemyUnit);
    }

	private IEnumerator PlayerTurn() {
		yield return new WaitForSeconds(2f);
		player.ActionCount = player.MaxActionCount;
        combatScreen.GetComponent<ActionsCounter>()?.UpdateActionsIndicator();
        info.text = "Your turn";
		yield return new WaitForSeconds(.7f);
        info.text = "Choose an action";
        state = BattleState.PLAYERTURN;
	}

	private IEnumerator EnemyTurn(bool isStunned) {
		if (state == BattleState.STARTED) yield return new WaitForSeconds(2f);

        state = BattleState.ENEMYTURN;

		if(isStunned) {
			info.text = "Enemy is stunned";
			player.IsEmeraldEffectActive = false;
			yield return new WaitForSeconds(1f);
		}
		else {
            info.text = enemy.EnemyName + "'s turn";
            yield return new WaitForSeconds(1f);

            enemy.PlayTurn(enemyUnit, playerUnit);
            info.text = enemy.EnemyName + " dealt " + enemy.GetDamageInfo() + " damage";
        }

        if (player.CurrentHp <= 0) {
			if(player.RubyEquipped == 1) {
				player.gameObject.GetComponent<Inventory>().getEquipmentInventorySO().ClearSlot(5);
				StartCoroutine(RevivePlayer());
			}
			else {
                state = BattleState.LOST;
                playerUnit.GetComponent<Animator>().SetTrigger("Death");
                StartCoroutine(EndBattle());
            }
		}
		else {
			StartCoroutine(PlayerTurn());
		}
	}

	private IEnumerator EndBattle() {
		if(player.SapphireEquipped == 1 || player.EmeraldEquipped == 1)
			player.gameObject.GetComponent<Inventory>().getEquipmentInventorySO().ClearSlot(5);


		if(state == BattleState.WON) {
			info.text = "You defeated the " + enemy.EnemyName;
			yield return new WaitForSeconds(2f);
			info.text = "You won the battle!";
			yield return new WaitForSeconds(2f);

            ExitBattleStation();
            CombatEndedEvent(true);
        }
		else if(state == BattleState.LOST) {
			yield return new WaitForSeconds(2f);
			info.text = enemy.EnemyName + " defeated you";
			yield return new WaitForSeconds(2f);
			info.text = "You lost the battle";
			yield return new WaitForSeconds(2f);

            ExitBattleStation();
            CombatEndedEvent(false);
        }
	}

	private IEnumerator RevivePlayer() {
		playerUnit.GetComponent<Animator>().SetTrigger("Death");
		yield return new WaitForSeconds(2.5f);
        player.CurrentHp = player.getMaxHp() / 2;
        UpdateCombatScreen();
        playerUnit.GetComponent<Animator>().SetTrigger("Revive");
		info.text = "Your ruby revived you!";
		yield return new WaitForSeconds(1f);
        StartCoroutine(PlayerTurn());
    }

	public void AttackButton() {
		if(state != BattleState.PLAYERTURN) return;

		StartCoroutine(PlayerAttackMove());
	}

	private IEnumerator PlayerAttackMove() {
		state = BattleState.ENEMYTURN;
        player.ActionCount = 0;
        UpdateCombatScreen();

        if (player.IsSapphireEffectActive) {
            playerUnit.GetComponent<Animator>().SetTrigger("ComboAttack");
            player.IsSapphireEffectActive = false;
			yield return new WaitForSeconds(1f);
        }
		else {
            playerUnit.GetComponent<Animator>().SetTrigger("Attack");
            yield return new WaitForSeconds(.5f);
        }

        UpdateBuffs();

        if(enemy.CurrentHp <= 0) {
            state = BattleState.WON;
            enemyUnit.GetComponent<Animator>().SetTrigger("Death");
            StartCoroutine(EndBattle());
        }
		else {
            if(player.IsEmeraldEffectActive) StartCoroutine(EnemyTurn(true));
			else StartCoroutine(EnemyTurn(false));
        }
    } 

	public void InventoryButton() {
		if(state != BattleState.PLAYERTURN)
			return;

		gameObject.GetComponent<InventoryPanel>().ToggleInventory();
	}

	public void InventoryItemUsed(ConsumableItemSO item) {
		UpdateCombatScreen();
		StartCoroutine(ItemUsingAnimation(item));
		if(player.ActionCount == 0) {
			gameObject.GetComponent<InventoryPanel>().ToggleInventory();
			UpdateBuffs();
            if (enemy.CurrentHp <= 0) {
                state = BattleState.WON;
                enemyUnit.GetComponent<Animator>().SetTrigger("Death");
                StartCoroutine(EndBattle());
            } 
			else {
                StartCoroutine(EnemyTurn(false));
            }
        }
	}

	private IEnumerator ItemUsingAnimation(ConsumableItemSO item) {
		
		gameObject.GetComponent<InventoryPanel>().ToggleInventory();

		if(item.consumeEffect == Effect.Heal || item.consumeEffect == Effect.HealingBuff) {
            playerVfx.SetTrigger("Regeneration");
			yield return new WaitForSeconds(1.5f);
			if(player.ActionCount > 0) gameObject.GetComponent<InventoryPanel>().ToggleInventory();
        }
		else if(item.consumeEffect == Effect.AttackBuff) {
            playerVfx.SetTrigger("Buff");
            yield return new WaitForSeconds(1.5f);
            if (player.ActionCount > 0) gameObject.GetComponent<InventoryPanel>().ToggleInventory();
        }
		else if(item.consumeEffect == Effect.Both) {
            playerVfx.SetTrigger("Regeneration");
            yield return new WaitForSeconds(1.5f);
			playerVfx.SetTrigger("Buff");
			yield return new WaitForSeconds(1.5f);
            if (player.ActionCount > 0) gameObject.GetComponent<InventoryPanel>().ToggleInventory();
        }
	}

	public void UpdateBuffs() {
        if (activeBuffs.Count > 0) {
            foreach (var buff in activeBuffs) {
                buff.ReApplyBuff();

                if (buff.Duration == 0) {
					buff.ClearBuff();
				}
            }

			for(int i = 0; i < activeBuffs.Count; i++) {
				if (activeBuffs[i].Duration != 0) {
					buffIcons[i].SetBuffIcon(activeBuffs[i].Type, activeBuffs[i].Duration, activeBuffs[i].getDescription());
					buffIcons[i].gameObject.SetActive(true);
				}
				else {
					buffIcons[i].gameObject.SetActive(false);
				}
			}
        }
		UpdateCombatScreen();
    }

	public static void AddBuff(Buff buff) {
		activeBuffs.Add(buff);
		if(activeBuffs.Count > Player.Instance.EffectsCount) {
			activeBuffs[0].ClearBuff();
			activeBuffs.RemoveAt(0);
		}
	}

	//PlayerUnit attack animations are calling this function
	private void HurtEnemy() {
		enemy.CurrentHp -= player.getDamage();
		UpdateCombatScreen();
		enemyUnit.GetComponent<Animator>().SetTrigger("Hurt");
    }

    //EnemyUnit attack animations are calling this function
    private void HurtPlayer(bool attackSuccessfull) {
		if(attackSuccessfull) {
			player.CurrentHp -= enemy.GetLastDealtDamage();
			playerUnit.GetComponent<Animator>().SetTrigger("Hurt");
		}
		else {
			//play block sfx & anim
			playerUnit.GetComponent<Animator>().SetTrigger("Block");	
		}
		UpdateCombatScreen();
	}
}
