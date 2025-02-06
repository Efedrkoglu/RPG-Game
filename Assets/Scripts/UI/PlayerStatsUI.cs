using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatsUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private TextMeshProUGUI expText;
    [SerializeField] private TextMeshProUGUI maxHpText;
    [SerializeField] private TextMeshProUGUI damageText;
    [SerializeField] private TextMeshProUGUI defText;
    [SerializeField] private TextMeshProUGUI blockChanceText;

    [SerializeField] private GameObject levelPointsMenu;

    [SerializeField] private GameObject levelUpIndicator;

    private void OnEnable() {
        SetUI(Player.Instance);

        if (Player.Instance.LevelPoints > 0)
            levelUpIndicator.SetActive(true);
        else
            levelUpIndicator.SetActive(false);
    }

    private void OnDisable() {
        levelUpIndicator.SetActive(false);
    }

    public void SetUI(Player player) {
        levelText.text = player.Level.ToString();
        expText.text = player.CurrentExp.ToString() + "/" + player.MaxExp.ToString();
        maxHpText.text = player.MaxHp.ToString();
        damageText.text = player.Damage.ToString();
        defText.text = player.DefPercent.ToString() + "%";
        blockChanceText.text = player.BlockChance.ToString() + "%";
    }

    public void OpenLevelPointsMenu() {
        levelPointsMenu.SetActive(true);
        gameObject.SetActive(false);
    }
}
