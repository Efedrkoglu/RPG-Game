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
    [SerializeField] private GameObject pressEMessage;

    private void Awake() {
        PlayerController.CombatTriggered += OnCombatTriggered;
        CombatSystem.CombatEnded += OnCombatEnded;
    }

    private void Start() {
        player = Player.Instance;
        UpdateHpBar();
    }

    private void Update() {
        if(Input.GetKeyDown(KeyCode.E)) {
            if (pressEMessage.activeInHierarchy)
                pressEMessage.SetActive(false);
        }
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

    public void TogglePressEMessage(bool toggle) {
        pressEMessage.SetActive(toggle);
    }

    public void TogglePressEMessage(bool toggle, string message) {
        pressEMessage.GetComponentInChildren<TextMeshProUGUI>().text = message;
        pressEMessage.SetActive(toggle);
    }

}
