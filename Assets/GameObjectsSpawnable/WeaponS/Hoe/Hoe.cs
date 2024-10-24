using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Hoe : Weapon
{
  [HideInInspector] public Cursor Cursor;


  public override void Start()
  {
    base.Start();
    Cursor = GameObject.Find("Cursor").GetComponent<Cursor>();
  }
  
  public override void Swing()
  {
    
    if (!isAttacking) {
      _hitCollider.enabled = true;
      harvestTileUnderCursor();
    }
  }
  
  
  public void harvestTileUnderCursor()
  {
    var foundTileMap = Cursor.getTileUnderCursor();
    if (foundTileMap is not null) {
      var loc = foundTileMap.layoutGrid.WorldToCell(Cursor.cursorWorldPosition());
      foundTileMap.SetTile(loc, null);
      Debug.Log("tile removed");
    }
  }
}