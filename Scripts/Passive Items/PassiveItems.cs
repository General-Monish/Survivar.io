using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassiveItems : MonoBehaviour
{
    protected PlayerStats player;
    public PassiveItemsScriptableObjects passiveItemsData;

    protected virtual void ApplyModiFier()
    {

    }

    private void Start()
    {
        player = FindObjectOfType<PlayerStats>();
        ApplyModiFier();
    }
}
