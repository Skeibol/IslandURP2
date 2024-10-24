using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySlotContainer : UISlotContainer
{
  public bool checkFreeSlotsAfterCraft(List<InventoryObject> requirements, InventoryObject itemToCraft)
  {
    foreach (InventorySlot _slot in InventorySlots) {
      if (_slot.ItemInSlot is null) {
        return true;
      }
    }

    return false;
  }

  public void craftItem(List<InventoryObject> requirements, InventoryObject itemToCraft)
  {
  }

  public bool isInventoryFull(InventoryObject _itemToAdd, int amount)
  {
    foreach (InventorySlot _slot in InventorySlots) {
      if (_slot.ItemInSlot is null) {
        return true;
      }

      if (_slot.ItemInSlot == _itemToAdd) {
        if (_slot.amount + amount <= _itemToAdd.stackSize) {
          return true;
        }
      }
    }

    return false;
  }
}