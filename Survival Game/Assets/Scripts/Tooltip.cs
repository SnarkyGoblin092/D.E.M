using UnityEngine;
using TMPro;

public class Tooltip : MonoBehaviour
{
    private Item item;
    private string data;
    [SerializeField] GameObject tooltip;

    private void Start()
    {
        tooltip.SetActive(false);
    }

    private void Update()
    {
        if (tooltip.activeSelf)
        {
            tooltip.transform.position = Input.mousePosition + new Vector3(15, -15, 0);
        }
    }

    public void Activate(Item _item)
    {
        item = _item;
        ContructDataString();
        tooltip.transform.GetChild(0).GetComponent<TMP_Text>().text = data;
        tooltip.SetActive(true);
    }

    public void Deactivate()
    {
        tooltip.SetActive(false);
    }

    public void ContructDataString()
    {
        data = "<b>" + item.Title + "</b>\n\n<b>Description:</b>\n   " + item.Description;

        if (item.Hunger != 0 && item.Thirst != 0)
        {
            data += "\n\n<b>Stats:</b>\n   Hunger: " + item.Hunger + "\n   Thirst: " + item.Thirst;
        }
        else if (item.Hunger != 0 && item.Thirst == 0)
        {
            data += "\n\n<b>Stats:</b>\n   Hunger: " + item.Hunger;
        }
        else if (item.Hunger == 0 && item.Thirst != 0)
        {
            data += "\n\n<b>Stats:</b>\n   Thirst: " + item.Thirst;
        }

        data += "\n\n<b>Drop Item [Q]</b>";
    }
}
