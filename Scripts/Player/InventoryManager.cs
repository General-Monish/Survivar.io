using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public List<WeaponController> weaponSlots = new List<WeaponController>(6);
    public int[] weaponsLevels = new int[6];
    public List<PassiveItems> PassiveItemsSlots = new List<PassiveItems>(6);
    public int[] passiveItemLevels = new int[6];
    public List<Image> weaponUISlots = new List<Image>(6); 
    public List<Image> passiveItemUISlots = new List<Image>(6);

    PlayerStats player;
    private void Start()
    {
        player = GetComponent<PlayerStats>();
    }

    [Serializable]
    public class WeaponUpgrade
    {
        public GameObject initialWeapon;
        public WeaponScriptableObjects weapondata;
    }

    [Serializable]
    public class passiveItemUpgrade
    {
        public GameObject InitialPassiveItem;
        public PassiveItemsScriptableObjects passiveItemdata;
    }

    [Serializable]
    public class UI_Upgrade
    {
        public TextMeshProUGUI UpgradeNameDisplay;
        public TextMeshProUGUI UpgradeDiscriptionDisplay;
        public Image UpgradeIcon;
        public Button UpgradeButton;
    }

    public List<WeaponUpgrade> weaponUpgradesOptions = new List<WeaponUpgrade>(); // list of upgrade options for weapons
    public List<passiveItemUpgrade> passiveItemUpgradesOptions = new List<passiveItemUpgrade>(); // list of upgrade options for passive items
    public List<UI_Upgrade> uI_UpgradesOptions = new List<UI_Upgrade>(); // list of upgrade options for UI 

    public void AddWeapon(int slotIndex , WeaponController weapon)  // add weapon to a specefic slot
    {
        weaponSlots[slotIndex]=weapon;
        weaponsLevels[slotIndex] = weapon.weaponData.Level;
        weaponUISlots[slotIndex].enabled = true;
        weaponUISlots[slotIndex].sprite = weapon.weaponData.Icon;

        if (GameManager.Instance != null && GameManager.Instance.chosingUpgrade)
        {
            GameManager.Instance.EndLevelUp();
        }
        
    }

    public void AddPassiveItem(int slotIndex,PassiveItems passiveItems) // add passiveItem to a specefic slot
    {
        PassiveItemsSlots[slotIndex] = passiveItems;
        passiveItemLevels[slotIndex] = passiveItems.passiveItemsData.Level;
        passiveItemUISlots[slotIndex].enabled = true;
        passiveItemUISlots[slotIndex].sprite = passiveItems.passiveItemsData.Icon;

        if (GameManager.Instance != null && GameManager.Instance.chosingUpgrade)
        {
            GameManager.Instance.EndLevelUp();
        }

    }

    public void LevelUpWeapon(int slotIndex)
    {
        if (weaponSlots.Count > slotIndex)
        {
            WeaponController weapon = weaponSlots[slotIndex];
            if (!weapon.weaponData.NextLevelPrefab)
            {
                Debug.LogError("No Next Level For" + weapon.name);
                return;
            }
            GameObject UPGRADEDwEAPON = Instantiate(weapon.weaponData.NextLevelPrefab, transform.position, Quaternion.identity);
            UPGRADEDwEAPON.transform.SetParent(transform);
            AddWeapon(slotIndex, UPGRADEDwEAPON.GetComponent<WeaponController>());
            Destroy(weapon.gameObject);
            weaponsLevels[slotIndex] = UPGRADEDwEAPON.GetComponent<WeaponController>().weaponData.Level;

        }

        if (GameManager.Instance != null && GameManager.Instance.chosingUpgrade)
        {
            GameManager.Instance.EndLevelUp();
        }
    }

    public void levelUpPassiveItems(int slotIndex)
    {
        if (PassiveItemsSlots.Count > slotIndex)
        {
            PassiveItems passiveItems = PassiveItemsSlots[slotIndex];
            if (!passiveItems.passiveItemsData.NextLevelPrefab)
            {
                Debug.LogError("No Next Level For" + passiveItems.name);
                return;
            }
            GameObject upgradedPassiveItem = Instantiate(passiveItems.passiveItemsData.NextLevelPrefab, transform.position, Quaternion.identity);
            upgradedPassiveItem.transform.SetParent(transform);
            AddPassiveItem(slotIndex, upgradedPassiveItem.GetComponent<PassiveItems>());
            Destroy(passiveItems.gameObject);
            passiveItemLevels[slotIndex] = upgradedPassiveItem.GetComponent<PassiveItems>().passiveItemsData.Level;
        }

        if (GameManager.Instance != null && GameManager.Instance.chosingUpgrade)
        {
            GameManager.Instance.EndLevelUp();
        }
    }

    void ApplyUpgradeOptions()
    {
        foreach(var upgradeOption in uI_UpgradesOptions)
        {
            int upgradeType = UnityEngine.Random.Range(1, 3); // chose btween wepon and passive item
            if (upgradeType == 1)
            {
                WeaponUpgrade ChoseweaponUpgrade = weaponUpgradesOptions[UnityEngine.Random.Range(0, weaponUpgradesOptions.Count)];
                if (ChoseweaponUpgrade != null)
                {
                    bool newWeapon = false;
                    for(int i = 0; i < weaponSlots.Count; i++)
                    {
                        if (weaponSlots[i] != null && weaponSlots[i].weaponData == ChoseweaponUpgrade.weapondata)
                        {
                            newWeapon = false;
                            if (!newWeapon)
                            {
                                upgradeOption.UpgradeButton.onClick.AddListener(() => LevelUpWeapon(i)); // Apply button fnality
                                upgradeOption.UpgradeDiscriptionDisplay.text = ChoseweaponUpgrade.weapondata.NextLevelPrefab.GetComponent<WeaponController>().weaponData.Description;
                                upgradeOption.UpgradeNameDisplay.text = ChoseweaponUpgrade.weapondata.NextLevelPrefab.GetComponent<WeaponController>().weaponData.Name;
                            }
                            break;
                        }
                        else
                        {
                            newWeapon = true;
                        }
                    }
                    if (newWeapon) // spawnin a new weapon
                    {
                        upgradeOption.UpgradeButton.onClick.AddListener(() =>player.SpawnWeapon(ChoseweaponUpgrade.initialWeapon));
                        upgradeOption.UpgradeDiscriptionDisplay.text = ChoseweaponUpgrade.weapondata.Description;
                        upgradeOption.UpgradeNameDisplay.text = ChoseweaponUpgrade.weapondata.Name;
                    }
                    upgradeOption.UpgradeIcon.sprite = ChoseweaponUpgrade.weapondata.Icon;
                }
            }
            else if (upgradeType == 2)
            {
                passiveItemUpgrade chosepassiveitemUpgrade = passiveItemUpgradesOptions[UnityEngine.Random.Range(0, passiveItemUpgradesOptions.Count)];
                if (chosepassiveitemUpgrade != null)
                {
                    bool newPassiveItem = false;
                    for (int i = 0; i < PassiveItemsSlots.Count; i++)
                    {
                        if (PassiveItemsSlots[i] != null && PassiveItemsSlots[i].passiveItemsData == chosepassiveitemUpgrade.passiveItemdata)
                        {
                            newPassiveItem = false;
                            if (!newPassiveItem)
                            {
                                upgradeOption.UpgradeButton.onClick.AddListener(() => levelUpPassiveItems(i));
                                upgradeOption.UpgradeDiscriptionDisplay.text = chosepassiveitemUpgrade.passiveItemdata.NextLevelPrefab.GetComponent<PassiveItems>().passiveItemsData.Description;
                                upgradeOption.UpgradeNameDisplay.text = chosepassiveitemUpgrade.passiveItemdata.NextLevelPrefab.GetComponent<PassiveItems>().passiveItemsData.Name;
                            }
                            break;
                        }
                        else
                        {
                            newPassiveItem = true;
                        }
                    }
                    if (newPassiveItem)
                    {
                        upgradeOption.UpgradeButton.onClick.AddListener(() => player.SpawnPassiveItem(chosepassiveitemUpgrade.InitialPassiveItem));
                        upgradeOption.UpgradeDiscriptionDisplay.text = chosepassiveitemUpgrade.passiveItemdata.Description;
                        upgradeOption.UpgradeNameDisplay.text = chosepassiveitemUpgrade.passiveItemdata.Name;
                    }
                    upgradeOption.UpgradeIcon.sprite = chosepassiveitemUpgrade.passiveItemdata.Icon;
                }
            }
        }
    }

    void RemoveUpgradeOptions()
    {
        foreach(var upgradeOption in uI_UpgradesOptions)
        {
            upgradeOption.UpgradeButton.onClick.RemoveAllListeners();
        }
    }

    void RemoveAndApplyUpgrades()
    {
        RemoveUpgradeOptions();
        ApplyUpgradeOptions();
    }
}
