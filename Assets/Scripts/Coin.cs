using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField] private Sprite silverCoin, goldCoin;
    [SerializeField] private SpriteRenderer sr;

    private CoinType coinType;
    private int amount;

    private ItemObtainedFeed itemObtainedFeed;

    private void Start() {
        itemObtainedFeed = GameObject.FindGameObjectWithTag("UI").GetComponentInChildren<ItemObtainedFeed>();
    }

    public void SetCoin(CoinType coinType, int amount) {
        this.coinType = coinType;

        if (this.coinType == CoinType.Silver)
            sr.sprite = silverCoin;
        else
            sr.sprite = goldCoin;

        this.amount = amount;
    }

    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.CompareTag("Player")) {
            switch(coinType) {
                case CoinType.Silver:
                    Player.Instance.SilverCoin += amount;
                    itemObtainedFeed?.OnAddCoin(silverCoin, "Silver Coin", amount);
                    break;

                case CoinType.Gold:
                    Player.Instance.GoldCoin += amount;
                    itemObtainedFeed?.OnAddCoin(goldCoin, "Gold Coin", amount);
                    break;
            }
            Destroy(gameObject);
        }
    }
}

public enum CoinType
{
    Silver,
    Gold
}
