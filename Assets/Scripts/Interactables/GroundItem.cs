using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class GroundItem : InteractableObject, ISerializationCallbackReceiver
{
    [HideInInspector]public ItemObject itemObject;
    public Item item;
    public GameObject model;
    public AnimationCurve dropCurve;

    public override void Interact(InteractionController interactionController)
    {
        PickupObject(interactionController);
    }

    public void DroppedObject(ItemObject _itemObject, Item _item)
    {
        itemObject = _itemObject;
        item = _item;
        model = Instantiate(itemObject.model, transform.position, quaternion.identity, transform);
        StartCoroutine(DropAnim(transform.forward));
    }

    void PickupObject(InteractionController interactionController)
    {
        if (interactionController.inventory.AddItem(item, 1)) //not same as tutorial video but seems to work
        {
            Destroy(gameObject);  
        }
    }

    public IEnumerator DropAnim(Vector3 forward)
    {
        float t = 0f;
        Vector3 orginalPos = transform.position;
        while (t < 0.5f)
        {
            t += Time.deltaTime;
            float y = dropCurve.Evaluate(t);

            Vector3 newPos = forward * (t * 2);
            newPos.y = y;

            transform.position = orginalPos + newPos;
            yield return null;
        }
    }

    private void OnEnable()
    {
        GroundItemManager.GroundItems.Add(this);
    }

    private void OnDisable()
    {
        GroundItemManager.GroundItems.Remove(this);
    }

    public void RefreshItem()
    {
        DestroyImmediate(model);
        model = Instantiate(itemObject.model, transform.position, quaternion.identity, transform);
        item = new Item(itemObject);
    }

    private void OnApplicationQuit()
    {
        model.Serialize();
    }

    public void OnBeforeSerialize()
    {
        /*
#if UNITY_EDITOR
        GetComponentInChildren<SpriteRenderer>().sprite = itemObject.uiDisplay;
        EditorUtility.SetDirty(GetComponentInChildren<SpriteRenderer>());
        
#endif
*/
    }

    public void OnAfterDeserialize()
    {
        
    }
}
