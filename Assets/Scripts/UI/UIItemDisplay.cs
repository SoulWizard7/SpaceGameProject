using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIItemDisplay : MonoBehaviour
{
    public TMP_Text itemName;
    public Image itemSprite;
    public TMP_Text itemDescription;

    public void Start()
    {
        SetItemDisplayNull();
    }

    public void UpdateItemDisplay(ItemObject itemObject, Item item)
    {
        if (itemObject == null || item == null)
        {
            SetItemDisplayNull();
            return;
        }
        itemSprite.color = new Color(1, 1, 1, 1);
        itemSprite.sprite = itemObject.uiDisplay;
        itemName.text = itemObject.data.Name;
        itemDescription.text = itemObject.description;
    }

    public void SetItemDisplayNull()
    {
        itemSprite.sprite = null;
        itemSprite.color = new Color(1, 1, 1, 0);
        itemName.text = null;
    }
}
