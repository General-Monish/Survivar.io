using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    CharacterScriptableObjects characterData;

    public float currentHealth;
    [HideInInspector]
    public float currentRecovery;
    [HideInInspector]
    public float currentMoveSpeed;
    //[HideInInspector]
    public float currentMight;
    [HideInInspector]
    public float currentProjectileSpeed;
    [HideInInspector]
    public float currentmagnet;



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

    InventoryManager inventory;
    public int weaponIndex;
    public int passiveItemIndex;
    public GameObject secWeaponTest;

    public GameObject firstPassiveItemTest;
    public GameObject SecPassiveItemTest;


    private void Awake()
    {
        characterData = CharacterSelecter.GetData();
        CharacterSelecter.Instance.DestroySingelton();

        inventory = FindObjectOfType<InventoryManager>();

        currentHealth = characterData.MaxHealth;
        currentMoveSpeed = characterData.MoveSpeed;
        currentProjectileSpeed = characterData.ProjectileSpeed;
        currentRecovery = characterData.Recovery;
        currentMight = characterData.Might;
        currentmagnet = characterData.Magnet;

        

        // spawning the starting weapon
        SpawnWeapon(characterData.StartingWeapon);
        SpawnPassiveItem(firstPassiveItemTest);
        SpawnPassiveItem(SecPassiveItemTest);
        SpawnWeapon(secWeaponTest);

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
        if (weaponIndex >= inventory.weaponSlots.Count - 1)
        {
            Debug.LogError("Inventory already Full!!");
            return;
        }
        // spawning the starting weapon
        GameObject spawnedWeapon = Instantiate(weapon, transform.position, Quaternion.identity);
        spawnedWeapon.transform.SetParent(transform); // set weapon to achild 
        inventory.AddWeapon(weaponIndex, spawnedWeapon.GetComponent<WeaponController>());

        weaponIndex++;
    }  
    public void SpawnPassiveItem(GameObject passiveItem)
    {
        if (passiveItemIndex >= inventory.PassiveItemsSlots.Count - 1)
        {
            Debug.LogError("Inventory already Full!!");
            return;
        }
        // spawning the starting passive item
        GameObject spawnedpassiveitem = Instantiate(passiveItem, transform.position, Quaternion.identity);
        spawnedpassiveitem.transform.SetParent(transform); // set weapon to achild 
        inventory.AddPassiveItem(passiveItemIndex, spawnedpassiveitem.GetComponent<PassiveItems>());

        passiveItemIndex++;
    }
}
