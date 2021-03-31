using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDatabase : MonoBehaviour
{
    public static ItemDatabase instance;
    public List<Item> items = new List<Item>();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(this);
        }

        DontDestroyOnLoad(this);

        BuildDatabase();
    }

    void BuildDatabase()
    {
        items = new List<Item> {
                new Item(0, "Banana", "It's a banana.",
                    new Dictionary<string, int>
                    {
                        {"Hunger Fill", 10 },
                        {"Thirst Fill", 5}
                    }),
                new Item(1, "Coconut", "It's a coconut.",
                    new Dictionary<string, int>
                    {}),
                new Item(2, "Open Coconut", "It's an opened coconut.",
                    new Dictionary<string, int>
                    {
                        {"Hunger Fill", 5 },
                        {"Thirst Fill", 15}
                    }),
                new Item(3, "Scroll", "It's a mysterious scroll.",
                    new Dictionary<string, int>
                    {
                        {"Mystery", 69}
                    }),
                new Item(4, "Branch", "It's a useless branch.",
                    new Dictionary<string, int>
                    {}),
                new Item(5, "Bed", "It's a not so comfy bed.",
                    new Dictionary<string, int>
                    {
                        {"Comfort", -2}
                    })
                };
    }

    public Item GetItem(int id)
    {
        return items.Find(item => item.id == id);
    }

    public Item GetItem(string itemName)
    {
        return items.Find(item => item.itemName == itemName);
    }
}
