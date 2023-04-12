using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;

public enum InterfaceType
{
    Inventory,
    Equipment,
    Chest,
    Weapon
}

[CreateAssetMenu(fileName = "New Inventory", menuName = "Inventory system/Inventory")]
public class InventoryObject : ScriptableObject
{
    public string savePath;
    public ItemDatabaseObject database;
    public InterfaceType type;
    public Inventory Container;
    public InventorySlot[] GetSlots { get { return Container.Slots; } }

    public bool AddItem(Item _item, int _amount)
    {
        //Issue with order of checking, if inventory is full stackable objects will not be added.
        
        if (EmptySlotCount <= 0) return false;

        InventorySlot slot = FindItemOnInventory(_item);
        if (!database.itemObjects[_item.Id].stackable || slot == null)
        {
            SetFirstEmptySlot(_item, _amount);
            return true;
        }
        
        slot.AddAmount(_amount);
        return true;
    }

    public int EmptySlotCount
    {
        get
        {
            int counter = 0;
            for (int i = 0; i < GetSlots.Length; i++)
            {
                if (GetSlots[i].item.Id <= -1)
                {
                    counter++;
                }
            }
            return counter;
        }
    }

    public InventorySlot FindItemOnInventory(Item _item)
    {
        for (int i = 0; i < GetSlots.Length; i++)
        {
            if (GetSlots[i].item.Id == _item.Id)
            {
                return GetSlots[i];
            }
        }
        return null;
    }

    public InventorySlot SetFirstEmptySlot(Item _item, int _amount)
    {
        for (int i = 0; i < GetSlots.Length; i++)
        {
            if (GetSlots[i].item.Id <= -1)
            {
                GetSlots[i].UpdateSlot(_item, _amount);
                return GetSlots[i];
            }
        }
        // inventory full, needs fixing
        return null;
    }

    public bool SwapItems(InventorySlot item1, InventorySlot item2) //Change back to void instead of bool??
    {
        if (item2.CanPlaceInSlot(item1.ItemObject) && item1.CanPlaceInSlot(item2.ItemObject))
        {
            InventorySlot temp = new InventorySlot(item2.item, item2.amount);
            item2.UpdateSlot(item1.item, item1.amount);
            item1.UpdateSlot(temp.item, temp.amount);

            // Is now an event in UpdateSlot - OnAfterUpdate
            // if (item2.parent.inventory.type == InterfaceType.Weapon) 
            // {
            //     item2.parent.GetController().playerShooting.CheckWeapon();
            // }
            //
            // if (item1.parent.inventory.type == InterfaceType.Weapon)
            // {
            //     item1.parent.GetController().playerShooting.CheckWeapon();
            // }
            return true;
        }
        return false;
    }

    public void RemoveItem(Item _item)
    {
        for (int i = 0; i < GetSlots.Length; i++)
        {
            if (GetSlots[i].item == _item)
            {
                GetSlots[i].UpdateSlot(null, 0);
            }
        }
    }
    
    [ContextMenu("Save")]
    public void Save()
    {
        // string saveData = JsonUtility.ToJson(this, true);
        // BinaryFormatter bf = new BinaryFormatter();
        // FileStream file = File.Create(String.Concat(Application.persistentDataPath, savePath));
        // bf.Serialize(file, saveData);
        // file.Close();

        IFormatter formatter = new BinaryFormatter();
        Stream stream = new FileStream(string.Concat(Application.persistentDataPath, savePath), FileMode.Create, FileAccess.Write);
        formatter.Serialize(stream, Container);
        stream.Close();
    }
    
    [ContextMenu("Load")]
    public void Load()
    {
        if (File.Exists(string.Concat(Application.persistentDataPath, savePath)))
        {
            // BinaryFormatter bf = new BinaryFormatter();
            // FileStream file = File.Open(string.Concat(Application.persistentDataPath, savePath), FileMode.Open);
            // JsonUtility.FromJsonOverwrite(bf.Deserialize(file).ToString(), this);
            // file.Close();
            
            IFormatter formatter = new BinaryFormatter();
            Stream stream = new FileStream(string.Concat(Application.persistentDataPath, savePath), FileMode.Open, FileAccess.Read);
            Inventory newContainer = (Inventory)formatter.Deserialize(stream);
            for (int i = 0; i < GetSlots.Length; i++)
            {
                GetSlots[i].UpdateSlot(newContainer.Slots[i].item, newContainer.Slots[i].amount);
            }
            stream.Close();
        }
    }

    [ContextMenu("Clear")]
    public void Clear()
    {
        Container.Clear();
    }
}

[System.Serializable]
public class Inventory
{
    public InventorySlot[] Slots = new InventorySlot[16];

    public void Clear()
    {
        for (int i = 0; i < Slots.Length; i++)
        {
            Slots[i].RemoveItem();
        }
    }
}

public delegate void SlotUpdated(InventorySlot _slot);

[System.Serializable]
public class InventorySlot //Data class
{
    public ItemType[] AllowedItems = new ItemType[0];
    [System.NonSerialized] public UserInterface parent;
    [System.NonSerialized] public GameObject slotDisplay;
    [System.NonSerialized] public SlotUpdated OnBeforeUpdate;
    [System.NonSerialized] public SlotUpdated OnAfterUpdate;
    
    public Item item = new Item();
    public int amount;

    public ItemObject ItemObject
    {
        get
        {
            if (item.Id >= 0)
            {
                return parent.inventory.database.itemObjects[item.Id];
            }
            return null;
        }
    }
    
    public InventorySlot()
    {
        UpdateSlot(new Item(), 0);
    }

    public InventorySlot(Item _item, int _amount)
    {
        UpdateSlot(_item, _amount);
    }
    
    public void UpdateSlot(Item _item, int _amount)
    {
        if (OnBeforeUpdate != null)
        {
            OnBeforeUpdate.Invoke(this);
        }
        item = _item;
        amount = _amount;
        
        if (OnAfterUpdate != null)
        {
            OnAfterUpdate.Invoke(this);
            // Is now an event in UpdateSlot - OnAfterUpdate
            // if (parent.inventory.type == InterfaceType.Weapon)
            // {
            //     parent.GetController().playerShooting.CheckWeapon();
            // }
        }
    }

    public void RemoveItem()
    {
        UpdateSlot(new Item(), 0);
    }

    public void AddAmount(int value)
    {
        UpdateSlot(item, amount += value);
    }

    public bool CanPlaceInSlot(ItemObject _itemObject)
    {
        if (AllowedItems.Length <= 0 || _itemObject == null || _itemObject.data.Id < 0) return true;

        for (int i = 0; i < AllowedItems.Length; i++)
        {
            if (_itemObject.type == AllowedItems[i]) return true;
        }

        return false;
    }
}
