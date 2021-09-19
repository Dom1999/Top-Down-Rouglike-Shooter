using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Equipment Object", menuName = "Inventory System/Items/Equipment")]

public class EquipmentObject : ItemObject
{
    public int weaponID;
    public int damage;
    public int ammo;
    public float rateOfFire;


    private void Awake()
    {
        this.type = ItemType.Equipment;
    }
}
