using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingBench : InGameItem
{
  public override void Interact()
  {
    Debug.Log("interact");
    _inventory.OpenInventory();
    _inventory.isCraftingBenchOpen = true;
    _inventory.RefreshCraftingInventory();
  }
}
