using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DynamicInterface : UserInterface
{
    public GameObject inventoryPrefab;
    public int X_START;
    public int Y_START;

    public int X_SPACE_BETWEEN_ITEM;
    public int Y_SPACE_BETWEEN_ITEM;
    public int NUMBER_OF_COLUMNS;
    
    
    public override void CreateSlots()
    {
        if(inventory == null) return;
        
        slotsOnInterface = new Dictionary<GameObject, InventorySlot>();
        for (int i = 0; i < inventory.GetSlots.Length; i++)
        {
            var obj = Instantiate(inventoryPrefab, Vector3.zero, Quaternion.identity, transform);
            obj.GetComponent<RectTransform>().localPosition = GetPosition(i);
            
            AddEvent(obj, EventTriggerType.PointerEnter, delegate{ OnEnter(obj); });
            AddEvent(obj, EventTriggerType.PointerExit, delegate{ OnExit(obj); });
            AddEvent(obj, EventTriggerType.BeginDrag, delegate{ OnDragStart(obj); });
            AddEvent(obj, EventTriggerType.EndDrag, delegate{ OnDragEnd(obj); });
            AddEvent(obj, EventTriggerType.Drag, delegate{ OnDrag(obj); });
            
            inventory.GetSlots[i].slotDisplay = obj;
            
            slotsOnInterface.Add(obj, inventory.GetSlots[i]);
        }
    }

    
    
    private Vector3 GetPosition(int i)
    {
        return new Vector3(X_START+(X_SPACE_BETWEEN_ITEM * (i % NUMBER_OF_COLUMNS)), Y_START + (-Y_SPACE_BETWEEN_ITEM * (i / NUMBER_OF_COLUMNS)), 0f);
    }
}
