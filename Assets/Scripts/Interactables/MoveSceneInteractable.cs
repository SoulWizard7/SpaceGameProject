using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveSceneInteractable : InteractableObject
{
    public EnumScene GoToScene;
    
    public override void Interact(InteractionController interactionController)
    {
        interactionController.SaveInventory();
        SceneLoader.Load(GoToScene);
    }
}
