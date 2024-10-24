using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentSlotContainer : UISlotContainer
{

  public override void HandleCursorClick(int childID,bool isEquipmentSlot)
  {
    if (_cursor.isItemOnCursor) {
      if (!_cursor.itemOnCursor.GetComponent<ItemInSlot>().InventoryObject.canBeEquipped) {
        return;
      }

      if (childID != 0 && _cursor.itemOnCursor.GetComponent<ItemInSlot>().InventoryObject.isWeapon) {
        return;
      }
    }
    
    base.HandleCursorClick(childID,true);
    setPlayerStats();
  }

  public void setPlayerStats()
  {
    PlayerStats.Instance.SetStatsToDefault();
    foreach (InventorySlot equipmentSlot in InventorySlots) {
      if (equipmentSlot.ItemInSlot is null) {
        continue;
      }
      foreach (string itemStatName in equipmentSlot.ItemInSlot.itemStats.Keys) {
        PlayerStats.Instance.ChangeStat(itemStatName, equipmentSlot.ItemInSlot.itemStats[itemStatName]);
      }
    }
  }
}