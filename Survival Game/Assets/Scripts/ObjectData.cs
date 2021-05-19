using UnityEngine;

public class ObjectData : MonoBehaviour
{
    [SerializeField] int itemID;

    ItemDatabase database;
    WorldItemsManager manager;

    public Item item;
    public int amount = 1;
    public bool interactable;
    public bool pickuppable;

    private void Start()
    {
        database = GameObject.Find("Inventory").GetComponent<ItemDatabase>();
        manager = GameObject.Find("Managers").GetComponent<WorldItemsManager>();
        item = database.GetItemByID(itemID);

        interactable = item.Interactable;
        pickuppable = item.Pickuppable;
    }
}
