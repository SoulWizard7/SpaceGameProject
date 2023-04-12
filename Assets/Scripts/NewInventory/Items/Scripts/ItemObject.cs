using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    Food,
    Equipment,
    Helmet,
    Chest,
    Boots,
    Weapon,
    Default
}

public enum Attributes
{
    MovementSpeed,
    AimBuff,
    Stamina,
    Armor
}

[CreateAssetMenu(fileName = "New item object,", menuName = "Inventory system/Items/item_USE_THIS")]
public class ItemObject : ScriptableObject
{
    public Sprite uiDisplay;
    public bool stackable;
    public ItemType type;
    [TextArea(15, 20)] 
    public string description;
    public Item data = new Item();
    public GameObject model;
    public WeaponBase weaponScript;

    public Item CreateItem()
    {
        Item newItem = new Item(this);
        return newItem;
    }
}

[System.Serializable]
public class Item
{
    public string Name;
    public int Id = -1;
    public ItemBuff[] buffs;

    public Item()
    {
        Name = "";
        Id = -1;
    }

    public Item(ItemObject item)
    {
        Name = item.name;
        Id = item.data.Id;
        buffs = new ItemBuff[item.data.buffs.Length];
        for (int i = 0; i < buffs.Length; i++)
        {
            buffs[i] = new ItemBuff(item.data.buffs[i].min, item.data.buffs[i].max)
            {
                attribute = item.data.buffs[i].attribute
            };

            // buffs[i] = new ItemBuff(item.data.buffs[i].min, item.data.buffs[i].max);
            // buffs[i].attribute = item.data.buffs[i].attribute;
        }
    }
}

[System.Serializable]
public class ItemBuff : IModifier
{
    public Attributes attribute;
    public int value;
    public int min;
    public int max;

    public ItemBuff(int _min, int _max)
    {
        min = _min;
        max = _max;
        GenerateValue();
    }

    public void GenerateValue()
    {
        value = UnityEngine.Random.Range(min, max);
    }

    public void AddValue(ref int baseValue)
    {
        baseValue += value;
    }
}
