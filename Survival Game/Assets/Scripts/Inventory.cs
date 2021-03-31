using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public List<Item> characterItems = new List<Item>();
    public UIInventory inventoryUI;

    private void Start()
    {
        GiveItem(0);
        GiveItem(1);
        GiveItem(2);
        GiveItem(3);
        GiveItem(4);
        GiveItem(5);
    }

    public void GiveItem(int id)
    {
        Item itemToAdd = ItemDatabase.instance.GetItem(id);
        characterItems.Add(itemToAdd);
        inventoryUI.AddNewItem(itemToAdd);
        Debug.Log($"Added item: {itemToAdd.itemName}");
    }

    public void GiveItem(string itemName)
    {
        Item itemToAdd = ItemDatabase.instance.GetItem(itemName);
        characterItems.Add(itemToAdd);
        inventoryUI.AddNewItem(itemToAdd);
        Debug.Log($"Added item: {itemToAdd.itemName}");
    }

    public Item CheckForItem(int id)
    {
        return characterItems.Find(item => item.id == id);
    }

    public Item CheckForItem(string itemName)
    {
        return characterItems.Find(item => item.itemName == itemName);
    }

    public void RemoveItem(int id)
    {
        Item itemToRemove = CheckForItem(id);
        if(itemToRemove != null)
        {
            characterItems.Remove(itemToRemove);
            inventoryUI.RemoveItem(itemToRemove);
            Debug.Log($"Item removed: {itemToRemove.itemName}");
        }
    }

    public void RemoveItem(string itemName)
    {
        Item itemToRemove = CheckForItem(itemName);
        if (itemToRemove != null)
        {
            characterItems.Remove(itemToRemove);
            inventoryUI.RemoveItem(itemToRemove);
            Debug.Log($"Item removed: {itemToRemove.itemName}");
        }
    }
}
