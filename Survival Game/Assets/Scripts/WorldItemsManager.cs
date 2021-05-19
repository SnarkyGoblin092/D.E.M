using System.Collections.Generic;
using UnityEngine;

public class WorldItemsManager : MonoBehaviour
{
    public List<GameObject> prefabs = new List<GameObject>();
    public List<GameObject> items = new List<GameObject>();
    public List<GameObject> tempItems = new List<GameObject>();
    [SerializeField] float distanceForMergingItems = 2f;
    [SerializeField] float timeToCheck = .5f;
    [SerializeField] LayerMask itemMask;

    float timeBeforeCheck;

    private void Start() {
        timeBeforeCheck = timeToCheck;
    }

    private void Update()
    {
        timeBeforeCheck -= Time.deltaTime;
        
        if (timeBeforeCheck <= 0)
        {   
            if (items.Count > 0)
            {
                foreach (GameObject item in items)
                {
                    int index = -1;

                    if (tempItems.Count > 0)
                    {
                        index = FindItemWithID(item);
                    }

                    if (index != -1 && item.GetComponent<ObjectData>().item.StackSize >= tempItems[index].GetComponent<ObjectData>().amount + item.GetComponent<ObjectData>().amount)
                    {
                        tempItems[index].GetComponent<ObjectData>().amount += item.GetComponent<ObjectData>().amount;
                    }
                    else
                    {
                        tempItems.Add(item);
                    }
                }

                if (tempItems.Count > 0)
                {
                    for (int i = 0; i < items.Count;)
                    {
                        if (!tempItems.Contains(items[i]))
                        {
                            Destroy(items[i]);
                            items.RemoveAt(i);
                        }
                        else
                        {
                            i++;
                        }
                    }

                    tempItems.Clear();
                }
            }

            timeBeforeCheck = timeToCheck;
        }
    }

    int FindItemWithID(GameObject item)
    {
        foreach (GameObject tempItem in tempItems)
        {
            float distance = Vector3.Distance(item.transform.position, tempItem.transform.position);

            if (distance <= distanceForMergingItems)
            {
                if (item.GetComponent<ObjectData>().item.ID == tempItem.GetComponent<ObjectData>().item.ID)
                {
                    return tempItems.IndexOf(tempItem);
                }
            }
        }

        return -1;
    }
}
