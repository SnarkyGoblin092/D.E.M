using UnityEngine;

public class Interactable : MonoBehaviour
{
    public enum types { ITEM, INTERACTABLE }

    public int id;
    [SerializeField] types type;
    public Item item;

    private void Start()
    {
        item = ItemDatabase.instance.GetItem(id);
    }

    public types GetObjectType => type;
}
