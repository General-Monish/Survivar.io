using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageIncrease : PassiveItems
{
    protected override void ApplyModiFier()
    {
        // this formula increases the Damage of attacking weapons  
        player.CurrentMight *= 1 + passiveItemsData.Multiplier / 100f;
    }
}
