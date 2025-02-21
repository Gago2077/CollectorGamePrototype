using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class OrderSystem : MonoBehaviour
{
    protected Dictionary<string, int> orderItems; // Changed key to string (product name)
    private TextMeshPro orderText;

    private void Start()
    {
        orderText = GetComponentInChildren<TextMeshPro>();
        if (orderText == null)
        {
            Debug.LogError("TextMeshPro component not found in OrderPapers.");
            return;
        }

        orderText.text = "Order List:\n";

        foreach (var item in orderItems)
        {
            // Directly use the product name (key) and quantity (value)
            orderText.text += $"{item.Key} x{item.Value}\n";
        }
    }
    private void Awake()
    {
        orderItems = new Dictionary<string, int>();
        var itemsInScene = GameObject.FindGameObjectsWithTag("Obtainable Item");

        foreach (var item in itemsInScene)
        {
            var product = item.GetComponent<ProductName>();
            if (product == null) continue; // Skip items without ProductName component

            if (Random.Range(0, 2) > 0)
            {
                string productName = product.productName;
                if (orderItems.ContainsKey(productName))
                {
                    orderItems[productName]++;
                }
                else
                {
                    orderItems.Add(productName, 1);
                }
            }
        }
    }
}
