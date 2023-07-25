using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField]
    Item[] contents;
    void Start()
    {
        foreach(var item in contents)
        {
            Debug.Log(item.GetName());
        }
    }

    // Update is called once per frame
   
}
