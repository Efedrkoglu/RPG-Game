using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameScreen : MonoBehaviour
{
    private Player player;

    [SerializeField] private Image hpBar;
    [SerializeField] private TextMeshProUGUI hpText;
    [SerializeField] private TextMeshProUGUI silverCoinText, goldCoinText;
    [SerializeField] private GameObject gameScreen;
    [SerializeField] private GameObject pressEMessage;
    [SerializeField] private GameObject statsPanel;
    [SerializeField] private Animator statsPanelAnimator;
    [SerializeField] private GameObject levelUpIndicator;

    [SerializeField] private GameObject levelCounterUI;

    private void Awake() {
        PlayerController.CombatTriggered += OnCombatTriggered;
        EnemyController.CombatTriggered += OnCombatTriggered;
        CombatSystem.CombatEnded += OnCombatEnded;
    }

    private void Start() {
        player = Player.Instance;
        player.OnHealthChanged += OnPlayerHealthChanged;
        player.UpdateCoins += UpdateCoins;
        player.LevelUp += OnPlayerLevelUp;
        UpdateHpBar();
        UpdateCoins();
    }

    private void Update() {
        if(Input.GetKeyDown(KeyCode.E)) {
            if (pressEMessage.activeInHierarchy)
                pressEMessage.SetActive(false);
        }
    }

    private void OnDestroy() {
        PlayerController.CombatTriggered -= OnCombatTriggered;
        EnemyController.CombatTriggered -= OnCombatTriggered;
        CombatSystem.CombatEnded -= OnCombatEnded;
        player.OnHealthChanged -= OnPlayerHealthChanged;
        player.UpdateCoins -= UpdateCoins;
        player.LevelUp -= OnPlayerLevelUp;
    }

    public void UpdateHpBar() {
        hpBar.fillAmount = player.CurrentHp / (float)player.getMaxHp();
        hpText.text = player.CurrentHp + "/" + player.getMaxHp();
    }

    public void ToggleStatsPanel() {
        levelUpIndicator.SetActive(false);

        if(!statsPanel.activeInHierarchy) {
            statsPanel.SetActive(true);
            statsPanelAnimator.SetTrigger("Open");
        }
        else {
            StartCoroutine(CloseStatsPanel());
        }
    }

    private IEnumerator CloseStatsPanel() {
        statsPanelAnimator.SetTrigger("Close");
        yield return new WaitForSeconds(.4f);
        statsPanel.SetActive(false);
    }

    public void OnPlayerHealthChanged() {
        UpdateHpBar();
    }

    public void OnCombatTriggered(Enemy enemy, int turn) {
        if(levelCounterUI != null) levelCounterUI.SetActive(false);
        gameScreen.SetActive(false);
    }

    public void OnCombatEnded(bool combatResult) {
        if (levelCounterUI != null) levelCounterUI.SetActive(true);
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

    public void UpdateCoins() {
        silverCoinText.text = player.SilverCoin.ToString();
        goldCoinText.text = player.GoldCoin.ToString();
    }

    public void OnPlayerLevelUp() {
        levelUpIndicator.SetActive(true);
    }
}
