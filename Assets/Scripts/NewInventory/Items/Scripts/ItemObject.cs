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
    WeaponMod,
    Default
}

public enum Attributes
{
    MovementSpeed,
    AimBuff,
    Stamina,
    Armor
}

public enum ModType
{
    Sight,
    Stock,
    Magazine,
    Handle
}

[CreateAssetMenu(fileName = "New item object,", menuName = "Inventory system/Items/item_USE_THIS")]
public class ItemObject : ScriptableObject
{
    public Sprite uiDisplay;
    public bool stackable;
    public ItemType type;
    [TextArea(8, 20)] 
    public string description;
    [TextArea(2, 20)]
    public string hoverDescription;
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
    public ItemMod[] weaponMods;

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
        }

        if (item.weaponScript == null)
        {
            weaponMods = Array.Empty<ItemMod>();
            return;
        }

        if (item.weaponScript == null) return;

        weaponMods = new ItemMod[item.weaponScript.defaultMods.Length];
        for (int i = 0; i < weaponMods.Length; i++)
        {
            weaponMods[i] = item.weaponScript.defaultMods[i].data.weaponMods[0];
            weaponMods[i].itemId = item.weaponScript.defaultMods[i].data.Id;
            /*
            weaponMods[i] = new ItemMod(item.data.weaponMods[i].durMin, item.data.weaponMods[i].durMax,
                item.data.weaponMods[i].helpMin, item.data.weaponMods[i].helpMax)
            {
                modType = item.data.weaponMods[i].modType
            };*/
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

[System.Serializable]
public class ItemMod : IModifier
{
    public ModType modType;
    public float durability;
    public float durMin;
    public float durMax;
    public int helpValue;
    public int helpMin;
    public int helpMax;
    public int itemId;
    
    public ItemMod(float _durMin, float _durMax, int _helpMin, int _helpMax)
    {
        durMin = _durMin;
        durMax = _durMax;
        GenerateDurabilityValue();
        
        helpMin = _helpMin;
        helpMax = _helpMax;
        GenerateHelpValue();
    }

    public ItemMod(ModType _modType, float _durability, int _helpValue, int _itemId)
    {
        modType = _modType;
        durability = _durability;
        helpValue = _helpValue;
        itemId = _itemId;
    }

    public ItemMod(bool noItem)
    {
        itemId = -1;
    }
    
    public void GenerateDurabilityValue()
    {
        durability = UnityEngine.Random.Range(durMin, durMax);
    }
    
    public void GenerateHelpValue()
    {
        helpValue = UnityEngine.Random.Range(helpMin, helpMax);
    }

    public void AddValue(ref int baseValue)
    {
        baseValue += helpValue;
    }
}
