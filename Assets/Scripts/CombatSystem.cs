using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CombatSystem : MonoBehaviour
{
	private enum CombatState {
		PlayersTurn,
		EnemiesTurn,
		Won,
		Lost
	}

	private CombatState state;
	private Player player;
	private Enemy enemy;

	[SerializeField] private TextMeshProUGUI playerHealth;
	[SerializeField] private TextMeshProUGUI playerDamage;
	[SerializeField] private TextMeshProUGUI enemyHealth;
	[SerializeField] private TextMeshProUGUI enemyDamage;
	[SerializeField] private Image playerHpBar;
	[SerializeField] private Image enemyHpBar;
	[SerializeField] private TextMeshProUGUI enemyName;
	[SerializeField] private GameObject combatScreen;

	private void Awake() {
		PlayerController.CombatTriggered += OnCombatTriggered;
	}

	private void OnDestroy() {
		PlayerController.CombatTriggered -= OnCombatTriggered;
	}

	public void OnCombatTriggered(Enemy enemy) {
		InitCombatScreen(GameManager.Instance.Player, enemy);
		state = CombatState.PlayersTurn;
		combatScreen.SetActive(true);
	}

	public void InitCombatScreen(Player player, Enemy enemy) {
		this.player = player;
		this.enemy = enemy;
		playerHealth.text = player.CurrentHp + "/" + player.MaxHp;
		playerDamage.text = player.Damage.ToString();
		playerHpBar.fillAmount = player.CurrentHp / (float)player.MaxHp;

		enemyHealth.text = enemy.CurrentHp + "/" + enemy.MaxHp;
		enemyDamage.text = enemy.Damage.ToString();
		enemyHpBar.fillAmount = enemy.CurrentHp / (float)enemy.MaxHp;
		enemyName.text = enemy.Name;
	}

	public void UpdateCombatScreen() {
		playerHealth.text = player.CurrentHp + "/" + player.MaxHp;
		playerHpBar.fillAmount = player.CurrentHp / (float)player.MaxHp;

		enemyHealth.text = enemy.CurrentHp + "/" + enemy.MaxHp;
		enemyHpBar.fillAmount = enemy.CurrentHp / (float)enemy.MaxHp;
	}

	public void AttackButton() {
		enemy.CurrentHp -= player.Damage;
		UpdateCombatScreen();
	}
}
