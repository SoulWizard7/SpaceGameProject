using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class StaticInterface : UserInterface
{
    public GameObject[] slots;
    private int curHighlight = 0;

    private Color unHighlightedColor;
    private Color highlightedColor;

    public override void CreateSlots()
    {
        if (inventory.type == InterfaceType.Weapon)
        {
            unHighlightedColor = slots[curHighlight].GetComponent<Image>().color;
            highlightedColor = new Color(unHighlightedColor.r, unHighlightedColor.g, unHighlightedColor.b, 1); // Temp idea for what weapon is highlighted
        }

        slotsOnInterface = new Dictionary<GameObject, InventorySlot>();
        for (int i = 0; i < inventory.GetSlots.Length; i++)
        {
            var obj = slots[i];
            
            AddEvent(obj, EventTriggerType.PointerEnter, delegate{ OnEnter(obj); });
            AddEvent(obj, EventTriggerType.PointerExit, delegate{ OnExit(obj); });
            AddEvent(obj, EventTriggerType.BeginDrag, delegate{ OnDragStart(obj); });
            AddEvent(obj, EventTriggerType.EndDrag, delegate{ OnDragEnd(obj); });
            AddEvent(obj, EventTriggerType.Drag, delegate{ OnDrag(obj); });
            AddEvent(obj, EventTriggerType.PointerClick, delegate{ OnClick(obj); });
            inventory.GetSlots[i].slotDisplay = obj;
            
            slotsOnInterface.Add(obj, inventory.GetSlots[i]);
        }
    }

    public void MoveHighlight(int index)
    {
        slots[curHighlight].GetComponent<Image>().color = unHighlightedColor;
        curHighlight = index;
        slots[curHighlight].GetComponent<Image>().color = highlightedColor;
    }
}
