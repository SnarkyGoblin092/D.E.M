using LitJson;
using System.IO;
using UnityEngine;
using System.Collections.Generic;

public class ItemDatabase : MonoBehaviour
{
    private List<Item> database = new List<Item>();
    private JsonData itemData;

    private void Start()
    {
        itemData = JsonMapper.ToObject(File.ReadAllText(Application.dataPath + "/StreamingAssets/Items.json"));
        
        ConstructItemDatabase();
    }

    public Item GetItemByID(int id)
    {
        foreach(Item item in database)
        {
            if(item.ID == id)
            {
                return item;
            }
        }
        return null;
    }

    public Item GetItemByTitle(string title)
    {
        foreach (Item item in database)
        {
            if (item.Title == title)
            {
                return item;
            }
        }
        return null;
    }

    private void ConstructItemDatabase()
    {
        for(int i = 0; i < itemData.Count; i++)
        {
            database.Add(new Item(
                (int)itemData[i]["id"],
                (string)itemData[i]["title"], 
                (int)itemData[i]["stats"]["hunger"],
                (int)itemData[i]["stats"]["thirst"],
                (string)itemData[i]["description"],
                (int)itemData[i]["stack_size"],
                (string)itemData[i]["slug"],
                (bool)itemData[i]["droppable"],
                (bool)itemData[i]["pickuppable"],
                (bool)itemData[i]["interactable"]
                ));
        }
    }
}

public class Item
{
    public int ID { get; set; }
    public string Title { get; set; }
    public int Hunger { get; set; }
    public int Thirst { get; set; }
    public string Description { get; set; }
    public int StackSize { get; set; }
    public string Slug { get; set; }
    public bool Droppable { get; set; }
    public bool Pickuppable { get; set; }
    public bool Interactable { get; set; }
    public int Amount { get; set; }
    public Sprite Sprite { get; set; }

    public Item(int id, string title, int hunger, int thirst, string description, int stackSize, string slug, bool droppable, bool pickuppable, bool interactable)
    {
        ID = id;
        Title = title;
        Hunger = hunger;
        Thirst = thirst;
        Description = description;
        StackSize = stackSize;
        Slug = slug;
        Droppable = droppable;
        Pickuppable = pickuppable;
        Interactable = interactable;
        Amount = 1;
        Sprite = Resources.Load<Sprite>("Items/" + slug);
    }

    public Item()
    {
        ID = -1;
    }
}
