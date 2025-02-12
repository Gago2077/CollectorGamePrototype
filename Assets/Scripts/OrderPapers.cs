using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class OrderPapers : OrderSystem
{
    private TextMeshPro orderText;
    private void Start()
    {
        orderText = GetComponentInChildren<TextMeshPro>();
        foreach (var item in orderItems)
        {
            orderText.text += item.name + "\n";
        }
    }

}
