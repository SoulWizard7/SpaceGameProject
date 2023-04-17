using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using TMPro;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public abstract class UserInterface : MonoBehaviour
{
    public InventoryObject inventory;
    public Dictionary<GameObject, InventorySlot> slotsOnInterface = new Dictionary<GameObject, InventorySlot>();
    private UIController _uiController;
    private InteractionController _interactionController;

    private void Start()
    {
        AddEvent(gameObject, EventTriggerType.PointerEnter, delegate{ OnEnterInterface(gameObject); }); 
        AddEvent(gameObject, EventTriggerType.PointerExit, delegate{ OnExitInterface(gameObject); });

        _uiController = GetComponentInParent<UIController>();
        _interactionController = _uiController.interactionController;
        
        //print(string.Concat("hej from ", inventory.name));
        //CreateUI();
    }

    protected UIController GetUIController()
    {
        return _uiController;
    }
    
    public InteractionController GetController()
    {
        return _interactionController;
    }

    public void CreateUI()
    {
        for (int i = 0; i < inventory.GetSlots.Length; i++)
        {
            if (inventory.GetSlots[i] == null)
            {
                print("CreateUI slots were null");
                inventory.GetSlots[i] = new InventorySlot();
            }
            inventory.GetSlots[i].parent = this;
            inventory.GetSlots[i].OnAfterUpdate += OnSlotUpdate; //does this add events to infinity?

            if (inventory.type == InterfaceType.Weapon)
            {
                inventory.GetSlots[i].OnAfterUpdate += CheckCurrentWeapon;
            }
        }
        CreateSlots();
        
        slotsOnInterface.UpdateSlotDisplay();
    }

    private void CheckCurrentWeapon(InventorySlot _slot)
    {
        if(!_interactionController) return; // this is ass, but gives error on quit
        GetController().playerShooting.CheckWeapon();
    }
    
    public void RemoveAllSlots()
    {
        for (int i = 0; i < inventory.GetSlots.Length; i++)
        {
            inventory.GetSlots[i].OnAfterUpdate -= OnSlotUpdate; //is this good?
            inventory.GetSlots[i].OnAfterUpdate -= CheckCurrentWeapon;
        }
        
        foreach (var slot in slotsOnInterface)
        {
            Destroy(slot.Key);
        }
    }

    private void OnSlotUpdate(InventorySlot _slot)
    {
        Image slotImage = _slot.slotDisplay.transform.GetChild(0).GetComponentInChildren<Image>();

        if (_slot.item.Id >= 0) //slot has item in it
        {
            slotImage.sprite = _slot.ItemObject.uiDisplay;
            slotImage.color = new Color(1, 1, 1, 1);
            _slot.slotDisplay.GetComponentInChildren<TextMeshProUGUI>().text =
                _slot.amount == 1 ? "" : _slot.amount.ToString("n0");
        }
        else
        {
            slotImage.sprite = null;
            slotImage.color = new Color(1, 1, 1, 0);
            _slot.slotDisplay.GetComponentInChildren<TextMeshProUGUI>().text = "";
        }
    }

    // private void Update()
    // {
    //     slotsOnInterface.UpdateSlotDisplay();
    // }

    public abstract void CreateSlots();

    private void OnEnterInterface(GameObject obj)
    {
        MouseData.interfaceMouseIsOver = obj.GetComponent<UserInterface>();
    }
    private void OnExitInterface(GameObject obj)
    {
        MouseData.interfaceMouseIsOver = null;
    }
    public void OnEnter(GameObject obj)
    {
        MouseData.slotHoveredOver = obj;
    }
    public void OnExit(GameObject o)
    {
        MouseData.slotHoveredOver = null;
    }
    public void OnDragStart(GameObject obj)
    {
        MouseData.tempItemBeingDragged = CreateTempItem(obj);
        GetUIController().uiItemDisplay.UpdateItemDisplay(slotsOnInterface[obj].ItemObject, slotsOnInterface[obj].item);
    }

    public void OnClick(GameObject obj)
    {
        GetUIController().uiItemDisplay.UpdateItemDisplay(slotsOnInterface[obj].ItemObject, slotsOnInterface[obj].item);
    }

    public GameObject CreateTempItem(GameObject obj)
    {
        GameObject tempItem = null;
        if (slotsOnInterface[obj].item.Id >= 0)
        {
            tempItem = new GameObject();
            var rt = tempItem.AddComponent<RectTransform>();
            rt.sizeDelta = new Vector2(50, 50);
            tempItem.transform.SetParent(transform.parent);
            var img = tempItem.AddComponent<Image>();
            img.sprite = slotsOnInterface[obj].ItemObject.uiDisplay;
            img.raycastTarget = false;
        }
        return tempItem;
    }
    public void OnDragEnd(GameObject obj)
    {
        Destroy(MouseData.tempItemBeingDragged);

        if (MouseData.interfaceMouseIsOver == null)
        {
            GetController().SpawnObject(slotsOnInterface[obj].ItemObject, slotsOnInterface[obj].item);
            slotsOnInterface[obj].RemoveItem();
            if (inventory.type == InterfaceType.Weapon)
            {
                //GetController().playerShooting.CheckWeapon();
            }
            return;
        }

        if (MouseData.slotHoveredOver)
        {
            InventorySlot mouseHoverSlotData = MouseData.interfaceMouseIsOver.slotsOnInterface[MouseData.slotHoveredOver];
            inventory.SwapItems(slotsOnInterface[obj], mouseHoverSlotData);
        }
    }

    public void OnDrag(GameObject obj)
    {
        if (MouseData.tempItemBeingDragged != null)
        {
            MouseData.tempItemBeingDragged.GetComponent<RectTransform>().position = Input.mousePosition;
        }
    }

    protected void AddEvent(GameObject obj, EventTriggerType type, UnityAction<BaseEventData> action)
    {
        EventTrigger trigger = obj.GetComponent<EventTrigger>();
        var eventTrigger = new EventTrigger.Entry();
        eventTrigger.eventID = type;
        eventTrigger.callback.AddListener(action);
        trigger.triggers.Add(eventTrigger);
    }
}

public static class MouseData
{
    public static UserInterface interfaceMouseIsOver;
    public static GameObject tempItemBeingDragged;
    public static GameObject slotHoveredOver;
}

public static class ExtentionMethods
{
    public static void UpdateSlotDisplay(this Dictionary<GameObject, InventorySlot> _slotsOnInterface)
    {
        foreach (KeyValuePair<GameObject, InventorySlot> _slot in _slotsOnInterface)
        {
            Image slotImage = _slot.Key.transform.GetChild(0).GetComponentInChildren<Image>();
            
            if (_slot.Value.item.Id >= 0) //slot has item in it
            {
                slotImage.sprite = _slot.Value.ItemObject.uiDisplay;
                slotImage.color = new Color(1, 1, 1, 1);
                _slot.Key.GetComponentInChildren<TextMeshProUGUI>().text = _slot.Value.amount == 1 ? "" : _slot.Value.amount.ToString("n0");
            }
            else
            {
                slotImage.sprite = null;
                slotImage.color = new Color(1, 1, 1, 0);
                _slot.Key.GetComponentInChildren<TextMeshProUGUI>().text = "";
            }
        }
    }
}
