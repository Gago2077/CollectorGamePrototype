using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetOrder : MonoBehaviour
{
    public Transform orderSpawnPoint;
    public GameObject orderPrefab;

    private void Update()
    {
        CreateOrder();
    }
    private void CreateOrder()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            Instantiate(orderPrefab, orderSpawnPoint.position, orderSpawnPoint.rotation);
        }
    }
}
