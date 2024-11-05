using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameScreen : MonoBehaviour
{
    private Player player;

    [SerializeField] private Image hpBar;
    [SerializeField] private TextMeshProUGUI hpText;
    [SerializeField] private GameObject gameScreen;

    private void Awake() {
        PlayerController.CombatTriggered += OnCombatTriggered;
        CombatSystem.CombatEnded += OnCombatEnded;
    }

    private void Start() {
        player = Player.Instance;
        UpdateHpBar();
    }

    private void OnDisable() {
        PlayerController.CombatTriggered -= OnCombatTriggered;
        CombatSystem.CombatEnded -= OnCombatEnded;
    }

    private void UpdateHpBar() {
        hpBar.fillAmount = player.CurrentHp / (float)player.MaxHp;
        hpText.text = player.CurrentHp + "/" + player.MaxHp;
    }

    public void OnCombatTriggered(Enemy enemy) {
        gameScreen.SetActive(false);
    }

    public void OnCombatEnded(bool combatResult) {
        gameScreen.SetActive(true);
        player = Player.Instance;
        UpdateHpBar();
    }

    
}
