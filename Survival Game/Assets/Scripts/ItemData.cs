using UnityEngine;
using UnityEngine.EventSystems;

public class ItemData : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
{
    public Item item;
    public string itemName;
    public int amount = 1;
    public int slot;

    private Inventory inv;
    private Tooltip tooltip;
    private Vector2 offset;

    private void Start()
    {
        inv = GameObject.Find("Inventory").GetComponent<Inventory>();
        tooltip = inv.GetComponent<Tooltip>();
    }

    private void Update() {
        itemName = item.Title;
        item.Amount = amount;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        offset = eventData.position - new Vector2(transform.position.x, transform.position.y);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if(item != null)
        {
            transform.parent.transform.name = "Slot(Clone)";
            transform.SetParent(transform.parent.parent.parent);
            transform.position = eventData.position - offset;
            transform.localScale = Vector3.one;
            GetComponent<CanvasGroup>().blocksRaycasts = false;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (item != null)
        {
            transform.position = eventData.position - offset;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        transform.SetParent(inv.slots[slot].transform);
        transform.position = inv.slots[slot].transform.position;
        GetComponent<CanvasGroup>().blocksRaycasts = true;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        tooltip.Activate(item);
        inv.deleteID = slot;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        tooltip.Deactivate();
        inv.deleteID = -1;
    }
}
