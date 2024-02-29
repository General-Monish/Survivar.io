using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
   public CharacterScriptableObjects characterData;

    float currentHealth;
    float currentRecovery;
    float currentMoveSpeed;
    float currentMight;
    float currentProjectileSpeed;

    // Expeience And Level Of The Player
    [Header("Experience/Level1")]
    public int exp=0;
    public int level = 1;
    public int expeCapping;


    [Serializable]
    public class LevelRange
    {
        public int startLevel;
        public int endLevel;
        public int expCappingIncrease;
    }

    public List<LevelRange> levelRanges;

    private void Awake()
    {
        currentHealth = characterData.MaxHealth;
        currentMoveSpeed = characterData.MoveSpeed;
        currentProjectileSpeed = characterData.ProjectileSpeed;
        currentRecovery = characterData.Recovery;
        currentMight = characterData.Might;
    }

    private void Start()
    {
        //initialising the first exp cap as the first exp capwill increase
        expeCapping = levelRanges[0].expCappingIncrease;
    }

    public void IncreaseExp(int amount)
    {
        exp += amount;
        LevelUpChecker();
    }

    void LevelUpChecker()
    {
        if (exp >= expeCapping)
        {
            level++;
            exp -= expeCapping;

            int expCapIncrease = 0;
            foreach(LevelRange range in levelRanges)
            {
                if(level>=range.startLevel && level <= range.endLevel)
                {
                    expCapIncrease = range.expCappingIncrease;
                    break;
                }
            }
            expeCapping += expCapIncrease;
        }
    }
   
}
