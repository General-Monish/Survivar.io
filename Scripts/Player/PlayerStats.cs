using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{
    CharacterScriptableObjects characterData;

     float currentHealth;
     float currentRecovery;
     float currentMoveSpeed;
     float currentMight;
     float currentProjectileSpeed;
     float currentmagnet;

#region CurrentStat properties
    public float CurrentHealth
    {
        get{return currentHealth; }
        set{
            if (currentHealth != value)
            {
                currentHealth=value;
                if (GameManager.Instance != null)
                {
                    GameManager.Instance.health.text ="Health:"+ currentHealth;
                }
            }
        }
    } 
    public float CurrentRecovery
    {
        get{return currentRecovery; }
        set{
            if (currentRecovery != value)
            {
                currentRecovery=value;
                if (GameManager.Instance != null)
                {
                    GameManager.Instance.recovery.text = "Recovery:" + currentRecovery;
                }
            }
        }
    }
    public float CurrentMight
    {
        get{return currentMight; }
        set{
            if (currentMight != value)
            {
                currentMight=value;
                if (GameManager.Instance != null)
                {
                    GameManager.Instance.might.text = "Might:" + currentMight;
                }
            }
        }
    }  
    public float CurrentMoveSpeed
    {
        get{return currentMoveSpeed; }
        set{
            if (currentMoveSpeed != value)
            {
                currentMoveSpeed=value;
                if (GameManager.Instance != null)
                {
                    GameManager.Instance.moveSpeed.text = "MoveSpeed:" + currentMoveSpeed;
                }
            }
        }
    }  
    public float CurrentMagnet
    {
        get{return currentmagnet; }
        set{
            if (currentmagnet != value)
            {
                currentmagnet=value;
                if (GameManager.Instance != null)
                {
                    GameManager.Instance.magnetRange.text = "MagnetRange:" + currentmagnet;
                }
            }
        }
    }  
    public float CurrentProjectileSpeed
    {
        get{return currentProjectileSpeed; }
        set{
            if (currentProjectileSpeed != value)
            {
                currentProjectileSpeed=value;
                if (GameManager.Instance != null)
                {
                    GameManager.Instance.projectileSpeed.text = "ProjectileSpeed:" + currentProjectileSpeed;
                }
            }
        }
    }

    #endregion


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

    [Header("UI")]
    public Image healthBar;
    public Image ExpBar;
    public TextMeshProUGUI LevelText;

    private void Awake()
    {
        characterData = CharacterSelecter.GetData();
        CharacterSelecter.Instance.DestroySingelton();

        inventory = FindObjectOfType<InventoryManager>();

        CurrentHealth = characterData.MaxHealth;
        CurrentMoveSpeed = characterData.MoveSpeed;
        CurrentProjectileSpeed = characterData.ProjectileSpeed;
        CurrentRecovery = characterData.Recovery;
        CurrentMight = characterData.Might;
        CurrentMagnet = characterData.Magnet;

        

        // spawning the starting weapon
        SpawnWeapon(characterData.StartingWeapon);
        //SpawnPassiveItem(firstPassiveItemTest);
        SpawnPassiveItem(SecPassiveItemTest);
        //SpawnWeapon(secWeaponTest);

    }

    private void Start()
    {
        //initialising the first exp cap as the first exp capwill increase
        expeCapping = levelRanges[0].expCappingIncrease;

        GameManager.Instance.health.text = "Health:" + currentHealth;
        GameManager.Instance.recovery.text = "Recovery:" + currentRecovery;
        GameManager.Instance.might.text = "Might:" + currentMight;
        GameManager.Instance.projectileSpeed.text = "ProjectileSpeed:" + currentProjectileSpeed;
        GameManager.Instance.magnetRange.text = "MagnetRange:" + currentmagnet;
        GameManager.Instance.moveSpeed.text = "MoveSpeed:" + currentMoveSpeed;

        GameManager.Instance.ChosenCharacterUI(characterData);
        UpdateHealthBar();
        UpdateExpBar();
        UpdateLevelTextBar();
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
        UpdateExpBar();
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
            UpdateLevelTextBar();

            GameManager.Instance.StartLevelUp();
        }
    }

    void UpdateExpBar()
    {
        ExpBar.fillAmount = (float)exp / expeCapping;
    }
    void UpdateLevelTextBar()
    {
        LevelText.text = "LV" + level.ToString();
    }

    public void TakeDamage(float dmg)
    {
        if (!isInvincible)
        {
            CurrentHealth -= dmg;
            invincibilityTimer = invincivibilityDuration;
            isInvincible = true;
            if (CurrentHealth <= 0)
            {
                Kill();
            }
            UpdateHealthBar();
        }
        
    }

    public void Kill()
    {
        if (!GameManager.Instance.isGameOver)
        {
            GameManager.Instance.AssignCharUI(level);
            GameManager.Instance.AssignchosenWeaponAndPassiveItemUI(inventory.weaponUISlots, inventory.passiveItemUISlots);
            GameManager.Instance.GameOver();
        }
    }

    public void RestoreHealth(float amt) // ================  RESTORING HEALTH ===========================================
    {
        if (CurrentHealth < characterData.MaxHealth)
        {
            CurrentHealth += amt;
            if (CurrentHealth > characterData.MaxHealth)
            {
                CurrentHealth = characterData.MaxHealth;
            }
        }
        
    }

    void Recover()
    {
        if (CurrentHealth < characterData.MaxHealth)
        {
            CurrentHealth += CurrentRecovery * Time.deltaTime;
            if (CurrentHealth > characterData.MaxHealth)
            {
                CurrentHealth = characterData.MaxHealth;
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

     void UpdateHealthBar()
    {
        healthBar.fillAmount = currentHealth / characterData.MaxHealth;
    }

 
}
