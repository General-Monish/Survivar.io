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
        public int weaponIndex;
        public GameObject initialWeapon;
        public WeaponScriptableObjects weapondata;
    }

    [Serializable]
    public class passiveItemUpgrade
    {
        public int passiveItemUpgradeIndex;
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

    public void LevelUpWeapon(int slotIndex,int upgradeIndex)
    {
        if (weaponSlots.Count > slotIndex)
        {
            WeaponController weapon = weaponSlots[slotIndex];
            if (!weapon.weaponData.NextLevelPrefab)
            {
                Debug.LogError("No Next Level For" + weapon.name);
                return;
            }
            GameObject upgradeWeapon = Instantiate(weapon.weaponData.NextLevelPrefab, transform.position, Quaternion.identity);
            upgradeWeapon.transform.SetParent(transform);
            AddWeapon(slotIndex, upgradeWeapon.GetComponent<WeaponController>());
            Destroy(weapon.gameObject);
            weaponsLevels[slotIndex] = upgradeWeapon.GetComponent<WeaponController>().weaponData.Level;

            weaponUpgradesOptions[upgradeIndex].weapondata = upgradeWeapon.GetComponent<WeaponController>().weaponData;
        if (GameManager.Instance != null && GameManager.Instance.chosingUpgrade)
            {
                GameManager.Instance.EndLevelUp();
            }
        }
       
    }

    public void levelUpPassiveItems(int slotIndex,int upgradeIndex)
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

            passiveItemUpgradesOptions[upgradeIndex].passiveItemdata = upgradedPassiveItem.GetComponent<PassiveItems>().passiveItemsData;
            if (GameManager.Instance != null && GameManager.Instance.chosingUpgrade)
            {
                GameManager.Instance.EndLevelUp();
            }
        }

      
    }

    void ApplyUpgradeOptions()
    {
        List<WeaponUpgrade> availableWeaponUpgrade = new List<WeaponUpgrade>(weaponUpgradesOptions);
        List<passiveItemUpgrade> availablePassiveItemUpgrades = new List<passiveItemUpgrade>(passiveItemUpgradesOptions);
        foreach(var upgradeOption in uI_UpgradesOptions)
        {
            if (availableWeaponUpgrade.Count == 0 && availablePassiveItemUpgrades.Count == 0)
            {
                return;
            }

            int upgradeType;
            if (availableWeaponUpgrade.Count == 0)
            {
                upgradeType = 2;
            }else if (availablePassiveItemUpgrades.Count == 0)
            {
                upgradeType = 1;
            }
            else
            {
                upgradeType = UnityEngine.Random.Range(1, 3);
            }
            if (upgradeType == 1)
            {
                WeaponUpgrade ChoseweaponUpgrade = availableWeaponUpgrade[UnityEngine.Random.Range(0, availableWeaponUpgrade.Count)];
                availableWeaponUpgrade.Remove(ChoseweaponUpgrade);
                if (ChoseweaponUpgrade != null)
                {
                    EnableUpgradeUI(upgradeOption);
                    bool newWeapon = false;
                    for(int i = 0; i < weaponSlots.Count; i++)
                    {
                        if (weaponSlots[i] != null && weaponSlots[i].weaponData == ChoseweaponUpgrade.weapondata)
                        {
                            newWeapon = false;
                            if (!newWeapon)
                            {
                                if (!ChoseweaponUpgrade.weapondata.NextLevelPrefab)
                                {
                                    DisableUpgradeUI(upgradeOption);
                                    break;
                                }
                                upgradeOption.UpgradeButton.onClick.AddListener(() => LevelUpWeapon(i,ChoseweaponUpgrade.weaponIndex)); // Apply button fnality
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
                passiveItemUpgrade chosepassiveitemUpgrade = availablePassiveItemUpgrades[UnityEngine.Random.Range(0, availablePassiveItemUpgrades.Count)];
                availablePassiveItemUpgrades.Remove(chosepassiveitemUpgrade);
                if (chosepassiveitemUpgrade != null)
                {
                    EnableUpgradeUI(upgradeOption);
                    bool newPassiveItem = false;
                    for (int i = 0; i < PassiveItemsSlots.Count; i++)
                    {
                        if (PassiveItemsSlots[i] != null && PassiveItemsSlots[i].passiveItemsData == chosepassiveitemUpgrade.passiveItemdata)
                        {
                            newPassiveItem = false;
                            if (!newPassiveItem)
                            {
                                upgradeOption.UpgradeButton.onClick.AddListener(() => levelUpPassiveItems(i,chosepassiveitemUpgrade.passiveItemUpgradeIndex));
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
                    if (!newPassiveItem)
                    {
                        if (chosepassiveitemUpgrade.passiveItemdata.NextLevelPrefab)
                        {
                            DisableUpgradeUI(upgradeOption);
                            break;
                        }
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
            DisableUpgradeUI(upgradeOption);
        }
    }

    void RemoveAndApplyUpgrades()
    {
        RemoveUpgradeOptions();
        ApplyUpgradeOptions();
    }

    void DisableUpgradeUI(UI_Upgrade ui)
    {
        ui.UpgradeNameDisplay.transform.parent.gameObject.SetActive(false);
    }
    void EnableUpgradeUI(UI_Upgrade ui)
    {
        ui.UpgradeNameDisplay.transform.parent.gameObject.SetActive(true);
    }
}
