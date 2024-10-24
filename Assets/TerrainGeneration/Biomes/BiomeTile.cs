using System;
using UnityEngine;
using UnityEngine.Tilemaps;
[Serializable]
public class BiomeTile
{
    public string Name;
    public RuleTile Tile;
    public Tilemap tilemap;
    public float lowerBoundHeight;
    public float upperBoundHeight;
}
