using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CombatSystem : MonoBehaviour
{
	private Player player;
	private Enemy enemy;
	private GameObject playerGO;
	private GameObject enemyGO;

	[SerializeField] private Transform playerCombatStation;
	[SerializeField] private Transform enemyCombatStation;
	[SerializeField] private TextMeshProUGUI playerHealth;
	[SerializeField] private TextMeshProUGUI playerDamage;
	[SerializeField] private TextMeshProUGUI enemyHealth;
	[SerializeField] private TextMeshProUGUI enemyDamage;
	[SerializeField] private TextMeshProUGUI enemyName;
	[SerializeField] private SwitchCamera switchCamera;
	[SerializeField] private GameObject combatScreen;

	private void Awake() {
		PlayerController.CombatTriggered += OnCombatTriggered;
	}

	private void OnDestroy() {
		PlayerController.CombatTriggered -= OnCombatTriggered;
	}

	private IEnumerator ActivateCombatScreen() {
		yield return new WaitForSeconds(.4f);
		combatScreen.SetActive(true);
	}

	public void OnCombatTriggered(GameObject enemyGO, Enemy enemy) {
		playerGO = Instantiate(GameManager.Instance.PlayerCombatUnit, playerCombatStation.position, playerCombatStation.rotation);
		this.enemyGO = Instantiate(enemyGO, enemyCombatStation.position, enemyCombatStation.rotation);

		player = GameManager.Instance.Player;
		this.enemy = enemy;

        InitCombatScreen(player, this.enemy);
		switchCamera.TriggerSwitchAnimation();
		StartCoroutine(ActivateCombatScreen());
    }

	public void InitCombatScreen(Player player, Enemy enemy) {
		playerHealth.text = player.CurrentHp + "/" + player.MaxHp;
		playerDamage.text = player.Damage.ToString();

		enemyHealth.text = enemy.CurrentHp + "/" + enemy.MaxHp;
		enemyDamage.text = enemy.Damage.ToString();
		enemyName.text = enemy.EnemyName;
	}

	public void UpdateCombatScreen() {
		playerHealth.text = player.CurrentHp + "/" + player.MaxHp;
		playerDamage.text = player.Damage.ToString();

		enemyHealth.text = enemy.CurrentHp + "/" + enemy.MaxHp;
		enemyDamage.text = enemy.Damage.ToString();
	}

	public void AttackButton() {
		Debug.Log("Attack button");
	}
}
