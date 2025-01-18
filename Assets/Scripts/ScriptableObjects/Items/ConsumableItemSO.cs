using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Effect
{
    Heal,
    HealingBuff,
    AttackBuff
}

[CreateAssetMenu(fileName = "Consumable Item", menuName = "ScriptableObjects/Items/Consumable Item")]
public class ConsumableItemSO : ItemSO
{
    public Effect consumeEffect;
    public int healAmount;
    public int attackBonus;
    public int duration;
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

            case Effect.HealingBuff:
                if(Player.Instance.ActionCount >= consumingCost) {
                    CombatSystem.AddBuff(new HealingBuff(duration, healAmount));
                    Player.Instance.ActionCount -= consumingCost;
                    return true;
                }
                break;

            case Effect.AttackBuff:
                if (Player.Instance.ActionCount >= consumingCost) {
                    CombatSystem.AddBuff(new AttackBuff(duration, attackBonus));
                    Player.Instance.ActionCount -= consumingCost;
                    return true;
                }
                break;
        }
        return false;
    }
}
