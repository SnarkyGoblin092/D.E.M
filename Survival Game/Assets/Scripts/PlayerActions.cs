using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerActions : MonoBehaviour
{
    [SerializeField] Camera cam;
    [SerializeField] GameObject inventoryPanel;
    [SerializeField] GameObject pickupText;
    [SerializeField] GameObject interactText;
    [SerializeField] Inventory playerInventory;
    [SerializeField] float rayDistance = 3f;

    GameObject previousObjectLookedAt;

    private void Awake()
    {
        cam = FindObjectOfType<Camera>();
        inventoryPanel = GameObject.Find("Canvas").transform.Find("InventoryPanel").gameObject;
        pickupText = GameObject.Find("Canvas").transform.Find("PickupText").gameObject;
        interactText = GameObject.Find("Canvas").transform.Find("InteractText").gameObject;
        playerInventory = transform.GetComponent<Inventory>();
    }

    private void Update()
    {
        CheckInputs();

        if (!inventoryPanel.activeSelf)
        {
            LockMouse();
            GetComponent<PlayerMovement>().canMove = true;
            CheckRay();
        } else
        {
            DisableKeyPopups();
            GetComponent<PlayerMovement>().canMove = false;
            ReleaseMouse();
        }
    }

    void CheckInputs()
    {
        if (InputManager.instance.GetKeyDown("inventory"))
            inventoryPanel.SetActive(!inventoryPanel.activeSelf);
    }

    void CheckRay()
    {
        Ray ray = new Ray(cam.transform.position, cam.transform.forward);

        if (Physics.Raycast(ray, out RaycastHit hit, rayDistance))
        {
            GameObject hitObject = hit.transform.gameObject;

            if (hitObject.GetComponent<Interactable>())
            {
                if(previousObjectLookedAt != hitObject)
                {
                    DisableKeyPopups();
                }

                if (hitObject.GetComponent<Interactable>().GetObjectType == Interactable.types.ITEM)
                {
                    pickupText.GetComponent<Text>().text = $"Pickup [{InputManager.instance.keybindings.interact}]";
                    pickupText.SetActive(true);

                    if (InputManager.instance.GetKeyDown("interact"))
                    {
                        Debug.Log(hitObject.transform.name);
                        PickupItem(hitObject);
                    }
                }
                else if (hitObject.GetComponent<Interactable>().GetObjectType == Interactable.types.INTERACTABLE)
                {
                    interactText.GetComponent<Text>().text = $"Interact [{InputManager.instance.keybindings.interact}]";
                    interactText.SetActive(true);
                }
            }

            previousObjectLookedAt = hitObject;
        }
        else
        {
            DisableKeyPopups();
        }
    }

    void DisableKeyPopups()
    {
        pickupText.SetActive(false);
        interactText.SetActive(false);
    }

    void LockMouse()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void ReleaseMouse()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    void PickupItem(GameObject hit)
    {
        Item item = hit.GetComponent<Interactable>().item;
        Debug.Log(item.itemName);
        playerInventory.GiveItem(item.id);
        Destroy(hit);
    }
}
