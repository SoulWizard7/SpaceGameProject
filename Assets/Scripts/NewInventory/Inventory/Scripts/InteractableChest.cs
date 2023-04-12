using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableChest : InteractableObject
{
    public InventoryObject chest;
    private bool isInteracting = false;
    
    public override void Interact(InteractionController interactionController)
    {
        interactionController.UIController.OpenChestScreen(chest);
        
        if (isInteracting)
        {
            isInteracting = false;
        }
        else
        {
            isInteracting = true;
            
        }
    }
    
}
