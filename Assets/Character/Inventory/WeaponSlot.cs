using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSlot : InventorySlot
{
  public Cursor _cursor;

  public override void onclick()
  {
    Debug.Log("overriding");
    if (_cursor.isItemOnCursor) {
      if (_cursor.itemOnCursor.GetComponent<ItemInSlot>().InventoryObject.isWeapon) {
        base.onclick();
      }
    }
    else {
      base.onclick();
    }
    
  }

  public void changeWeapon()
  {
    if (ItemInSlot is not null) {
      _inventorySlotContainer.Inventory.changeWeapon(ItemInSlot.weaponObject);
    }
    else {
      _inventorySlotContainer.Inventory.removeWeapon();
    }
  }
}