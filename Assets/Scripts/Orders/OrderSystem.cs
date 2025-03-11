using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class OrderSystem : MonoBehaviour
{
    public TextMeshPro orderText;
    public List<KeyValuePair<string, int>> currentOrder = new List<KeyValuePair<string, int>>();
    public CartInventory cartInventory;

    private void Start()
    {
        orderText = GetComponentInChildren<TextMeshPro>();
        if (orderText == null)
        {
            Debug.LogError("TextMeshPro component not found in OrderManager.");
            return;
        }
        cartInventory = FindObjectOfType<CartInventory>();
        GenerateNewOrder();
    }

    private void StrikeThroughFoundItems()
    {
        if (cartInventory == null || currentOrder == null || currentOrder.Count == 0)
            return;

        string newOrderText = "New Order:\n";

        foreach (var item in currentOrder)
        {
            string line = $"{item.Key} x{item.Value}";
            if (cartInventory.itemsInCart.TryGetValue(item.Key, out int cartQuantity) && cartQuantity == item.Value)
            {
                line = $"<s>{line}</s>";
            }
            newOrderText += line + "\n";
        }

        orderText.text = newOrderText;
    }

    private void Update()
    {
        StrikeThroughFoundItems();
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

        int numItems = Random.Range(1, Mathf.Min(4, availableProducts.Count + 1));
        List<string> orderProducts = new List<string>();
        Dictionary<string, int> orderQuantities = new Dictionary<string, int>();

        for (int i = 0; i < numItems; i++)
        {
            if (availableProducts.Count == 0) break;

            string productName = availableProducts[Random.Range(0, availableProducts.Count)];
            int maxQuantity = AvailableItemsManager.Instance.AvailableItems[productName];

            if (maxQuantity == 0)
            {
                availableProducts.Remove(productName);
                continue;
            }

            int quantity = Random.Range(1, Mathf.Min(3, maxQuantity + 1));

            if (orderQuantities.ContainsKey(productName))
            {
                orderQuantities[productName] += quantity;
            }
            else
            {
                orderProducts.Add(productName);
                orderQuantities.Add(productName, quantity);
            }

            if (orderQuantities[productName] >= maxQuantity)
            {
                availableProducts.Remove(productName);
            }
        }

        foreach (var product in orderProducts)
        {
            int quantity = orderQuantities[product];
            AvailableItemsManager.Instance.DecreaseItem(product, quantity);
            currentOrder.Add(new KeyValuePair<string, int>(product, quantity));
        }

        // Initial text generation without underlines
        StrikeThroughFoundItems();
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