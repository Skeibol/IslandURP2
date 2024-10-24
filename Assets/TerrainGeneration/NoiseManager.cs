using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoiseManager : MonoBehaviour
{
  public ContinentalnessMap continentalnessMap;
  public CarvingMap carvingMap;
  public CaveMap caveMap;
  public BiomeMap biomeMap;
  public int seed;

  public float[][] biomes;
  public float[][] caves;

  public float[][] heights;


  // Start is called before the first frame update
  public float[][] calculateHeights(int worldSize, float startX, float startY)
  {
    float[][] carving = carvingMap.getSamples(worldSize, startX, startY, seed);
    float[][] continentalness = continentalnessMap.getSamples(worldSize, startX, startY, seed);
    heights = new float[worldSize][];
    for (int i = 0; i < worldSize; i++) {
      heights[i] = new float[worldSize];
    }

    for (int y = 0; y < worldSize; y++) {
      for (int x = 0; x < worldSize; x++) {
        heights[x][y] = (carving[x][y] + continentalness[x][y]) / 2f;
      }
    }

    return heights;
  }

  public float[][] calculateCaves(int worldSize, float startX, float startY)
  {
    caves = caveMap.getSamples(worldSize, startX, startY, seed);

    return caves;
  }


  public float[][] calculateBiomes(int worldSize, float startX, float startY)
  {
    heights = calculateHeights(worldSize, startX, startY);


    biomes = biomeMap.getSamples(worldSize, startX, startY, seed);


    for (int y = 0; y < worldSize; y++) {
      for (int x = 0; x < worldSize; x++) {
        biomes[x][y] = (biomes[x][y] + heights[x][y]) / 2;
      }
    }

    return biomes;
  }

  public Vector3Int GetSuitableSpawnLocation(int worldSize, int startX, int startY, float lowerThreshold,
    float upperThreshold)
  {
    calculateHeights(worldSize, startX, startY);
    calculateBiomes(worldSize, startX, startY);


    for (int y = 0; y < worldSize; y++) {
      for (int x = 0; x < worldSize; x++) {
        var sample = (biomes[x][y] + heights[x][y]) / 2;
        if (sample > upperThreshold || heights[x][y] < lowerThreshold) {
          Debug.Log($"finding location.. {startX} , {startY}");
          return GetSuitableSpawnLocation(worldSize, startX + worldSize, startY + worldSize, lowerThreshold,
            upperThreshold);
        }
      }
    }

    Debug.Log($"found location.! {startX} , {startY}");
    return new Vector3Int(startX, startY);
  }
}