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
        state = BattleState.STARTED;
		activeBuffs = new List<Buff>();
		for(int i = 0; i < buffIcons.Length; i++) {
			buffIcons[i].gameObject.SetActive(false);
		}
		
		Player.Instance.IsInCombat = true;
		Player.Instance.IsSapphireEffectActive = false;
		Player.Instance.IsEmeraldEffectActive = false;

        playerUnit = Instantiate(Player.Instance.PlayerUnit, playerCombatStation.position, playerCombatStation.rotation);
		playerUnit.GetComponent<PlayerUnit>().OnEnemyHurt += HurtEnemy;

		enemyUnit = Instantiate(enemy.Unit, enemyCombatStation.position, enemyCombatStation.rotation);

		player = Player.Instance;
		this.enemy = enemy;

        InitCombatScreen(player, this.enemy);
		switchCamera.TriggerSwitchAnimation();
		StartCoroutine(ToggleCombatScreen());

		if(turn == 0) StartCoroutine(PlayerTurn());
		else if(turn == 1) StartCoroutine(EnemyTurn());
		else StartCoroutine(PlayerTurn());
    }

	private void InitCombatScreen(Player player, Enemy enemy) {
		playerHealth.text = player.CurrentHp + "/" + player.getMaxHp();
		playerDamage.text = player.getDamage().ToString();
		playerUnit.GetComponent<HpBar>().InitHpBar(player.MaxHp, player.CurrentHp);

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

	private IEnumerator EnemyTurn() {
		if (state == BattleState.STARTED) yield return new WaitForSeconds(2f);

        state = BattleState.ENEMYTURN;
        info.text = enemy.EnemyName + "'s turn";
        yield return new WaitForSeconds(2f);

		enemy.PlayTurn(enemyUnit, playerUnit);
		UpdateCombatScreen();
        info.text = enemy.EnemyName + " dealt " + enemy.LastDealtDamage + " damage";

        if (player.CurrentHp <= 0) {
			if(player.RubyEquipped) {
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
        player.CurrentHp = player.MaxHp / 2;
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
        player.ActionCount = 0;
        UpdateCombatScreen();

        if (player.IsSapphireEffectActive) {
            playerUnit.GetComponent<Animator>().SetTrigger("ComboAttack");
            player.IsSapphireEffectActive = false;
            Debug.Log("Sapphire used");
			yield return new WaitForSeconds(1f);
        }
		else if(player.IsEmeraldEffectActive) {

        }
		else {
            int random = UnityEngine.Random.Range(1, 100);

            if (random < 50) playerUnit.GetComponent<Animator>().SetTrigger("Attack2");
            else playerUnit.GetComponent<Animator>().SetTrigger("Attack");

			yield return new WaitForSeconds(.5f);
        }

        UpdateBuffs();

        if(enemy.CurrentHp <= 0) {
            state = BattleState.WON;
            enemyUnit.GetComponent<Animator>().SetTrigger("Death");
            StartCoroutine(EndBattle());
        }
		else {
            StartCoroutine(EnemyTurn());
        }
    } 

	public void InventoryButton() {
		if(state != BattleState.PLAYERTURN)
			return;

		gameObject.GetComponent<InventoryPanel>().ToggleInventory();
	}

	public void InventoryItemUsed() {
		UpdateCombatScreen();

		if(player.ActionCount == 0) {
			gameObject.GetComponent<InventoryPanel>().ToggleInventory();
			UpdateBuffs();
            if (enemy.CurrentHp <= 0) {
                state = BattleState.WON;
                enemyUnit.GetComponent<Animator>().SetTrigger("Death");
                StartCoroutine(EndBattle());
            } 
			else {
                StartCoroutine(EnemyTurn());
            }
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
}
