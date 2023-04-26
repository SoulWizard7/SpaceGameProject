using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIItemDisplay : MonoBehaviour
{
    public Image itemSprite;
    public TMP_Text itemDescription;
    private StaticInterface _interface;
    public ItemObject currentDisplayItemObject;
    public Item currentDisplayItem;
    public UIHoverItemDisplay itemDisplay;
    private InventorySlot _slot;

    public void Start()
    {
        _interface = GetComponent<StaticInterface>();
        _interface.CreateUI();
        SetItemDisplayNull();
    }

    public void UpdateItemDisplay(InventorySlot slot)
    {
        if(_slot != null) _slot.parent.DisableHighlight(_slot);
        
        if (slot.ItemObject == null || slot.data == null)
        {
            SetItemDisplayNull();
            return;
        }
        
        slot.parent.EnableHighlight(slot);
        _slot = slot;

        currentDisplayItemObject = slot.ItemObject;
        currentDisplayItem = slot.data;

        itemSprite.color = new Color(1, 1, 1, 1);
        itemSprite.sprite = currentDisplayItemObject.uiDisplay;
        itemDescription.text = currentDisplayItemObject.description;
        
        itemDisplay.itemName.text = currentDisplayItemObject.data.Name;
        itemDisplay.ResetItemValues();

        if (currentDisplayItemObject.type == ItemType.Helmet || currentDisplayItemObject.type == ItemType.Chest || currentDisplayItemObject.type == ItemType.Boots)
        {
            _interface.SetSlotsActive(false);
            itemDisplay.SetEquipmentItemValues(currentDisplayItem);
            itemDisplay.UpdateItemValues();
        }
        // else if (currentDisplayItemObject.weaponScript == null)
        // {
        //     _interface.inventory.Clear();
        //     _interface.SetSlotsActive(false);
        //     Debug.Log("no weaponScript");
        // }
        else if (currentDisplayItemObject.type == ItemType.Weapon)
        {
            itemDisplay.SetAndUpdateWeaponItemValues(currentDisplayItem, currentDisplayItemObject);
            
            _interface.inventory.Clear();
            _interface.SetSlotsActive(true);   
            
            for (int i = 0; i < _interface.inventory.GetSlots.Length; i++)
            {
                if (currentDisplayItem.weaponMods[i].itemId >= 0)
                {
                    _interface.inventory.GetSlots[i].UpdateSlot(new Item(_interface.inventory.database.itemObjects[currentDisplayItem.weaponMods[i].itemId]), 1);
                    _interface.inventory.GetSlots[i].data.weaponMods = new[]
                    {
                        new ItemMod(currentDisplayItem.weaponMods[i].modType, currentDisplayItem.weaponMods[i].durability, currentDisplayItem.weaponMods[i].helpValue,
                            currentDisplayItem.weaponMods[i].itemId)
                    };
                }
            }
        }
        else
        {
            _interface.SetSlotsActive(false);
        }
    }

    public void SetItemDisplayNull()
    {
        itemSprite.sprite = null;
        itemSprite.color = new Color(1, 1, 1, 0);
        itemDescription.text = "";
        
        currentDisplayItemObject = null;
        currentDisplayItem = null;
        
        itemDisplay.RemoveAllText();
        
        _interface.SetSlotsActive(false);
        
        _interface.inventory.Clear();
    }

    private void OnApplicationQuit()
    {
        _interface.inventory.Clear();
    }
}
