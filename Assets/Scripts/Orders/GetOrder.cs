using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetOrder : MonoBehaviour
{
    [SerializeField] private Transform _orderSpawnPoint;
    [SerializeField] private GameObject _orderPrefab;

    private void Update()
    {
        CreateOrder();
    }
    private void CreateOrder()
    {
        if (Input.GetKeyDown(KeyCode.G) && OrderManager.Instance.ActiveOrders.Count < OrderManager.Instance.MaxAvailableOrders )
        {
            OrderManager.Instance.ActiveOrders.Add(Instantiate(_orderPrefab, _orderSpawnPoint.position, _orderSpawnPoint.rotation).transform);
        }
    }
}
