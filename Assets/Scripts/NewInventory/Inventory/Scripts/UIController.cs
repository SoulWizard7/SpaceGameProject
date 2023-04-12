using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    public GameObject inventoryScreen;
    public GameObject chestScreen;
    public GameObject weaponsScreen;
    public DynamicInterface chestInterface;
    public DynamicInterface inventoryInterface;
    public StaticInterface equipmentInterface;
    public StaticInterface weaponsInterface;
    
    public GameObject Player;
    public InteractionController interactionController;
    public PlayerShooting playerShooting;

    public void CreateRegularInterfaces()
    {
        inventoryInterface.CreateUI();
        equipmentInterface.CreateUI();
        chestInterface.CreateUI();
        weaponsInterface.CreateUI();
    }

    public void ToggleInventory()
    {
        if (inventoryScreen.activeSelf)
        {
            inventoryScreen.SetActive(false);
            CloseChestScreen();
            CloseWeaponsScreen();
        }
        else
        {
            inventoryScreen.SetActive(true);
            OpenWeaponsScreen();
        }
    }

    public void OpenChestScreen(InventoryObject inventory)
    {
        chestInterface.RemoveAllSlots();
        chestInterface.inventory = inventory;
        chestInterface.CreateUI();
        
        if (!chestScreen.activeSelf)
        {
            chestScreen.SetActive(true);
        }
        
        if (!inventoryScreen.activeSelf)
        {
            inventoryScreen.SetActive(true);
        }
    }

    public void CloseChestScreen()
    {
        chestScreen.SetActive(false);
    }

    public void OpenWeaponsScreen()
    {
        weaponsScreen.SetActive(true);
        weaponsInterface.MoveHighlight(playerShooting.currentWeaponIndex);
    }

    public void CloseWeaponsScreen()
    {
        weaponsScreen.SetActive(false);
    }
}
