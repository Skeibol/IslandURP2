using System;
using UnityEngine;
[CreateAssetMenu(menuName = "objects/spawnable")]
public class SpawnableObject : ScriptableObject
{
  public string Name;
  public int ID;
  public GameObject GameObjectPrefab;
  public Vector3Int dimensions;
  
}
