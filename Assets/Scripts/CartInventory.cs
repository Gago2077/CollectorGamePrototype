using System.Collections.Generic;
using UnityEngine;

public class CartInventory : MonoBehaviour
{
    private Dictionary<Transform, Transform> previousParents = new Dictionary<Transform, Transform>();
    public Dictionary<string, int> itemsInCart = new Dictionary<string, int>();
    public OrderSystem orderSystem;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Obtainable Item") || other.CompareTag("OrderItem"))
        {
            previousParents.Add(other.transform, other.transform.parent);
            other.transform.SetParent(transform);

            ProductName product = other.GetComponent<ProductName>();
            if (product != null)
            {
                string productName = product.productName;
                if (itemsInCart.ContainsKey(productName))
                {
                    itemsInCart[productName]++;
                }
                else
                {
                    itemsInCart.Add(productName, 1);
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Obtainable Item") || other.CompareTag("OrderItem"))
        {
            other.transform.SetParent(previousParents[other.transform]);
            previousParents.Remove(other.transform);

            ProductName product = other.GetComponent<ProductName>();
            if (product != null)
            {
                string productName = product.productName;
                if (itemsInCart.ContainsKey(productName))
                {
                    itemsInCart[productName]--;
                    if (itemsInCart[productName] <= 0)
                    {
                        itemsInCart.Remove(productName);
                    }
                }
            }
        }
    }
}