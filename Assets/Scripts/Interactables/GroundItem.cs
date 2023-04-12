using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;

public class GroundItem : InteractableObject, ISerializationCallbackReceiver
{
    public ItemObject itemObject;
    public Item item;
    public GameObject model;
    public bool hasBeenDropped = false;
    public AnimationCurve dropCurve;

    public override void Interact(InteractionController interactionController)
    {
        PickupObject(interactionController);
    }

    private void Start()
    {
        Destroy(GetComponentInChildren<SpriteRenderer>()); // Remove component from prefab when no longer needed
        
        
        if (!hasBeenDropped)
        {
            item = new Item(itemObject);
        }
        

        if (model == null)
        {
            if (itemObject.model)
            {
                model = Instantiate(itemObject.model, transform.position, quaternion.identity, transform);
            }
            else
            {
                print("Database model is null");
            }
        }
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
