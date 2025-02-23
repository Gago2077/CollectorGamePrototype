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

        for (int i = 0; i < numItems; i++)
        {
            string productName = availableProducts[Random.Range(0, availableProducts.Count)];
            int quantity = Random.Range(1, Mathf.Min(3, AvailableItemsManager.Instance.AvailableItems[productName] + 1));

            if (currentOrder.ContainsKey(productName))
            {
                currentOrder[productName] += quantity;
                AvailableItemsManager.Instance.DecreaseItem(productName, quantity);

            }
            else
            {
                currentOrder.Add(productName, quantity);
                AvailableItemsManager.Instance.DecreaseItem(productName, quantity);

            }

        }

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
