using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundObject : MonoBehaviour
{
  public InventoryObject InventoryObject;

  // Start is called before the first frame update
  void Start()
  {
  }

  // Update is called once per frame
  void Update()
  {
  }

  private void OnTriggerEnter2D(Collider2D other)
  {
    if (other.name == "Player") {
      var inv = other.GetComponent<Inventory>();
      if (inv.isInventoryFull(InventoryObject, 1)) {
        inv.addToInventory(InventoryObject, isUIaction: false);
        Destroy(this.gameObject);
      }
    }
  }
}