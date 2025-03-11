using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CartInventory : MonoBehaviour
{
    private Dictionary<Transform, Transform> previousParents = new Dictionary<Transform, Transform>();
    public List<Transform> itemsInCart = new List<Transform>();
    public OrderSystem orderSystem;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Obtainable Item") || other.CompareTag("OrderItem"))
        {
            previousParents.Add(other.transform, other.transform.parent);
            other.transform.SetParent(transform);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Obtainable Item") || other.CompareTag("OrderItem"))
        {
            other.transform.SetParent(previousParents[other.transform]);
            previousParents.Remove(other.transform);
        }
    }
   
}
