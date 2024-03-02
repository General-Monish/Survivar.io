using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public  CharacterScriptableObjects characterData;

    public float currentHealth;
    [HideInInspector]
    public float currentRecovery;
    [HideInInspector]
    public float currentMoveSpeed;
    [HideInInspector]
    public float currentMight;
    [HideInInspector]
    public float currentProjectileSpeed;
    [HideInInspector]
    public float currentmagnet;

    //Spawned Weapon
    public List<GameObject> SpawnedWeapon;

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

    [Header("I-Frames")]
    public float invincivibilityDuration;
    float invincibilityTimer;
    bool isInvincible;

    public List<LevelRange> levelRanges;

    private void Awake()
    {
        characterData = CharacterSelecter.GetData();
        CharacterSelecter.Instance.DestroySingelton();
        currentHealth = characterData.MaxHealth;
        currentMoveSpeed = characterData.MoveSpeed;
        currentProjectileSpeed = characterData.ProjectileSpeed;
        currentRecovery = characterData.Recovery;
        currentMight = characterData.Might;
        currentmagnet = characterData.Magnet;

        // spawning the starting weapon
        Debug.Log("spawnWeaponCalled");
        SpawnWeapon(characterData.StartingWeapon);
    }

    private void Start()
    {
        //initialising the first exp cap as the first exp capwill increase
        expeCapping = levelRanges[0].expCappingIncrease;
    }

    private void Update()
    {
        if (invincibilityTimer>0)
        {
            invincibilityTimer -= Time.deltaTime;
        }else if (isInvincible)
        {
            isInvincible = false;
        }

        Recover();
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

    public void TakeDamage(float dmg)
    {
        if (!isInvincible)
        {
            currentHealth -= dmg;
            invincibilityTimer = invincivibilityDuration;
            isInvincible = true;
            if (currentHealth <= 0)
            {
                Kill();
            }
        }
        
    }

    public void Kill()
    {
        Debug.Log("Player is Dead!!");
    }

    public void RestoreHealth(float amt) // ================  RESTORING HEALTH ===========================================
    {
        if (currentHealth < characterData.MaxHealth)
        {
            currentHealth += amt;
            if (currentHealth > characterData.MaxHealth)
            {
                currentHealth = characterData.MaxHealth;
            }
        }
        
    }

    void Recover()
    {
        if (currentHealth < characterData.MaxHealth)
        {
            currentHealth += currentRecovery * Time.deltaTime;
            if (currentHealth > characterData.MaxHealth)
            {
                currentHealth = characterData.MaxHealth;
            }
        }
    }

    public void SpawnWeapon(GameObject weapon)
    {
        Debug.Log("spawn called");
        // spawning the starting weapon
        GameObject spawnedWeapon = Instantiate(weapon, transform.position, Quaternion.identity);
        Debug.Log("weapon instantitated");
        spawnedWeapon.transform.SetParent(transform); // set weapon to achild 
        Debug.Log("parrent set");
        SpawnedWeapon.Add(spawnedWeapon);// add list to spawned weapons
    }
}
