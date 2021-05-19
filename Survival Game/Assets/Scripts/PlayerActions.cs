using UnityEngine;
using TMPro;

public class PlayerActions : MonoBehaviour
{
    [SerializeField] Camera cam;
    [SerializeField] GameObject itemTooltip;
    [SerializeField] float rayDistance = 3f;
    [SerializeField] GameObject titlePanel;
    [SerializeField] GameObject slotPanel;
    [SerializeField] GameObject inventoryPanel;
    [SerializeField] GameObject tooltip;

    [SerializeField] Inventory inventory;
    [SerializeField] WorldItemsManager worldItemsManager;

    GameObject previousObjectLookedAt;

    PlayerMovement pm;

    private void Awake()
    {
        cam = FindObjectOfType<Camera>();
        worldItemsManager = GameObject.Find("Managers").GetComponent<WorldItemsManager>();
        inventory = GameObject.Find("Inventory").GetComponent<Inventory>();
        pm = gameObject.GetComponent<PlayerMovement>();
    }

    private void Update()
    {
        CheckInputs();

        if (!inventoryPanel.activeSelf)
        {
            tooltip.SetActive(false);
            CheckRay();
        } else
        {
            DisableKeyPopups();
        }

        if(pm.canMove){
            LockMouse();
        } else {
            ReleaseMouse();
        }
    }

    void CheckInputs()
    {
        if(GetComponent<PlayerMovement>().canToggleInventory){
            if (InputManager.instance.GetKeyDown("inventory"))
            {
                titlePanel.SetActive(!titlePanel.activeSelf);
                slotPanel.SetActive(!slotPanel.activeSelf);
                inventoryPanel.SetActive(!inventoryPanel.activeSelf);
                
                GetComponent<PlayerMovement>().canMove = !GetComponent<PlayerMovement>().canMove;

                if (inventoryPanel.activeSelf)
                {
                    inventory.deleteID = -1;
                }
            }
        }
    }

    void CheckRay()
    {
        Ray ray = new Ray(cam.transform.position, cam.transform.forward);

        if (Physics.Raycast(ray, out RaycastHit hit, rayDistance))
        {
            GameObject hitObject = hit.transform.gameObject;
            ObjectData data;
            if (data = hitObject.GetComponent<ObjectData>())
            {
                if (data.item.Pickuppable)
                {
                    if(data.amount > 1){
                        itemTooltip.GetComponent<TMP_Text>().text = $"{data.item.Title} {data.amount}\n\nPickup [{InputManager.instance.keybindings.interact}]";
                    }
                    else 
                    {
                        itemTooltip.GetComponent<TMP_Text>().text = $"{data.item.Title}\n\nPickup [{InputManager.instance.keybindings.interact}]";
                    }
                    itemTooltip.SetActive(true);

                    if (InputManager.instance.GetKeyDown("interact") && pm.canMove)
                    {
                        PickupItem(hitObject);
                    }
                }
                else if (data.item.Interactable)
                {
                    itemTooltip.GetComponent<TMP_Text>().text = $"Interact [{InputManager.instance.keybindings.interact}]";
                    itemTooltip.SetActive(true);

                    if (InputManager.instance.GetKeyDown("interact") && pm.canMove)
                    {
                        if(hitObject.GetComponent<Bed>()){
                            hitObject.GetComponent<Bed>().SkipTime();
                        }
                        else if(hitObject.GetComponent<Campfire>()){
                            hitObject.GetComponent<Campfire>().playing = !hitObject.GetComponent<Campfire>().playing;
                        } 
                    }

                    
                }
            }
            else
            {
                DisableKeyPopups();
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
        itemTooltip.SetActive(false);
    }

    public void LockMouse()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void ReleaseMouse()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    void PickupItem(GameObject hit)
    {
        ObjectData objectData = hit.GetComponent<ObjectData>();
        int freeScace = inventory.CheckInventorySpace(objectData.item);
        
        Debug.Log(freeScace);

        int index;

        if((index = inventory.CheckInventory(objectData.item)) != -1)
        {
            int amountToAdd = objectData.item.StackSize - inventory.items[index].Amount;
            int groundAmount = objectData.amount - amountToAdd;

            if (groundAmount < 0)
            {
                amountToAdd += groundAmount;
            }

            inventory.AddItem(objectData.item.ID, amountToAdd);

            objectData.amount -= amountToAdd;
            objectData.item.Amount -= amountToAdd;
        } 
        else 
        {
            int amountToAdd = 0;

            if(objectData.amount >= objectData.item.StackSize){
                amountToAdd = objectData.item.StackSize;
            }
            else 
            {
                amountToAdd = objectData.amount;
            }

            if(freeScace >= amountToAdd)
            {
                objectData.amount -= amountToAdd;
                objectData.item.Amount -= amountToAdd;

                inventory.AddItem(objectData.item.ID, amountToAdd);
            } 
            else 
            {
                objectData.amount -= freeScace;
                objectData.item.Amount -= freeScace;

                inventory.AddItem(objectData.item.ID, freeScace);
            }
        }

        if(objectData.amount <= 0) 
        {
            worldItemsManager.items.Remove(hit);
            Destroy(hit);
        }
    }
}
