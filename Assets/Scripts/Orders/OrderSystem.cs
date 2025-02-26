using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class OrderSystem : MonoBehaviour
{
    private TextMeshPro orderText;
    private Dictionary<string, int> currentOrder = new Dictionary<string, int>();

    private void Start()
    {
        orderText = GetComponentInChildren<TextMeshPro>();
        if (orderText == null)
        {
            Debug.LogError("TextMeshPro component not found in OrderManager.");
            return;
        }

        GenerateNewOrder();
    }

    public void GenerateNewOrder()
    {
        if (AvailableItemsManager.Instance == null) return;

        currentOrder.Clear();
        orderText.text = "New Order:\n";

        List<string> availableProducts = new List<string>(AvailableItemsManager.Instance.AvailableItems.Keys);

        if (availableProducts.Count == 0)
        {
            orderText.text += "No items available!";
            return;
        }

        int numItems = Random.Range(1, Mathf.Min(4, availableProducts.Count + 1)); // Random order size
        Dictionary<string, int> tempOrder = new Dictionary<string, int>(); // Temporary storage

        for (int i = 0; i < numItems; i++)
        {
            if (availableProducts.Count == 0) break; // Avoid errors if items run out

            string productName = availableProducts[Random.Range(0, availableProducts.Count)];

            // Get max possible quantity safely
            int maxQuantity = AvailableItemsManager.Instance.AvailableItems.ContainsKey(productName)
                ? AvailableItemsManager.Instance.AvailableItems[productName]
                : 0;

            if (maxQuantity == 0)
            {
                availableProducts.Remove(productName); // Remove unavailable items from selection
                continue;
            }

            int quantity = Random.Range(1, Mathf.Min(3, maxQuantity + 1));

            if (tempOrder.ContainsKey(productName))
            {
                tempOrder[productName] += quantity;
            }
            else
            {
                tempOrder.Add(productName, quantity);
            }

            if (tempOrder[productName] >= maxQuantity)
            {
                availableProducts.Remove(productName); // Ensure we don’t request more than available
            }
        }

        // Now safely update AvailableItemsManager AFTER determining all selections
        foreach (var item in tempOrder)
        {
            currentOrder[item.Key] = item.Value;
            AvailableItemsManager.Instance.DecreaseItem(item.Key, item.Value);
        }

        // Update UI
        foreach (var item in currentOrder)
        {
            orderText.text += $"{item.Key} x{item.Value}\n";
        }
    }


    public void CompleteOrder()
    {
        if (AvailableItemsManager.Instance == null) return;

        foreach (var item in currentOrder)
        {
            AvailableItemsManager.Instance.DecreaseItem(item.Key, item.Value);
        }

        GenerateNewOrder();
    }
}
