using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Inventory : MonoBehaviour
{
    [SerializeField] GameObject inventoryPanel;
    [SerializeField] GameObject slotPanel;
    [SerializeField] GameObject hotbarPanel;
    [SerializeField] GameObject tooltip;
    [SerializeField] WorldItemsManager worldItemsManager;

    public ItemDatabase database;
    public GameObject inventorySlot;
    public GameObject inventoryItem;

    public int deleteID = -1;

    public readonly int inventorySlotAmount = 36;
    public readonly int hotbarSlotAmount = 9;
    public List<Item> items = new List<Item>();
    public List<GameObject> slots = new List<GameObject>();

    public int selectedSlot;
    int previousSelectedSlot;

    public Color baseColor;
    public Color selectedColor;

    public float itemDropPower;

    private KeyCode[] keyCodes = new KeyCode[] { KeyCode.Alpha1, KeyCode.Alpha2, KeyCode.Alpha3, 
                                                 KeyCode.Alpha4, KeyCode.Alpha5, KeyCode.Alpha6, 
                                                 KeyCode.Alpha7, KeyCode.Alpha8, KeyCode.Alpha9 };

    private void Start()
    {
        database = GetComponent<ItemDatabase>();
        worldItemsManager = GameObject.Find("Managers").GetComponent<WorldItemsManager>();

        selectedSlot = inventorySlotAmount;
        previousSelectedSlot = inventorySlotAmount;

        for (int i = 0; i < inventorySlotAmount + hotbarSlotAmount; i++)
        {
            if(i < inventorySlotAmount)
            {
                items.Add(new Item());
                slots.Add(Instantiate(inventorySlot));

                slots[i].GetComponent<Slot>().id = i;
                slots[i].transform.SetParent(slotPanel.transform);
                slots[i].transform.localScale = Vector3.one;
            }
            else
            {
                items.Add(new Item());
                slots.Add(Instantiate(inventorySlot));

                slots[i].GetComponent<Slot>().id = i;
                slots[i].transform.SetParent(hotbarPanel.transform);
                slots[i].transform.localScale = Vector3.one;
            }
            
        }
        
        AddItem(0, 1);
        AddItem(1, 1);
        AddItem(2, 1);
        AddItem(3, 1);
        AddItem(4, 1);
        AddItem(5, 1);
        AddItem(8, 1);
        AddItem(9, 1);
        AddItem(10, 1);

        for(int i = 0; i < items.Count;i++){
            Debug.Log(items[i].ID);
        }
    }

    private void Update()
    {
        if (!inventoryPanel.activeSelf)
        {
            deleteID = selectedSlot;
            slots[selectedSlot].transform.localScale = Vector3.one * 1.2f;
            slots[selectedSlot].GetComponent<Image>().color = selectedColor;

            ChangeHotbar(false);
        }
        else
        {
            slots[selectedSlot].transform.localScale = Vector3.one;
            slots[selectedSlot].GetComponent<Image>().color = baseColor;

            ChangeHotbar(true);
        }

        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.Q))
        {
            DropItem(1, true);
        }
        else if(Input.GetKeyDown(KeyCode.Q))
        {
            DropItem(1, false);
        }

        if (Input.GetAxisRaw("Mouse ScrollWheel") > 0f)
        {
            SelectSlot(-1);
        }

        if (Input.GetAxisRaw("Mouse ScrollWheel") < 0f)
        {
            SelectSlot(1);
        }

        for (int i = 0; i < keyCodes.Length; ++i)
        {
            if (Input.GetKeyDown(keyCodes[i]))
            {
                SelectSlotWithNumber(i + 1);
            }
        }

        for (int i = 0; i < slots.Count; i++) {

            ItemData data = null;
            if (slots[i].transform.childCount > 0)
            {
                data = slots[i].transform.GetChild(0).GetComponent<ItemData>();
            }
            if (data != null)
            {
                if (data.amount != 1)
                {
                    data.transform.GetChild(0).GetComponent<TMP_Text>().text = data.amount.ToString();
                }
                else
                {
                    data.transform.GetChild(0).GetComponent<TMP_Text>().text = "";
                }
            }
        }
    }

    public void ChangeHotbar(bool state)
    {
        GridLayoutGroup glg = hotbarPanel.GetComponent<GridLayoutGroup>();

        if (state)
        {
            glg.spacing = new Vector2(5f, 5f);
            hotbarPanel.GetComponent<RectTransform>().sizeDelta = new Vector2(790, 110);
            //hotbarPanel.GetComponent<Image>().color = new Color(255, 255, 255, 255);
            hotbarPanel.GetComponent<RectTransform>().localPosition = new Vector3(0, -253f ,0);
        }
        else
        {
            glg.spacing = new Vector2(20f, 5f);
            hotbarPanel.GetComponent<RectTransform>().sizeDelta = new Vector2(910, 110);
            //hotbarPanel.GetComponent<Image>().color = new Color(255, 255, 255, 0);
            hotbarPanel.GetComponent<RectTransform>().localPosition = new Vector3(0, -475f, 0);
        }
    }

    public void SelectSlot(int input)
    {
        if (!inventoryPanel.activeSelf)
        {
            if (selectedSlot + input > inventorySlotAmount + hotbarSlotAmount - 1)
            {
                selectedSlot = inventorySlotAmount + hotbarSlotAmount - 1;
            }
            else if (selectedSlot + input < inventorySlotAmount)
            {
                selectedSlot = inventorySlotAmount;
            }
            else
            {
                selectedSlot += input;

                slots[previousSelectedSlot].transform.localScale = Vector3.one;
                slots[previousSelectedSlot].GetComponent<Image>().color = baseColor;

                previousSelectedSlot = selectedSlot;
            }
        }
    }

    public void SelectSlotWithNumber(int input)
    {
        if (!inventoryPanel.activeSelf)
        {
            selectedSlot = inventorySlotAmount - 1 + input;

            slots[previousSelectedSlot].transform.localScale = Vector3.one;
            slots[previousSelectedSlot].GetComponent<Image>().color = baseColor;

            previousSelectedSlot = selectedSlot;
        }
    }

    public void AddItem(int _id, int _amount)
    {
        Item itemToAdd = database.GetItemByID(_id);
        int index = CheckInventory(itemToAdd);

        if(index == -1)
        {
            for(int i = 0; i < items.Count; i++)
            {
                Debug.Log(items[i].ID);
                if(items[i].ID == -1){
                    GameObject itemObj = Instantiate(inventoryItem);
                    ItemData itemData = itemObj.GetComponent<ItemData>();

                    int leftOverAmount = 0;

                    if (_amount > itemToAdd.StackSize)
                    {
                        itemData.amount = itemToAdd.StackSize;
                        itemToAdd.Amount = itemToAdd.StackSize;

                        leftOverAmount = _amount - itemToAdd.StackSize;
                    }
                    else
                    {
                        itemData.amount = _amount;
                        itemToAdd.Amount = _amount;
                    }

                    items[i] = itemToAdd;
                    itemData.item = itemToAdd;
                    itemData.slot = i;

                    slots[i].name = itemToAdd.Title;
                    itemObj.GetComponent<Image>().enabled = true;
                    itemObj.GetComponent<Image>().sprite = itemToAdd.Sprite;

                    itemObj.transform.SetParent(slots[i].transform);
                    itemObj.transform.position = Vector3.zero;
                    itemObj.transform.localScale = Vector3.one;
                    itemObj.transform.localPosition = Vector3.zero;

                    if(leftOverAmount > 0){
                        AddItem(_id, leftOverAmount);
                    }

                    break;
                }
                
            }
            Debug.Log("---------------");
        }
        else
        {
            ItemData data = slots[index].transform.GetChild(0).GetComponent<ItemData>();
            
            if(data.amount == data.item.StackSize)
            {
                for(int i = 0; i < items.Count; i++)
                {
                    if(items[i].ID == -1){
                        GameObject itemObj = Instantiate(inventoryItem);
                        ItemData itemData = itemObj.GetComponent<ItemData>();

                        int leftOverAmount = 0;

                        if (_amount > itemToAdd.StackSize)
                        {
                            itemData.amount = itemToAdd.StackSize;
                            itemToAdd.Amount = itemToAdd.StackSize;

                            leftOverAmount = _amount - itemToAdd.StackSize;
                        }
                        else
                        {
                            itemData.amount = _amount;
                            itemToAdd.Amount = _amount;
                        }

                        items[i] = itemToAdd;
                        itemData.item = itemToAdd;
                        itemData.slot = i;

                        slots[i].name = itemToAdd.Title;
                        itemObj.GetComponent<Image>().enabled = true;
                        itemObj.GetComponent<Image>().sprite = itemToAdd.Sprite;

                        itemObj.transform.SetParent(slots[i].transform);
                        itemObj.transform.position = Vector3.zero;
                        itemObj.transform.localScale = Vector3.one;
                        itemObj.transform.localPosition = Vector3.zero;

                        if(leftOverAmount > 0){
                            AddItem(_id, leftOverAmount);
                        }

                        break;
                    }
                }
            }
            else 
            {
                if (data.amount + _amount > itemToAdd.StackSize)
                {
                    data.amount = itemToAdd.StackSize;
                    items[index].Amount = itemToAdd.StackSize;
                }
                else
                {
                    data.amount += _amount;
                    items[index].Amount += _amount;
                }
            }
        }
    } 

    public void AddItem(string _name, int _amount)
    {
        Item itemToAdd = database.GetItemByTitle(_name);
        int index = CheckInventory(itemToAdd);
        
        if(index == -1)
        {
            for(int i = 0; i < items.Count; i++)
            {
                if(items[i].ID == -1){
                    GameObject itemObj = Instantiate(inventoryItem);
                    ItemData itemData = itemObj.GetComponent<ItemData>();

                    if (_amount > itemToAdd.StackSize)
                    {
                        itemData.amount = itemToAdd.StackSize;
                        itemToAdd.Amount = itemToAdd.StackSize;
                    }
                    else
                    {
                        itemData.amount = _amount;
                        itemToAdd.Amount = _amount;
                    }

                    int leftOverAmount = _amount - itemToAdd.StackSize;

                    items[i] = itemToAdd;
                    itemData.item = itemToAdd;
                    itemData.slot = i;

                    slots[i].name = itemToAdd.Title;
                    itemObj.GetComponent<Image>().enabled = true;
                    itemObj.GetComponent<Image>().sprite = itemToAdd.Sprite;

                    itemObj.transform.SetParent(slots[i].transform);
                    itemObj.transform.position = Vector3.zero;
                    itemObj.transform.localScale = Vector3.one;
                    itemObj.transform.localPosition = Vector3.zero;

                    if(leftOverAmount > 0){
                        AddItem(_name, leftOverAmount);
                    }

                    break;
                }
            }
        }
        else
        {
            ItemData data = slots[index].transform.GetChild(0).GetComponent<ItemData>();
            
            items[index].Amount += _amount;
        }
    } 

    public void RemoveItem(int _amount, bool all)
    {
        if (deleteID != -1)
        {
            Item itemToRemove = database.GetItemByID(deleteID);
            int index = CheckInventory(itemToRemove);

            if (items.Contains(itemToRemove))
            {
                if (all)
                {
                    items[index].ID = -1;
                    Destroy(slots[index].transform.GetChild(0).gameObject);
                    slots[index].transform.name = "Slot(Clone)";
                }
                else
                {
                    ItemData data = slots[index].transform.GetChild(0).GetComponent<ItemData>();
                    data.amount -= _amount;

                    if (data.amount != 1)
                        data.transform.GetChild(0).GetComponent<TMP_Text>().text = data.amount.ToString();
                    else
                    {
                        data.transform.GetChild(0).GetComponent<TMP_Text>().text = "";
                    }

                    if (data.amount <= 0)
                    {
                        items[index].ID = -1;
                        Destroy(slots[index].transform.GetChild(0).gameObject);
                        slots[index].transform.name = "Slot(Clone)";
                    }
                }
            }
        }
    }

    public void DropItem(int _amount, bool all)
    {
        if (deleteID != -1)
        {
            if (slots[deleteID].transform.childCount > 0)
            {
                ItemData itemData = slots[deleteID].transform.GetChild(0).GetComponent<ItemData>();

                if (itemData.item.ID != -1)
                {
                    Item itemToDrop = database.GetItemByID(itemData.item.ID);
                    //int index = CheckInventory(itemToDrop);

                    if (itemToDrop.Droppable)
                    {
                        if (items[deleteID].ID != -1)
                        {
                            //ItemData data = slots[index].transform.GetChild(0).GetComponent<ItemData>();

                            GameObject spawner = GameObject.Find("Camera Holder");
                            Transform parent = GameObject.Find("Environment").transform.Find("ItemsInTheWorld").transform;

                            GameObject droppedItem = Instantiate(worldItemsManager.prefabs[itemData.item.ID], spawner.transform.position, spawner.transform.rotation, parent);

                            if (all)
                            {
                                droppedItem.GetComponent<ObjectData>().amount = itemData.amount;
                                itemData.amount = 0;
                                items[deleteID].Amount = 0;
                            }
                            else
                            {
                                droppedItem.GetComponent<ObjectData>().amount = _amount;
                                itemData.amount -= _amount;
                                items[deleteID].Amount -= _amount;
                            }

                            droppedItem.GetComponent<Rigidbody>().AddForce(spawner.transform.forward * itemDropPower, ForceMode.Impulse);
                            worldItemsManager.items.Add(droppedItem);

                            if (itemData.amount <= 0)
                            {
                                items[deleteID] = new Item();
                                Destroy(slots[deleteID].transform.GetChild(0).gameObject);
                                slots[deleteID].transform.name = "Slot(Clone)";
                                tooltip.SetActive(false);
                            }
                        }
                    }
                }
            }
        }
    }

    public int CheckInventory(Item item)
    {
        /*
        for (int i = items.Count - 1; i >= 0; i--)
        {
            if (items[i].ID == item.ID)
            {
                if(items[i].Amount < items[i].StackSize)
                {
                    return i;
                }
            }
        }
        */

        for (int i = 0 ; i < items.Count; i++)
        {
            if (items[i].ID == item.ID)
            {
                if(items[i].Amount < items[i].StackSize)
                {
                    return i;
                }
            }
        }
        return -1;
    }

    public int CheckInventorySpace(Item item)
    {
        int freeSpace = items.Count * item.StackSize;
        /*
        for(int i = items.Count - 1; i >= 0; i--)
        {
            if(items[i].ID == -1)
            {
                freeSpace += item.StackSize;
            }
            else if(items[i].ID == item.ID)
            {
                freeSpace += item.StackSize - items[i].Amount;
            }
        }
        */

        for(int i = 0; i < items.Count; i++)
        {
            if(items[i].ID == item.ID)
            {
                freeSpace -= item.StackSize - items[i].Amount;
            } 
            else if (items[i].ID != -1)
            {
                freeSpace -= item.StackSize;
            }
            Debug.Log(items[i].Amount + "   "+ freeSpace);
        }
        
        return freeSpace;
    }
}
