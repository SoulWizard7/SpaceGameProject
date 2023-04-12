using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class InteractionController : MonoBehaviour
{
    public InteractableObject curObject;
    public PlayerShooting playerShooting {get; private set;}

    // Used for save functions and AddItem in GroundItem script
    public InventoryObject inventory;
    public InventoryObject equipment;
    public InventoryObject weapons;

    public UIController UIController;
    public GameObject itemPrefab;

    public float interactionMaxDistance = 2f;
    
    private void Awake()
    {
        playerShooting = GetComponent<PlayerShooting>();
    }

    void Update()
    {
        if (Input.GetButtonDown("InteractButton"))
        {
            TryInteract();
        }

        if (Input.GetButtonDown("InventoryButton"))
        {
            UIController.ToggleInventory();
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            SaveInventory();
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            LoadInventory();
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            playerShooting.currentWeaponIndex = 0;
            playerShooting.CheckWeapon();
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            playerShooting.currentWeaponIndex = 1;
            playerShooting.CheckWeapon();
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            playerShooting.currentWeaponIndex = 2;
            playerShooting.CheckWeapon();
        }
    }

    public void SaveInventory()
    {
        inventory.Save();
        equipment.Save();
        weapons.Save();
    }

    public void LoadInventory()
    {
        inventory.Load();
        equipment.Load();
        weapons.Load();
        playerShooting.CheckWeapon();
    }

    private void TryInteract()
    {
        curObject = GetInteractable(transform.position - transform.forward * 0.3f, transform.forward); // this is kind of ass
        if (curObject) curObject.Interact(this);
    }

    private InteractableObject GetInteractable(Vector3 startPos, Vector3 dir)
    {
        RaycastHit hit;

        if (Physics.BoxCast(startPos, new Vector3(0.5f, 2f, 0.1f), dir, out hit, transform.rotation, interactionMaxDistance))
        {
            InteractableObject interactableObject = hit.collider.GetComponent<InteractableObject>();
            if (interactableObject)
            {
                Debug.DrawLine(startPos, startPos + (dir * 3), Color.cyan, 1f);
                return interactableObject;
            }
        }
        
        Debug.DrawRay(startPos, dir, Color.red, 1f);
        return null;
    }

    public void SpawnObject(ItemObject itemObject, Item item)
    {
        GroundItem groundItem = Instantiate(itemPrefab, transform.position, Quaternion.identity).GetComponent<GroundItem>();
        groundItem.hasBeenDropped = true;
        groundItem.itemObject = itemObject;
        groundItem.item = item;
        groundItem.StartCoroutine(groundItem.DropAnim(transform.forward));
    }

    private void OnApplicationQuit()
    {
        inventory.Clear();
        equipment.Clear();
        weapons.Clear();
    }
}

