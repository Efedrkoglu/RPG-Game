using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CombatSystem : MonoBehaviour
{
	private GameObject player;
	private GameObject enemy;

	[SerializeField] private TextMeshProUGUI playerHealth;
	[SerializeField] private TextMeshProUGUI playerDamage;
	[SerializeField] private TextMeshProUGUI enemyHealth;
	[SerializeField] private TextMeshProUGUI enemyDamage;
	[SerializeField] private Image playerHpBar;
	[SerializeField] private Image enemyHpBar;
	[SerializeField] private GameObject combatScreen;

	private void Awake() {
		PlayerController.CombatTriggered += OnCombatTriggered;
	}

	private void OnDestroy() {
		PlayerController.CombatTriggered -= OnCombatTriggered;
	}

	public void OnCombatTriggered(Player p, Enemy e) {
		InitCombatScreen(p, e);
		combatScreen.SetActive(true);
	}

	public void InitCombatScreen(Player p, Enemy e) {
		playerHealth.text = p.getCurrentHp().ToString() + "/" + p.getMaxHp();
		playerDamage.text = p.getDamage().ToString();
		playerHpBar.fillAmount = p.getCurrentHp() / p.getMaxHp();

		enemyHealth.text = e.getCurrentHp().ToString() + "/" + e.getMaxHp();
		enemyDamage.text = e.getDamage().ToString();
		enemyHpBar.fillAmount = e.getCurrentHp() / e.getMaxHp();
	}

}
