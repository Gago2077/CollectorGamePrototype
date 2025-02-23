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

            string productName = product.productName;
            if (AvailableItems.ContainsKey(productName))
            {
                AvailableItems[productName]++;
            }
            else
            {
                AvailableItems.Add(productName, 1);
            }
        }
    }

    public void DecreaseItem(string productName, int amount)
    {
        if (AvailableItems.ContainsKey(productName))
        {
            AvailableItems[productName] -= amount;
            if (AvailableItems[productName] <= 0)
            {
                AvailableItems.Remove(productName);
            }
        }
    }
}
