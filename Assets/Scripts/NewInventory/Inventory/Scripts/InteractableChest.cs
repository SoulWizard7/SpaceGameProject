using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class InteractableChest : InteractableObject
{
    public InventoryObject chest;

    public override void Interact(InteractionController interactionController)
    {
        if (!IsThisChestOpen(interactionController))
        {
            interactionController.OpenChest(chest, transform.position);
        }
    }

    bool IsThisChestOpen(InteractionController interactionController)
    {
        if (interactionController.uiController.chestScreen.activeSelf)
        {
            if (interactionController.uiController.chestInterface.inventory == chest)
            {
                return true;
            }
        }
        return false;
    }
}
