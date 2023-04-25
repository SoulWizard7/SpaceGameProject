using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIHoverItemDisplay : MonoBehaviour
{
    public TMP_Text itemName;
    public UIController uiController;

    public void SetItemDisplayNull()
    {
        itemName.text = "";
    }

}