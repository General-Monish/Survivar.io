using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ExpGems : Pickup,ICollectables
{
    public int expGranted;
    public void Collect()
    {
        Debug.Log("Called Gem");
        PlayerStats player = FindObjectOfType<PlayerStats>();
        player.IncreaseExp(expGranted);
    }
}
