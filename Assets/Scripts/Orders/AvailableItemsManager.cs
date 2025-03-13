using System.Collections.Generic;
using UnityEngine;

public class AvailableItemsManager : MonoBehaviour
{
    public static AvailableItemsManager Instance { get; private set; }
    public Dictionary<string, int> AvailableItems { get; private set; } = new Dictionary<string, int>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        UpdateAvailableItems();
    }

    public void UpdateAvailableItems()
    {
        AvailableItems.Clear();
        var itemsInScene = GameObject.FindGameObjectsWithTag("Obtainable Item");

        foreach (var item in itemsInScene)
        {
            var product = item.GetComponent<ProductName>();
            if (product == null) continue;

            string productName = product.Name;
            if (AvailableItems.ContainsKey(productName))
            {
                AvailableItems[productName]++;
            }
            else
            {
                AvailableItems.Add(productName, 1);
            }
        }

        //DebugAvailableItems("After scanning scene:");
    }

    public void DecreaseItem(string productName, int amount)
    {
        if (AvailableItems.ContainsKey(productName))
        {
            AvailableItems[productName] -= amount;
            if (AvailableItems[productName] <= 0)
            {
                AvailableItems.Remove(productName);
                Debug.Log($"'{productName}' removed from AvailableItems.");
            }

            //DebugAvailableItems($"After decreasing '{productName}' by {amount}:");
        }
        else
        {
            Debug.LogWarning($"Tried to decrease '{productName}', but it doesn't exist in AvailableItems!");
        }
    }

    public void IncreaseItem(string productName, int amount = 1)
    {
        if (AvailableItems.ContainsKey(productName))
        {
            AvailableItems[productName] += amount;
        }
        else
        {
            AvailableItems.Add(productName, amount);
        }

        //DebugAvailableItems($"After adding '{productName}' x{amount}:");
    }

    //private void DebugAvailableItems(string message)
    //{
    //    Debug.Log($"{message} Current Available Items:");
    //    foreach (var item in AvailableItems)
    //    {
    //        Debug.Log($"Item: {item.Key}, Count: {item.Value}");
    //    }
    //}
}
