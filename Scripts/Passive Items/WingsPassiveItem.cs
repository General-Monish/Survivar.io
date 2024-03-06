using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WingsPassiveItem : PassiveItems
{
    protected override void ApplyModiFier()
    {
        // this formula increases the speed 
        player.CurrentMoveSpeed *= 1 + passiveItemsData.Multiplier / 100f;
    }
}
