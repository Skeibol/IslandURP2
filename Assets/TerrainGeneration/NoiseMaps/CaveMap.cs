using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaveMap : NoiseMap
{
  public override float[][]  getSamples(int size, float startX, float startY, int _seed)
  {
    
    var map = Noise.GenerateNoiseMap(size, size, _seed, scale, octaves, persistence,
      lacunarity, new Vector2(startX, startY));
    for (int y = 0; y < size; y++) {
      for (int x = 0; x < size; x++) {
        map[x][y]  = Mathf.Abs((map[x][y]  * 2) - 1);
      }
    }

    return map;
  }
}