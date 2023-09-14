using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="Unnamed Item",menuName ="Invertory Item")]
public class Item : ScriptableObject
{
    [SerializeField]
    string itemName;
    [SerializeField]
    string description;
    [SerializeField]
    float itemWeight;

    public string GetName()
    {
        return itemName;
    }
}
