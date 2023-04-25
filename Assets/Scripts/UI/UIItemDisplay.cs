using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIItemDisplay : MonoBehaviour
{
    public TMP_Text itemName;
    public Image itemSprite;
    public TMP_Text itemDescription;
    private StaticInterface _interface;
    public ItemObject currentDisplayItemObject;
    public Item currentDisplayItem;

    public void Start()
    {
        _interface = GetComponent<StaticInterface>();
        _interface.CreateUI();
        SetItemDisplayNull();
    }

    public void UpdateItemDisplay(ItemObject itemObject, Item item)
    {
        if (itemObject == null || item == null)
        {
            SetItemDisplayNull();
            return;
        }

        currentDisplayItemObject = itemObject;
        currentDisplayItem = item;

        itemSprite.color = new Color(1, 1, 1, 1);
        itemSprite.sprite = itemObject.uiDisplay;
        itemName.text = itemObject.data.Name;
        itemDescription.text = itemObject.description;

        if (itemObject.weaponScript == null)
        {
            _interface.inventory.Clear();
            _interface.SetSlotsActive(false);
            Debug.Log("no weaponScript");
        }
        else
        {
            _interface.inventory.Clear();
            _interface.SetSlotsActive(true);   
            
            for (int i = 0; i < _interface.inventory.GetSlots.Length; i++)
            {
                if (item.weaponMods[i].itemId >= 0)
                {
                    _interface.inventory.GetSlots[i].UpdateSlot(new Item(_interface.inventory.database.itemObjects[item.weaponMods[i].itemId]), 1);
                    _interface.inventory.GetSlots[i].item.weaponMods = new[]
                    {
                        new ItemMod(item.weaponMods[i].modType, item.weaponMods[i].durability, item.weaponMods[i].helpValue,
                            item.weaponMods[i].itemId)
                    };
                }
            }
        }
    }

    public void SetItemDisplayNull()
    {
        itemSprite.sprite = null;
        itemSprite.color = new Color(1, 1, 1, 0);
        itemName.text = "";
        itemDescription.text = "";
        
        currentDisplayItemObject = null;
        currentDisplayItem = null;
        
        _interface.SetSlotsActive(false);
        
        _interface.inventory.Clear();
    }

    private void OnApplicationQuit()
    {
        _interface.inventory.Clear();
    }
}
