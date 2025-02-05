using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelPointsUI : MonoBehaviour
{
    [SerializeField] private GameObject prevWindow;
    [SerializeField] private TextMeshProUGUI levelPointsText, vigorPointsText, strengthPointsText;
    [SerializeField] private Button incVigor, decVigor, incStrength, decStrength, confirmBtn, closeBtn;

    private int levelPoints, vigorPoints, strengthPoints;

    private void OnEnable() {
        levelPoints = Player.Instance.LevelPoints;
        vigorPoints = Player.Instance.VigorPoints;
        strengthPoints = Player.Instance.StrPoints;

        UpdateUI();
    }

    private void UpdateUI() {
        levelPointsText.text = "Level Points: " + levelPoints.ToString();
        vigorPointsText.text = "Points: " + vigorPoints.ToString();
        strengthPointsText.text = "Points: " + strengthPoints.ToString();

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

        if (levelPoints != Player.Instance.LevelPoints || vigorPoints != Player.Instance.VigorPoints || strengthPoints != Player.Instance.StrPoints)
            confirmBtn.interactable = true;
        else
            confirmBtn.interactable = false;
    }

    public void IncVigor() {
        levelPoints--;
        vigorPoints++;
        UpdateUI();
    }

    public void IncStrength() {
        levelPoints--;
        strengthPoints++;
        UpdateUI();
    }

    public void DecVigor() {
        vigorPoints--;
        levelPoints++;
        UpdateUI();
    }

    public void DecStrength() {
        strengthPoints--;
        levelPoints++;
        UpdateUI();
    }

    public void ConfirmBtn() {
        Player.Instance.LevelPoints = levelPoints;
        Player.Instance.VigorPoints = vigorPoints;
        Player.Instance.StrPoints = strengthPoints;
        CloseBtn();
    }

    public void CloseBtn() {
        prevWindow.SetActive(true);
        prevWindow.GetComponent<Animator>().SetTrigger("Open");
        gameObject.SetActive(false);
    }
}
