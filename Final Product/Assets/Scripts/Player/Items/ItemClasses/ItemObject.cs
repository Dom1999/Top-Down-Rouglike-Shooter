using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    Equipment,
    Consumable,
    Default
}

public abstract class ItemObject : ScriptableObject
{
    //public GameObject prefab;
    public int id;
    public Sprite uiDisplay;
    
    public ItemType type;

    [TextArea(15, 20)]
    public string description;
}

[System.Serializable]
public class Item
{
    public string name;
    public bool canStack;
    public int id;

    public Item(ItemObject item)
    {
        name = item.name;
        id = item.id;
        canStack = true;

        if (id == 1)
        {
            canStack = false;
        }
    }
}
