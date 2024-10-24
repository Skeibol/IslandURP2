using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "objects/inventory")]
public class InventoryObject : ScriptableObject
{
  public string Name;
  public int ID;
  public Sprite inventorySprite;
  public GameObject worldGameObject;
  public int stackSize = 1;
  public bool requiresBench;
  public List<InventoryObject> requirements;
  public bool isPlaceable;
  public GameObject groundObject;
  public bool canBeEquipped;
  public bool isWeapon;
  public GameObject weaponObject;

  public GenericDictionary<string, int> itemStats = new GenericDictionary<string, int>()
    { { "attackDamage", 0 }, 
      { "harvestDamage", 0 },   
      { "speed", 0 } };


  [HideInInspector] public bool canBeCrafted;
}