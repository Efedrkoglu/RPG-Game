using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Consumable Item", menuName = "ScriptableObjects/Items/Consumable Item")]
public class ConsumableItemSO : ItemSO
{
    public Effect consumeEffect;
    public int healAmount;
    public bool onlyConsumableDuringCombat;
    public int consumingCost;

    public virtual void Awake() {
        type = ItemType.Consumable;
    }

    public override bool UseItem() {
        bool result = false;
        if(Player.Instance.IsInCombat) {
            result = UseItemDuringCombat();
            return result;
        }

        switch(consumeEffect) {
            case Effect.Heal:
                Player.Instance.CurrentHp += healAmount;
                result = true;
                break;
        }
        return result;
    }

    public bool UseItemDuringCombat() {
        switch(consumeEffect) {
            case Effect.Heal:
                if (Player.Instance.ActionCount >= consumingCost) {
                    Player.Instance.CurrentHp += healAmount;
                    Player.Instance.ActionCount -= consumingCost;
                    return true;
                }
                break;
        }
        return false;
    }
}

public enum Effect
{
    Heal,
    HealOverTime,
    BuffAttack
}
