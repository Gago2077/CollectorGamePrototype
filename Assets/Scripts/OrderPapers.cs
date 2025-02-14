using TMPro;
using UnityEngine;

public class OrderPapers : OrderSystem
{
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
}