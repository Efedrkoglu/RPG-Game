using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    protected string enemyName;
    protected int maxHp;
    protected int currentHp;
    protected int damage;
    protected int exp;
    protected int turnCount;
    protected int lastDealtDamage;
    protected int damageInfo;
    protected List<Loot> loots;

    [SerializeField] protected GameObject unit;
    [SerializeField] protected GameObject lootBagPrefab;
    [SerializeField] protected ItemSO[] drops;
    [SerializeField] protected double[] dropChances;
    [SerializeField] protected int[] maxDropAmount;

    [SerializeField] protected GameObject coinPrefab;
    [SerializeField] protected CoinType coinType;
    [SerializeField] protected int coinAmount;

    protected Animator vfxAnimator;

    protected virtual void Start() {
        CreateDrops();
        turnCount = 0;
        currentHp = maxHp;
    }

    private void CreateDrops() {
        loots = new List<Loot>();
        System.Random rng = new System.Random();

        for(int i = 0; i < drops.Length; i++) {
            double randomNumber = (double)rng.Next(0, 101) / 100;

            if(randomNumber <= dropChances[i]) {
                int dropAmount = rng.Next(1, (maxDropAmount[i] + 1));
                loots.Add(new Loot(drops[i], dropAmount));
            }
        }
    }

    public virtual void PlayTurn(GameObject enemyUnit, GameObject playerUnit) {
        
    }

    protected bool CheckAttack() {
        int random = UnityEngine.Random.Range(1, 100);

        if (random > Player.Instance.BlockChance) return true;
        else {
            if(Player.Instance.SapphireEquipped == 1) Player.Instance.IsSapphireEffectActive = true;
            if(Player.Instance.EmeraldEquipped == 1) Player.Instance.IsEmeraldEffectActive = true;
            return false;
        }
    }

    public int GetLastDealtDamage() {
        return lastDealtDamage;
    }

    protected void SetLastDealtDamage(bool isAttackSuccessfull) {
        if(isAttackSuccessfull) {
            int calculatedDamage = 0;

            if (Player.Instance.DefPercent != 0) calculatedDamage = damage - ((damage * Player.Instance.DefPercent) / 100);
            else calculatedDamage = damage;

            lastDealtDamage = calculatedDamage;
        }
        else lastDealtDamage = 0;
    }

    public int GetDamageInfo() {
        return damageInfo;
    }

    protected void SetDamageInfo(int damageInfo) {
        this.damageInfo = damageInfo;
    }

    public void Die() {
        Player.Instance.IncreaseExp(exp);

        if(loots.Count > 0) {
            GameObject lootBag = Instantiate(lootBagPrefab, new Vector3(transform.position.x, transform.position.y + .1f, transform.position.z), Quaternion.identity);
            lootBag.GetComponent<LootBag>().Loots = loots;    
        }

        GameObject coin = Instantiate(coinPrefab, new Vector3(transform.position.x + .2f, transform.position.y + .1f, transform.position.z - .2f), Quaternion.identity);
        coin.GetComponent<Coin>().SetCoin(coinType, coinAmount);

        StartCoroutine(PlayDeathAnim());
    }

    private IEnumerator PlayDeathAnim() {
        gameObject.GetComponent<Animator>().SetTrigger("Die");
        yield return new WaitForSeconds(3);
        Destroy(gameObject);
    }

    public void setVfxAnimator(Animator vfxAnimator) {
        this.vfxAnimator = vfxAnimator;
    }

    public string EnemyName {
        get { return enemyName; }
    }

    public int MaxHp {
        get { return maxHp; }
    }

    public GameObject Unit {
        get { return unit; }
    }

    public int CurrentHp {
        get { return currentHp; }
        set {
            currentHp = value;

            if (currentHp < 0)
                currentHp = 0;
            else if (currentHp > maxHp)
                currentHp = maxHp;
        }
    }

    public int Damage {
        get { return damage; }
        set { damage = value; }
    }
}
