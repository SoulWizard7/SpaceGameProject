using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using TMPro;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DisplayInventory : MonoBehaviour
{
    /*
    
    public GameObject inventoryPrefab;
    public InventoryObject inventory;

    public int X_START;
    public int Y_START;

    public int X_SPACE_BETWEEN_ITEM;
    public int Y_SPACE_BETWEEN_ITEM;
    public int NUMBER_OF_COLUMNS;

    private Dictionary<GameObject, InventorySlot> itemsDisplayed = new Dictionary<GameObject, InventorySlot>();

    private void Start()
    {
        CreateSlots();
    }
    
    private void Update()
    {
        UpdateDisplay();
    }

    private void UpdateDisplay()
    {
        foreach (KeyValuePair<GameObject, InventorySlot> _slot in itemsDisplayed)
        {
            if (_slot.Value.ID >= 0) //slot has item in it
            {
                _slot.Key.transform.GetChild(0).GetComponentInChildren<Image>().sprite = inventory.database.GetItem[_slot.Value.item.Id].uiDisplay;
                _slot.Key.transform.GetChild(0).GetComponentInChildren<Image>().color = new Color(1, 1, 1, 1);
                _slot.Key.GetComponentInChildren<TextMeshProUGUI>().text = _slot.Value.amount == 1 ? "" : _slot.Value.amount.ToString("n0");
            }
            else
            {
                _slot.Key.transform.GetChild(0).GetComponentInChildren<Image>().sprite = null;
                _slot.Key.transform.GetChild(0).GetComponentInChildren<Image>().color = new Color(1, 1, 1, 0);
                _slot.Key.GetComponentInChildren<TextMeshProUGUI>().text = "";
            }
        }
    }

    public void CreateSlots()
    {
        itemsDisplayed = new Dictionary<GameObject, InventorySlot>();
        for (int i = 0; i < inventory.Container.Items.Length; i++)
        {
            var obj = Instantiate(inventoryPrefab, Vector3.zero, quaternion.identity, transform);
            obj.GetComponent<RectTransform>().localPosition = GetPosition(i);
            
            AddEvent(obj, EventTriggerType.PointerEnter, delegate{ OnEnter(obj); });
            AddEvent(obj, EventTriggerType.PointerExit, delegate{ OnExit(obj); });
            AddEvent(obj, EventTriggerType.BeginDrag, delegate{ OnDragStart(obj); });
            AddEvent(obj, EventTriggerType.EndDrag, delegate{ OnDragEnd(obj); });
            AddEvent(obj, EventTriggerType.Drag, delegate{ OnDrag(obj); });
            
            itemsDisplayed.Add(obj, inventory.Container.Items[i]);
        }
    }
    
    public void OnEnter(GameObject obj)
    {
        mouseData.slotHoveredOver = obj;
        if (itemsDisplayed.ContainsKey(obj))
        {
            mouseData.hoverItem = itemsDisplayed[obj];
        }
    }
    private void OnExit(GameObject o)
    {
        mouseData.slotHoveredOver = null;
        mouseData.hoverItem = null;
    }
    private void OnDragStart(GameObject obj)
    {
        var mouseObject = new GameObject();
        var rt = mouseObject.AddComponent<RectTransform>();
        rt.sizeDelta = new Vector2(50, 50);
        mouseObject.transform.SetParent(transform.parent);
        if (itemsDisplayed[obj].ID >= 0)
        {
            var img = mouseObject.AddComponent<Image>();
            img.sprite = inventory.database.GetItem[itemsDisplayed[obj].ID].uiDisplay;
            img.raycastTarget = false;
        }

        mouseData.tempItemBeingDragged = mouseObject;
        mouseData.item = itemsDisplayed[obj];
    }
    private void OnDragEnd(GameObject obj)
    {
        if (mouseData.slotHoveredOver)
        {
            inventory.SwapItems(itemsDisplayed[obj], itemsDisplayed[mouseData.slotHoveredOver]);
        }
        else
        {
            inventory.RemoveItem(itemsDisplayed[obj].item);
        }
        Destroy(mouseData.tempItemBeingDragged);
        mouseData.item = null;
    }
    private void OnDrag(GameObject obj)
    {
        if (mouseData.tempItemBeingDragged != null)
        {
            mouseData.tempItemBeingDragged.GetComponent<RectTransform>().position = Input.mousePosition;
        }
    }

    private void AddEvent(GameObject obj, EventTriggerType type, UnityAction<BaseEventData> action)
    {
        EventTrigger trigger = obj.GetComponent<EventTrigger>();
        var eventTrigger = new EventTrigger.Entry();
        eventTrigger.eventID = type;
        eventTrigger.callback.AddListener(action);
        trigger.triggers.Add(eventTrigger);
    }
    public Vector3 GetPosition(int i)
    {
        return new Vector3(X_START+(X_SPACE_BETWEEN_ITEM * (i % NUMBER_OF_COLUMNS)), Y_START + (-Y_SPACE_BETWEEN_ITEM * (i / NUMBER_OF_COLUMNS)), 0f);
    }*/
}

