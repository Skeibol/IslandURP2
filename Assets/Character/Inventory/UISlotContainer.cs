using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISlotContainer : MonoBehaviour
{
  public List<InventorySlot> InventorySlots;
  public Cursor _cursor;
  public GameObject itemInSlotPrefab;
  public Transform cursorItemSlot;
  public Inventory Inventory;

  // Start is called before the first frame update
  public virtual void Start()
  {
    var c = 0;
    foreach (Transform child in transform) {
      var inventorySlot = child.GetComponent<InventorySlot>();
      inventorySlot.ID = c;
      InventorySlots.Add(inventorySlot);
      c += 1;
    }
  }


  // Update is called once per frame
  public virtual bool AddItemToSlots(InventoryObject _itemToAdd, int slotNumber = -1)
  {
    if (slotNumber < 0) {
      foreach (InventorySlot _inventorySlot in InventorySlots) {
        if (_inventorySlot.ItemInSlot == _itemToAdd && _inventorySlot.amount < _itemToAdd.stackSize) {
          _inventorySlot.addAmount(1);
          return true;
        }
      }

      foreach (InventorySlot _inventorySlot in InventorySlots) {
        if (_inventorySlot.ItemInSlot is null) {
          _inventorySlot.addItemToSlot(_itemToAdd, itemInSlotPrefab);
          return true;
        }
      }
    }
    else {
      if (InventorySlots[slotNumber].ItemInSlot == _itemToAdd &&
          InventorySlots[slotNumber].amount < _itemToAdd.stackSize) {
        InventorySlots[slotNumber].addAmount(1);
        return true;
      }
      else if (InventorySlots[slotNumber].ItemInSlot is null) {
        InventorySlots[slotNumber].addItemToSlot(_itemToAdd, itemInSlotPrefab);
        return true;
      }
    }

    return false;
  }

  public virtual void RemoveItemFromSlots(InventoryObject _itemToRemove, int slotNumber = -1)
  {
    if (slotNumber < 0) {
      foreach (InventorySlot _itemSlot in InventorySlots) {
        if (_itemSlot.ItemInSlot == _itemToRemove) {
          _itemSlot.removeAmount(1);
          break;
        }
      }
    }
    else {
      if (Inventory.inventory.Contains(_itemToRemove)) {
        InventorySlots[slotNumber].removeAmount(1);
      }
    }
  }


  public virtual bool addToPlayersInventory(InventoryObject _inventoryObjectToAdd, int amount = 1, int slotID = -1,bool isEquipmentSlot = false)
  {
    var success = false;
    for (int i = 0; i < amount; i++) {
      success = AddItemToSlots(_inventoryObjectToAdd, slotID);
      if (!success) {
        return success;
      }

      if (!isEquipmentSlot) {
        Inventory.addToInventory(_inventoryObjectToAdd, 1, slotID);
      }
    }

    return success;
  }

  public virtual void HandleCursorClick(int childID, bool isEquipmentSlot = false)
  {
    var clickedSlot = transform.GetChild(childID).GetComponent<InventorySlot>();


    if (_cursor.isItemOnCursor) {
      var cursorItem = _cursor.itemOnCursor.GetComponent<ItemInSlot>();
      if (clickedSlot.ItemInSlot is null) {
        // AKO JE NEŠ NA KURSORU A NIŠTA U SLOTU
        addToPlayersInventory(cursorItem.InventoryObject,
          cursorItem.amount, childID,isEquipmentSlot);
        _cursor.removeItem();
      }

      else {
        if (clickedSlot.ItemInSlot == cursorItem.InventoryObject) {
          // AKO KLIKNE NA ISTI - STACKING
          var amt = cursorItem.amount;
          for (int i = 0; i < amt; i++) {
            Debug.Log(i);
            var success = addToPlayersInventory(cursorItem.InventoryObject,
              1, childID);
            if (success) {
              cursorItem.amount -= 1;
              if (cursorItem.amount == 0) {
                _cursor.removeItem();
              }
            }
          }
        }
        else {
          // AKO KLIKNE S DRUGIM ITEMOM - SWAPPING
          var _clickedSlotSaved = clickedSlot.transform.GetChild(0).gameObject;
          clickedSlot.transform.GetChild(0).SetParent(cursorItemSlot);
          Inventory.removeFromInventory(clickedSlot.ItemInSlot, clickedSlot.amount);
          clickedSlot.ItemInSlot = null;
          clickedSlot.amount = 0;


          addToPlayersInventory(cursorItem.InventoryObject,
            cursorItem.amount, childID,isEquipmentSlot);
          _cursor.removeItem();
          _cursor.pickupItem(_clickedSlotSaved);
        }
      }
    }
    else {
      // AKO NEMA NIŠTA NA KURSORU STAVI NA KURSOR
      if (clickedSlot.ItemInSlot is not null) {
        _cursor.pickupItem(clickedSlot.transform.GetChild(0).gameObject);
        clickedSlot.transform.GetChild(0).SetParent(cursorItemSlot);
        Inventory.removeFromInventory(clickedSlot.ItemInSlot, clickedSlot.amount);
        clickedSlot.ItemInSlot = null;
        clickedSlot.amount = 0;
      }
    }
  }
}