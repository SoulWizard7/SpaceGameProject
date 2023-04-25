using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIHoverItemDisplayController : MonoBehaviour
{
    public UIHoverItemDisplay _hoverItemDisplay;
    public UIHoverItemDisplay _compareItemDisplay;
    public GameObject _hoverItem;
    public GameObject _compareItem;

    public ItemObject currentHoveredItemObject;
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
            else
            {
                _compareItem.SetActive(false);
            }
        }
    }

    public void EnableHoverItemDisplay()
    {
        currentHoveredItemObject = MouseData.interfaceMouseIsOver.slotsOnInterface[MouseData.slotHoveredOver].ItemObject;
        if(!currentHoveredItemObject) return;
        
        _hoverItem.SetActive(true);
        _hoverItemDisplay.itemName.text = currentHoveredItemObject.data.Name;

        UpdateCompareWeaponDisplay();
    }

    public void DisableHoverItemDisplay()
    {
        _hoverItem.SetActive(false);
        _compareItem.SetActive(false);
    }
}
