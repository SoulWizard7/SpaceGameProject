using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public Attribute[] attributes;
    private InteractionController _interactionController;

    public void AttritureModified(Attribute attribute)
    {
        Debug.Log(string.Concat(attribute.type, " was updated. Value is now ", attribute.value.ModifiedValue));
    }

    private void Start()
    {
        _interactionController = GetComponent<InteractionController>();
        for (int i = 0; i < attributes.Length; i++)
        {
            attributes[i].SetParent(this);
        }

        for (int i = 0; i < _interactionController.equipment.GetSlots.Length; i++)
        {
            _interactionController.equipment.GetSlots[i].OnBeforeUpdate += OnBeforeSlotUpdate;
            _interactionController.equipment.GetSlots[i].OnAfterUpdate += OnAfterSlotUpdate;
        }

        ApplyAttributeNames();
        // for (int i = 0; i < _interactionController.weapons.GetSlots.Length; i++)
        // {
        //     _interactionController.weapons.GetSlots[i].OnBeforeUpdate += OnBeforeSlotUpdate;
        //     _interactionController.weapons.GetSlots[i].OnAfterUpdate += OnAfterSlotUpdate;
        // }
    }

    private void ApplyAttributeNames()
    {
        _interactionController.UIController.SetStatScreenNames(attributes);
    }


    public void OnBeforeSlotUpdate(InventorySlot _slot)
    {
        if (_slot.ItemObject == null)
        {
            return;
        }

        switch (_slot.parent.inventory.type)
        {
            case InterfaceType.Inventory:
                break;
            case InterfaceType.Equipment:
                print(string.Concat("Removed ", _slot.ItemObject, " on ", _slot.parent.inventory.type, ", Allowed ITems: ", string.Join(", ", _slot.AllowedItems)));
                
                for (int i = 0; i < _slot.item.buffs.Length; i++)
                {
                    for (int j = 0; j < attributes.Length; j++)
                    {
                        if (attributes[j].type == _slot.item.buffs[i].attribute)
                        {
                            attributes[j].value.RemoveModifier(_slot.item.buffs[i]);
                        }
                    }
                }
                break;
            case InterfaceType.Chest:
                break;
            default:
                break;
        }
    }

    public void OnAfterSlotUpdate(InventorySlot _slot)
    {
        
        if (_slot.ItemObject == null)
        {
            _interactionController.UIController.UpdateStatsScreen(attributes);
            return;
        }
        
        switch (_slot.parent.inventory.type)
        {
            case InterfaceType.Inventory:
                break;
            case InterfaceType.Equipment:
                print(string.Concat("placed ", _slot.ItemObject, " on ", _slot.parent.inventory.type, ", Allowed ITems: ", string.Join(", ", _slot.AllowedItems)));

                for (int i = 0; i < _slot.item.buffs.Length; i++)
                {
                    for (int j = 0; j < attributes.Length; j++)
                    {
                        if (attributes[j].type == _slot.item.buffs[i].attribute)
                        {
                            attributes[j].value.AddModifier(_slot.item.buffs[i]);
                        }
                    }
                }
                
                _interactionController.UIController.UpdateStatsScreen(attributes);
                
                break;
            case InterfaceType.Chest:
                break;
            default:
                break;
        }
    }
}

[System.Serializable]
public class Attribute
{
    [System.NonSerialized] public PlayerStats parent;
    public Attributes type;
    public ModifiableInt value;

    public void SetParent(PlayerStats _parent)
    {
        parent = _parent;
        value = new ModifiableInt();
    }

    public void AttributeModified()
    {
        parent.AttritureModified(this);
    }
}
