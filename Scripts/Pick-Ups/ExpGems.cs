using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpGems : MonoBehaviour,ICollectables
{
    public int expGranted;
    public void Collect()
    {
        PlayerStats player = FindObjectOfType<PlayerStats>();
        player.IncreaseExp(expGranted);
        Destroy(gameObject);
    }
}
