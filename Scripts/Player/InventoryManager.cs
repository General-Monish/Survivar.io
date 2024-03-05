using System.Collections;
using System.Collections.Generic;
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

    public void AddWeapon(int slotIndex , WeaponController weapon)  // add weapon to a specefic slot
    {
        weaponSlots[slotIndex]=weapon;
        weaponsLevels[slotIndex] = weapon.weaponData.Level;
        weaponUISlots[slotIndex].enabled = true;
        weaponUISlots[slotIndex].sprite = weapon.weaponData.Icon;
        
    }

    public void AddPassiveItem(int slotIndex,PassiveItems passiveItems) // add passiveItem to a specefic slot
    {
        PassiveItemsSlots[slotIndex] = passiveItems;
        passiveItemLevels[slotIndex] = passiveItems.passiveItemsData.Level;
        passiveItemUISlots[slotIndex].enabled = true;
        passiveItemUISlots[slotIndex].sprite = passiveItems.passiveItemsData.Icon;
        
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
    }
}
