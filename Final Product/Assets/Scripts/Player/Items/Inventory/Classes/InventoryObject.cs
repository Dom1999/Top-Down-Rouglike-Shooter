using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEditor;
using UnityEngine;


[CreateAssetMenu(fileName = "New Inventory", menuName = "Inventory System/Inventory")]
public class InventoryObject : ScriptableObject
{
    public ItemDatabaseObject database;
    public Inventory container;
    public string savePath = "";
    public void AddItem(Item item, int amount)
    {
        if (!item.canStack)
        {
            SetFirstEmptySlot(item, amount);
            return;
        }


        for (int i = 0; i < container.Items.Length; i++)
        {
            if (container.Items[i].ID == item.id)
            {
                container.Items[i].addAmount(amount);
                return;
            }
        }
        SetFirstEmptySlot(item, amount);

    }
    public InventorySlot SetFirstEmptySlot(Item item, int amount)
    {
        for (int i = 0; i < container.Items.Length; i++)
        {
            if (container.Items[i].ID <= -1)
            {
                container.Items[i].UpdateSlot(item.id, item, amount);
                return container.Items[i];
            }
        }
        return null;
    }

    public void MoveItem(InventorySlot item1, InventorySlot item2)
    {
        InventorySlot temp = new InventorySlot(item2.ID, item2.item, item2.amount);
        item2.UpdateSlot(item1.ID, item1.item, item1.amount);
        item1.UpdateSlot(temp.ID, temp.item, temp.amount);

    }

    public void RemoveItem(Item item)
    {
        for (int i = 0; i < container.Items.Length; i++)
        {
            if (container.Items[i].item == item)
            {
                container.Items[i].UpdateSlot(-1, null, 0);
            }
        }
    }

    [ContextMenu("Save")]
    public void Save()
    {

        IFormatter formatter = new BinaryFormatter();
        Stream stream = new FileStream(string.Concat(Application.persistentDataPath, savePath), FileMode.Create, FileAccess.Write);
        formatter.Serialize(stream, container);
        stream.Close();
    }

    [ContextMenu("Load")]
    public void Load()
    {
        if(File.Exists(string.Concat(Application.persistentDataPath, savePath)))
        {
            IFormatter formatter = new BinaryFormatter();
            Stream stream = new FileStream(string.Concat(Application.persistentDataPath, savePath), FileMode.Open, FileAccess.Read);
            Inventory newContainer = (Inventory)formatter.Deserialize(stream);
            for (int i = 0; i < container.Items.Length; i++)
            {
                container.Items[i].UpdateSlot(newContainer.Items[i].ID, newContainer.Items[i].item, newContainer.Items[i].amount);
            }
            stream.Close();

        }
    }

    [ContextMenu("Clear")]
    public void Clear()
    {
        container = new Inventory();
    }

}
[System.Serializable]
public class InventorySlot
{
    public int ID;
    public Item item;
    public int amount;

    public InventorySlot()
    {
        this.item = null;
        this.amount = 0;
        this.ID = -1;
    }

    public InventorySlot(int id, Item item, int amount)
    {
        this.item = item;
        this.amount = amount;
        this.ID = id;
    }
    public void UpdateSlot(int id, Item item, int amount)
    {
        this.item = item;
        this.amount = amount;
        this.ID = id;
    }

    public void addAmount(int value)
    {
        this.amount += value;
    }

    public void addAmount()
    {
        this.amount++;
    }
}

[System.Serializable]
public class Inventory
{
    public InventorySlot[] Items = new InventorySlot[24];
}
