using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropRateManager : MonoBehaviour
{
    [Serializable]
    public class Drop
    {
        public string name;
        public GameObject itemPrefab;
        public float dropRate;
    }

    public List<Drop> drops;

    private void OnDestroy()
    {
        List<Drop> possibleDrops = new List<Drop>(); 

        float randomNumber = UnityEngine.Random.Range(0f, 100f);

        foreach(Drop rate in drops)
        {
            if (randomNumber <= rate.dropRate)
            {
                possibleDrops.Add(rate);
            }
        }
        // checking if there are possible drops
        if (possibleDrops.Count > 0)
        {
            Drop drop = possibleDrops[UnityEngine.Random.Range(0, possibleDrops.Count)];
            Instantiate(drop.itemPrefab, transform.position, Quaternion.identity);
        }
    }
}
