using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderSystem : MonoBehaviour
{
    public List<GameObject> itemsInScene;
    protected List<GameObject> orderItems;
    private void Awake()
    {
        itemsInScene = new List<GameObject>();
        orderItems = new List<GameObject>();
        itemsInScene.AddRange(GameObject.FindGameObjectsWithTag("Obtainable Item"));
        foreach (var item in itemsInScene)
        {
            if (Random.Range(0, 2) > 0)
            {
                orderItems.Add(item);
            }
        }
    }
    private void Update()
    {
        
    }
}
