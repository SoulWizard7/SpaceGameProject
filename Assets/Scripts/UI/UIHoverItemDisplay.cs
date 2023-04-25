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

    private int[] itemValues = new []{0,0,0,0};
    private int[] itemComparedValues = new []{0,0,0,0};
    
    public void SetItemValue(Attributes attribute, int value) => itemValues[(int) attribute] = value;
    public void SetComparedItemValue(Attributes attribute, int value) => itemComparedValues[(int) attribute] = value;

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

    public int GetValue(Attributes attribute)
    {
        return itemValues[(int)attribute];
    }

    public void SetItemDisplayNull()
    {
        itemName.text = "";
    }
    
    private string GetValueAsString(int value)
    {
        if (value > 0)
        {
            return String.Concat("+", value.ToString(), "\n");
        }
        return String.Concat(value.ToString(), "\n");
    }
    
    private string GetComparedValueAsString(int value)
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
    
    
}