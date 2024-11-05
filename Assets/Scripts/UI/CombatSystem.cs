using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
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

	public static event Action<bool> CombatEnded;

	private void Awake() {
		PlayerController.CombatTriggered += OnCombatTriggered;
	}

	private void OnDestroy() {
		PlayerController.CombatTriggered -= OnCombatTriggered;
	}

	private void CombatEndedEvent(bool combatResult) {
		CombatEnded?.Invoke(combatResult);
	}

	private IEnumerator ToggleCombatScreen() {
        yield return new WaitForSeconds(.4f);

        if (combatScreen.active == false) 
			combatScreen.SetActive(true);
		else
			combatScreen.SetActive(false);
	}

	private void OnCombatTriggered(Enemy enemy) {
        state = BattleState.STARTED;
        playerUnit = Instantiate(Player.Instance.PlayerUnit, playerCombatStation.position, playerCombatStation.rotation);
		enemyUnit = Instantiate(enemy.Unit, enemyCombatStation.position, enemyCombatStation.rotation);

		player = Player.Instance;
		this.enemy = enemy;

        InitCombatScreen(player, this.enemy);
		switchCamera.TriggerSwitchAnimation();
		StartCoroutine(ToggleCombatScreen());
		StartCoroutine(PlayerTurn());
    }

	private void InitCombatScreen(Player player, Enemy enemy) {
		playerHealth.text = player.CurrentHp + "/" + player.MaxHp;
		playerDamage.text = player.Damage.ToString();
		playerUnit.GetComponent<HpBar>().InitHpBar(player.MaxHp, player.CurrentHp);

        enemyHealth.text = enemy.CurrentHp + "/" + enemy.MaxHp;
		enemyDamage.text = enemy.Damage.ToString();
		enemyName.text = enemy.EnemyName;
		enemyUnit.GetComponent<HpBar>().InitHpBar(enemy.MaxHp, enemy.CurrentHp);

        info.text = "Battle against " + enemy.EnemyName;
    }

	private void UpdateCombatScreen() {
		playerHealth.text = player.CurrentHp + "/" + player.MaxHp;
		playerDamage.text = player.Damage.ToString();
		playerUnit.GetComponent<HpBar>().UpdateHpBar(player.CurrentHp);

		enemyHealth.text = enemy.CurrentHp + "/" + enemy.MaxHp;
		enemyDamage.text = enemy.Damage.ToString();
		enemyUnit.GetComponent<HpBar>().UpdateHpBar(enemy.CurrentHp);
	}

	private void ExitBattleStation() {
        switchCamera.TriggerSwitchAnimation();
        StartCoroutine(ToggleCombatScreen());
		Destroy(playerUnit);
		Destroy(enemyUnit);
    }

	private IEnumerator PlayerTurn() {
		yield return new WaitForSeconds(2f);
		info.text = "Your turn";
		yield return new WaitForSeconds(1f);
        info.text = "Choose an action";
        state = BattleState.PLAYERTURN;
	}

	private IEnumerator EnemyTurn() {
		yield return new WaitForSeconds(2f);

		player.CurrentHp -= enemy.Damage;
		UpdateCombatScreen();

		enemyUnit.GetComponent<Animator>().SetTrigger("Attack");
        info.text = enemy.EnemyName + " dealt " + enemy.Damage + " damage";

        if (player.CurrentHp <= 0) {
			state = BattleState.LOST;
			playerUnit.GetComponent<Animator>().SetTrigger("Death");
			StartCoroutine(EndBattle());
		}
		else {
			playerUnit.GetComponent<Animator>().SetTrigger("Hurt");
			StartCoroutine(PlayerTurn());
		}
	}

	private IEnumerator EndBattle() {
		if(state == BattleState.WON) {
			info.text = "You killed the " + enemy.EnemyName;
			yield return new WaitForSeconds(2f);
			info.text = "You won the battle!";
			yield return new WaitForSeconds(2f);

            ExitBattleStation();
            CombatEndedEvent(true);
        }
		else if(state == BattleState.LOST) {
			yield return new WaitForSeconds(2f);
			info.text = enemy.EnemyName + " killed you";
			yield return new WaitForSeconds(2f);
			info.text = "You lost the battle";
			yield return new WaitForSeconds(2f);

            ExitBattleStation();
            CombatEndedEvent(false);
        }
	}

	public void AttackButton() {
		if (state != BattleState.PLAYERTURN)
			return;

		enemy.CurrentHp -= player.Damage;
		UpdateCombatScreen();

		playerUnit.GetComponent<Animator>().SetTrigger("Attack");

		if(enemy.CurrentHp <= 0) {
			state = BattleState.WON;
			enemyUnit.GetComponent<Animator>().SetTrigger("Death");
			StartCoroutine(EndBattle());
		}
		else {
			state = BattleState.ENEMYTURN;
			enemyUnit.GetComponent<Animator>().SetTrigger("Hurt");
			info.text = enemy.EnemyName + "'s turn";
			StartCoroutine(EnemyTurn());
		}
	}

	public void HealButton() {
		if (state != BattleState.PLAYERTURN)
			return;

		player.CurrentHp += 10;

		if (player.CurrentHp > player.MaxHp)
			player.CurrentHp = player.MaxHp;

        UpdateCombatScreen();

        state = BattleState.ENEMYTURN;
		info.text = enemy.EnemyName + "'s turn";
		StartCoroutine(EnemyTurn());
	}


}
