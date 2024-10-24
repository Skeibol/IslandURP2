using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
  public InventoryObject ItemInSlot;
  public int ID;
  public int amount = 0;
  public UISlotContainer _inventorySlotContainer;

  // Start is called before the first frame update
  public virtual void Start()
  {
    
    _inventorySlotContainer = transform.parent.GetComponent<UISlotContainer>();
  }

  // Update is called once per frame
  public virtual void Update()
  {
  }

  public virtual void addAmount(int _amount)
  {
    amount += _amount;
    transform.GetChild(0).GetComponent<ItemInSlot>().amount = amount;
  }

  public virtual void removeAmount(int _amount)
  {
    amount -= _amount;
    if (amount == 0) {
      removeItemFromSlot();
    }
    else {
      transform.GetChild(0).GetComponent<ItemInSlot>().amount = amount;
    }
  }

  public virtual void addItemToSlot(InventoryObject _itemToAdd, GameObject inventoryPrefab, int _amount = 1)
  {
    ItemInSlot = _itemToAdd;
    amount += _amount;
    var icon = Instantiate(inventoryPrefab, transform).GetComponent<ItemInSlot>();
    
    icon.InventoryObject = _itemToAdd;
    icon.amount = amount;
  }
  public virtual void removeItemFromSlot()
  {
    Destroy(transform.GetChild(0).gameObject);
    ItemInSlot = null;
    amount = 0;
  }


  public virtual void onclick()
  {
    _inventorySlotContainer.HandleCursorClick(ID);

  }
}