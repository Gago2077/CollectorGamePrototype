using System.Collections.Generic;
using UnityEngine;

public class CartInventory : MonoBehaviour
{
    private Dictionary<Transform, Transform> _previousParents = new Dictionary<Transform, Transform>();
    public Dictionary<string, int> ItemsInCart = new Dictionary<string, int>();
    [SerializeField] private OrderSystem _orderSystem;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Obtainable Item") || other.CompareTag("OrderItem"))
        {
            _previousParents.Add(other.transform, other.transform.parent);
            other.transform.SetParent(transform);

            ProductName product = other.GetComponent<ProductName>();
            if (product != null)
            {
                string productName = product.Name;
                if (ItemsInCart.ContainsKey(productName))
                {
                    ItemsInCart[productName]++;
                }
                else
                {
                    ItemsInCart.Add(productName, 1);
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Obtainable Item") || other.CompareTag("OrderItem"))
        {
            other.transform.SetParent(_previousParents[other.transform]);
            _previousParents.Remove(other.transform);

            ProductName product = other.GetComponent<ProductName>();
            if (product != null)
            {
                string productName = product.Name;
                if (ItemsInCart.ContainsKey(productName))
                {
                    ItemsInCart[productName]--;
                    if (ItemsInCart[productName] <= 0)
                    {
                        ItemsInCart.Remove(productName);
                    }
                }
            }
        }
    }
}