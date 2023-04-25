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
                var a = MouseData.interfaceMouseIsOver.slotsOnInterface[MouseData.slotHoveredOver];
                var b = interactionController.weapons.GetSlots[interactionController.playerShooting.currentWeaponIndex];

                if (a != b)
                {
                    _compareItemDisplay.itemName.text = interactionController.GetCurrentWeapon().data.Name;
                    _compareItem.SetActive(true);
                }
            }
        }
    }

    private string GetComparedValueAsString(int value1, int value2)
    {
        int value = value1 - value2;
        string valueAsString = String.Empty;
        if (value > 0)
        {
            valueAsString = String.Concat("+", value.ToString());
            return StringExtensions.AddColor(valueAsString, Color.green);
        }
        return StringExtensions.AddColor(value.ToString(), Color.red);
    }

    private string GetValueAsString(int value)
    {
        if (value > 0)
        {
            return String.Concat("+", value.ToString(), "\n");
        }
        return String.Concat(value.ToString(), "\n");
    }

    private void UpdateCompareEquipmentDisplay()
    {
        if (_hoverItem.activeSelf && MouseData.interfaceMouseIsOver.inventory.type != InterfaceType.Equipment) // dont need to compare if hovered interface is equipment interface
        {
            if (currentHoveredItemObject.type == ItemType.Helmet && interactionController.equipment.GetSlots[0].ItemObject) // Has item in helmet slot
            {
                _compareItem.SetActive(true);
                
                var compareItem = interactionController.equipment.GetSlots[0];
                
                _compareItemDisplay.itemName.text = compareItem.ItemObject.data.Name;
                _compareItemDisplay.ResetItemValues();

                for (int i = 0; i < compareItem.item.buffs.Length; i++)
                {
                    Attributes curAttribute = compareItem.item.buffs[i].attribute;
                    _compareItemDisplay.SetItemValue(curAttribute, compareItem.item.buffs[i].value);
                }

                for (int i = 0; i < 3; i++) // Maybe not good to hardcode 4
                {
                    _compareItemDisplay.SetComparedItemValue((Attributes)i, _compareItemDisplay.GetValue((Attributes)i) - _hoverItemDisplay.GetValue((Attributes)i));
                    _hoverItemDisplay.SetComparedItemValue((Attributes)i, _hoverItemDisplay.GetValue((Attributes)i) - _compareItemDisplay.GetValue((Attributes)i));
                }
                
                _compareItemDisplay.UpdateItemValues();
                _compareItemDisplay.UpdateComparedItemValues();
                _hoverItemDisplay.UpdateComparedItemValues();
            }
            else if (currentHoveredItemObject.type == ItemType.Chest && interactionController.equipment.GetSlots[1].ItemObject)
            {
                _compareItemDisplay.itemName.text = interactionController.equipment.GetSlots[1].ItemObject.data.Name;
                _compareItem.SetActive(true);
            }
            else if (currentHoveredItemObject.type == ItemType.Boots && interactionController.equipment.GetSlots[2].ItemObject)
            {
                _compareItemDisplay.itemName.text = interactionController.equipment.GetSlots[2].ItemObject.data.Name;
                _compareItem.SetActive(true);
            }
        }
    }

    public void EnableHoverItemDisplay()
    {
        currentHoveredItemObject = MouseData.interfaceMouseIsOver.slotsOnInterface[MouseData.slotHoveredOver].ItemObject;
        if(!currentHoveredItemObject) return;
        currentHoveredItem = MouseData.interfaceMouseIsOver.slotsOnInterface[MouseData.slotHoveredOver].item;

        _hoverItem.SetActive(true);
        _compareItem.SetActive(false);
        
        _hoverItemDisplay.ResetItemValues();
        _hoverItemDisplay.UpdateComparedItemValues();
        
        _hoverItemDisplay.itemName.text = currentHoveredItemObject.data.Name;

        if (currentHoveredItemObject.type == ItemType.Weapon)
        {
            UpdateCompareWeaponDisplay();
        }
        else if (currentHoveredItemObject.type == ItemType.Helmet || currentHoveredItemObject.type == ItemType.Chest || currentHoveredItemObject.type == ItemType.Boots)
        {
            for (int i = 0; i < currentHoveredItem.buffs.Length; i++)
            {
                _hoverItemDisplay.SetItemValue(currentHoveredItem.buffs[i].attribute, currentHoveredItem.buffs[i].value);
            }
            
            _hoverItemDisplay.UpdateItemValues();
            
            UpdateCompareEquipmentDisplay();
        }
    }

    public void DisableHoverItemDisplay()
    {
        _hoverItem.SetActive(false);
        _compareItem.SetActive(false);
    }
}
