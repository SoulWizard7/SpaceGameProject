using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIHoverItemDisplay : MonoBehaviour
{
    public TextMeshProUGUI itemName;
    public UIController uiController;
    public TextMeshProUGUI itemBuffNameText;
    public TextMeshProUGUI itemValueText;
    public TextMeshProUGUI itemComparedValueText;

    private float[] itemValues = new []{0f,0f,0f,0f};
    private float[] itemComparedValues = new []{0f,0f,0f,0f};
    
    public static string EquipmentValues = "MovementSpeed\nAimBuff\nStamina\nArmor";
    public static string WeaponValues = "FireType\nDamage\nFireRate";
    
    public void SetEquipmentItemValue(Attributes attribute, float value) => itemValues[(int) attribute] = value;
    public void SetComparedItemValue(Attributes attribute, float value) => itemComparedValues[(int) attribute] = value;

    public void ResetItemValues()
    {
        for (int i = 0; i < itemValues.Length; i++)
        {
            itemValues[i] = 0;
        }
        for (int i = 0; i < itemComparedValues.Length; i++)
        {
            itemComparedValues[i] = 0;
        }
    }
    
    public void SetEquipmentItemValues(Item item) // Can remove string perhaps?
    {
        itemBuffNameText.text = EquipmentValues;
        for (int i = 0; i < item.buffs.Length; i++)
        {
            SetEquipmentItemValue(item.buffs[i].attribute, item.buffs[i].value);
        }
    }

    public void SetAndUpdateWeaponItemValues(Item item, ItemObject itemObject)
    {
        itemBuffNameText.text = WeaponValues;
        //"FireType\nDamage\nFireRate";
        string a = String.Concat(itemObject.weaponScript.GetFireType(), "\n");
        a = String.Concat(a, itemObject.weaponScript.GetDamage(), "\n");
        itemValues[1] = itemObject.weaponScript.GetDamage();
        a = String.Concat(a, itemObject.weaponScript.GetFireRate(), "/s", "\n");
        itemValues[2] = itemObject.weaponScript.GetFireRate();
        itemValueText.text = a;
    }

    public void UpdateItemValues()
    {
        string a = String.Empty;
        for (int i = 0; i < itemValues.Length; i++)
        {
            a = String.Concat(a, GetValueAsString(itemValues[i]));
        }

        itemValueText.text = a;
    }
    
    public void UpdateComparedItemValues()
    {
        string a = String.Empty;
        
        for (int i = 0; i < itemComparedValues.Length; i++)
        {
            a = String.Concat(a, GetComparedValueAsString(itemComparedValues[i]), "\n");
        }

        itemComparedValueText.text = a;
    }

    public float GetValue(Attributes attribute)
    {
        return itemValues[(int)attribute];
    }

    public void SetItemDisplayNull()
    {
        itemName.text = "";
    }
    
    private string GetValueAsString(float value)
    {
        if (value > 0)
        {
            return String.Concat("+", value.ToString(), "\n");
        }
        return String.Concat(value.ToString(), "\n");
    }
    
    private string GetComparedValueAsString(float value)
    {
        if (value > 0)
        {
            string valueAsString = String.Concat("+", value.ToString());
            return StringExtensions.AddColor(valueAsString, Color.green);
        }
        if (value < 0)
        {
            return StringExtensions.AddColor(value.ToString(), Color.red);
        }

        return "";
    }

    public void RemoveAllText()
    {
        itemName.text = "";
        itemBuffNameText.text = "";
        itemValueText.text = "";
        itemComparedValueText.text = "";
        ResetItemValues();
    }
}