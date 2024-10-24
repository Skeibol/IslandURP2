using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;


public class Biome : MonoBehaviour
{
  public int ID;
  public string Name;
  public float lowerBound;
  public float upperBound;
  public List<BiomeTile> outworldTiles;
  public List<BiomeTile> caveTiles;
  public List<SpawnableObject> SpawnableObjects;
  public float treeTreshold;
  public List<Grid> outworldGrids;
  public List<Grid> caveGrids;
  

  public void EnableOutworldTilemaps()
  {
    foreach (Grid _outworldGrid in outworldGrids) {
      _outworldGrid.enabled = true;
    }

    foreach (Grid _caveGrid in caveGrids) {
      _caveGrid.enabled = false;
    }
  }

  public void EnableCaveTilemaps()
  {
    foreach (Grid _outworldGrid in outworldGrids) {
      _outworldGrid.enabled = false;
    }

    foreach (Grid _caveGrid in caveGrids) {
      _caveGrid.enabled = true;
    }
  }


  public void placeTileOutworld(Vector3Int tilePos, float heightValue)
  {
    foreach (BiomeTile tile in outworldTiles) {
      if (heightValue < tile.upperBoundHeight && heightValue > tile.lowerBoundHeight) {
        tile.tilemap.SetTile(tilePos, tile.Tile);
        return;
      }
    }
  }

  public void placeTileCaves(Vector3Int tilePos, float heightValue)
  {
    foreach (BiomeTile tile in caveTiles) {
      if (heightValue < tile.upperBoundHeight && heightValue > tile.lowerBoundHeight) {
        tile.tilemap.SetTile(tilePos, tile.Tile);
        return;
      }
    }
  }

  public SpawnableObject getBiomeObject(float rndValue)
  {
    var _rnd = Random.Range(0, 25);
    var obj = Random.Range(0, SpawnableObjects.Count);

    if (rndValue < 0.4) {
      if (_rnd < 5) {
        return SpawnableObjects[obj];
      }
    }
    else {
      if (_rnd < 1) {
        return SpawnableObjects[obj];
      }
    }

    return null;
  }

  private int getIDByName(string _name)
  {
    try {
      return SpawnableObjects.First(o => o.Name == _name).ID;
    }
    catch (Exception e) {
      Debug.Log(e);
      return -1;
    }
  }

  private GameObject getPrefabByID(int id)
  {
    try {
      return SpawnableObjects.First(o => o.ID == id).GameObjectPrefab;
    }
    catch (Exception e) {
      Debug.Log(e);
      return null;
    }
  }
}