﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item
{
    public class Ingredient
    {
        public Item item;
        public int amount;
    }

    public int id;
    public string itemName;
    public string description;
    public int count;
    public Sprite icon;
    public List<Ingredient> recipe;
    public Dictionary<string, int> stats = new Dictionary<string, int>();

    public Item(int id, string itemName, string description, Dictionary<string, int> stats)
    {
        this.id = id;
        this.itemName = itemName;
        this.description = description;
        this.icon = Resources.Load<Sprite>($"Items/{itemName}");
        this.stats = stats;
    }

    public Item(Item item)
    {
        this.id = item.id;
        this.itemName = item.itemName;
        this.description = item.description;
        this.icon = Resources.Load<Sprite>($"Items/{item.itemName}");
        this.stats = item.stats;
    }
    
}