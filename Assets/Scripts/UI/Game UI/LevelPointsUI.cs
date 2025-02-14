using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelPointsUI : MonoBehaviour
{
    [SerializeField] private GameScreen gameScreen;
    [SerializeField] private GameObject prevWindow;
    [SerializeField] private TextMeshProUGUI levelPointsText, vigorPointsText, strengthPointsText, costText;
    [SerializeField] private Button incVigor, decVigor, incStrength, decStrength, confirmBtn, closeBtn;

    private int levelPoints, vigorPoints, strengthPoints, cost;

    private void OnEnable() {
        levelPoints = Player.Instance.LevelPoints;
        vigorPoints = Player.Instance.VigorPoints;
        strengthPoints = Player.Instance.StrPoints;
        cost = 0;

        UpdateUI();
    }

    private void UpdateUI() {
        levelPointsText.text = "Level Points: " + levelPoints.ToString();
        vigorPointsText.text = "Points: " + vigorPoints.ToString();
        strengthPointsText.text = "Points: " + strengthPoints.ToString();
        costText.text = "Cost: " + cost.ToString();

        if (levelPoints > 0) {
            incVigor.interactable = true;
            incStrength.interactable = true;
        } else {
            incVigor.interactable = false;
            incStrength.interactable = false;
        }

        if (vigorPoints > 0)
            decVigor.interactable = true;
        else
            decVigor.interactable = false;

        if (strengthPoints > 0)
            decStrength.interactable = true;
        else
            decStrength.interactable = false;

        if (levelPoints != Player.Instance.LevelPoints || vigorPoints != Player.Instance.VigorPoints || strengthPoints != Player.Instance.StrPoints) {
            if (cost <= Player.Instance.GoldCoin)
                confirmBtn.interactable = true;
            else
                confirmBtn.interactable = false;
        }
        else
            confirmBtn.interactable = false;
    }

    public void IncVigor() {
        levelPoints--;
        vigorPoints++;

        if(vigorPoints <= Player.Instance.VigorPoints)
            cost--;

        UpdateUI();
    }

    public void IncStrength() {
        levelPoints--;
        strengthPoints++;

        if (strengthPoints <= Player.Instance.StrPoints)
            cost--;

        UpdateUI();
    }

    public void DecVigor() {
        vigorPoints--;
        levelPoints++;

        if (vigorPoints < Player.Instance.VigorPoints)
            cost++;

        UpdateUI();
    }

    public void DecStrength() {
        strengthPoints--;
        levelPoints++;

        if (strengthPoints < Player.Instance.StrPoints)
            cost++;

        UpdateUI();
    }

    public void ConfirmBtn() {
        Player.Instance.LevelPoints = levelPoints;
        Player.Instance.VigorPoints = vigorPoints;
        Player.Instance.StrPoints = strengthPoints;
        Player.Instance.GoldCoin -= cost;

        gameScreen.UpdateHpBar();

        CloseBtn();
    }

    public void CloseBtn() {
        prevWindow.SetActive(true);
        prevWindow.GetComponent<Animator>().SetTrigger("Open");
        gameObject.SetActive(false);
    }
}
