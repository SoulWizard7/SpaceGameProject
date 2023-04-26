using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class StringExtensions
{
    public static string AddColor(this string text, Color col) => $"<color={ColorHexFromUnityColor(col)}>{text}</color>";
    public static string ColorHexFromUnityColor(this Color unityColor) => $"#{ColorUtility.ToHtmlStringRGBA(unityColor)}";
}

public class UIHoverItemDisplayController : MonoBehaviour
{
    public UIHoverItemDisplay _hoverItemDisplay;
    public UIHoverItemDisplay _compareItemDisplay;
    public GameObject _hoverItem;
    public GameObject _compareItem;

    public ItemObject currentHoveredItemObject;
    public Item currentHoveredItem;
    public InteractionController interactionController;


    private void Start()
    {
        _hoverItemDisplay.SetItemDisplayNull();
        _compareItemDisplay.SetItemDisplayNull();
        DisableHoverItemDisplay();
    }

    public void UpdateCompareWeaponDisplay()
    {
        if (_hoverItem.activeSelf)
        {
            if (currentHoveredItemObject.weaponScript && interactionController.GetCurrentWeapon())
            {
                var hoveredWeapon = MouseData.interfaceMouseIsOver.slotsOnInterface[MouseData.slotHoveredOver];
                var equippedWeapon = interactionController.weapons.GetSlots[interactionController.playerShooting.currentWeaponIndex];

                if (hoveredWeapon != equippedWeapon)
                {
                    _compareItemDisplay.itemName.text = interactionController.GetCurrentWeapon().data.Name;
                    _compareItemDisplay.SetAndUpdateWeaponItemValues(equippedWeapon.data, equippedWeapon.ItemObject);
                    // CompareItems();
                    // _compareItemDisplay.UpdateComparedItemValues(); // fire rate is wrong color, fml
                    // _hoverItemDisplay.UpdateComparedItemValues();
                    _compareItem.SetActive(true);
                }
            }
            else
            {
                _compareItem.SetActive(false);
            }
        }
    }

    private void CompareItems()
    {
        for (int i = 0; i < 4; i++) // Maybe not good to hardcode 4
        {
            _compareItemDisplay.SetComparedItemValue((Attributes)i, _compareItemDisplay.GetValue((Attributes)i) - _hoverItemDisplay.GetValue((Attributes)i));
            _hoverItemDisplay.SetComparedItemValue((Attributes)i, _hoverItemDisplay.GetValue((Attributes)i) - _compareItemDisplay.GetValue((Attributes)i));
        }
    }

    private void UpdateCompareEquipmentDisplay(int slot)
    {
        if (_hoverItem.activeSelf && MouseData.interfaceMouseIsOver.inventory.type != InterfaceType.Equipment) // dont need to compare if hovered interface is equipment interface
        {
            if (interactionController.equipment.GetSlots[slot].ItemObject) // Has item in equipment slot
            {
                _compareItem.SetActive(true);
                _compareItemDisplay.itemBuffNameText.text = UIHoverItemDisplay.EquipmentValues;
                
                var compareItem = interactionController.equipment.GetSlots[slot];
                
                _compareItemDisplay.itemName.text = compareItem.ItemObject.data.Name;
                _compareItemDisplay.SetEquipmentItemValues(compareItem.data);
                
                CompareItems();
                _compareItemDisplay.UpdateItemValues();
                _compareItemDisplay.UpdateComparedItemValues();
                _hoverItemDisplay.UpdateComparedItemValues();
            }
        }
    }

    public void EnableHoverItemDisplay()
    {
        currentHoveredItemObject = MouseData.interfaceMouseIsOver.slotsOnInterface[MouseData.slotHoveredOver].ItemObject;
        if(!currentHoveredItemObject) return;
        currentHoveredItem = MouseData.interfaceMouseIsOver.slotsOnInterface[MouseData.slotHoveredOver].data;

        _hoverItem.SetActive(true);
        _compareItem.SetActive(false);

        _hoverItemDisplay.RemoveAllText();
        _compareItemDisplay.RemoveAllText();
        
        
        _hoverItemDisplay.itemName.text = currentHoveredItemObject.data.Name;

        if (currentHoveredItemObject.type == ItemType.Weapon)
        {
            _hoverItemDisplay.SetAndUpdateWeaponItemValues(currentHoveredItem, currentHoveredItemObject);
            UpdateCompareWeaponDisplay();
        }
        else if (currentHoveredItemObject.type == ItemType.Helmet)
        {
            _hoverItemDisplay.SetEquipmentItemValues(currentHoveredItem);
            _hoverItemDisplay.UpdateItemValues();
            UpdateCompareEquipmentDisplay(0);
        }
        else if (currentHoveredItemObject.type == ItemType.Chest)
        {
            _hoverItemDisplay.SetEquipmentItemValues(currentHoveredItem);
            _hoverItemDisplay.UpdateItemValues();
            UpdateCompareEquipmentDisplay(1);
        }
        else if (currentHoveredItemObject.type == ItemType.Boots)
        {
            _hoverItemDisplay.SetEquipmentItemValues(currentHoveredItem);
            _hoverItemDisplay.UpdateItemValues();
            UpdateCompareEquipmentDisplay(2);
        }
        else
        {
            _hoverItemDisplay.itemBuffNameText.text = currentHoveredItemObject.hoverDescription;
        }
    }
    
    public void DisableHoverItemDisplay()
    {
        _hoverItem.SetActive(false);
        _compareItem.SetActive(false);
    }
}
