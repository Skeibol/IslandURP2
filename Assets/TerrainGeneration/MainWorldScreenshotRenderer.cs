using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class MainWorldScreenshotRenderer : NoiseMap
{
  public ContinentalnessMap continentalnessMap;
  public CarvingMap carvingMap;
  public BiomeMap biomeMap;
  public float errorMargin;


  public override void renderWorldScreenshot()
  {
    _outputTextureHeight = new Texture2D(worldSize, worldSize);
    _outputTextureHeight.filterMode = FilterMode.Point;

    matchDims(carvingMap);
    matchDims(continentalnessMap);
    matchDims(biomeMap);
    float[][] carving = carvingMap.getSamples(worldSize,offsetX,offsetY,seed);
    float[][] continentalness = continentalnessMap.getSamples(worldSize,offsetX,offsetY,seed);
    float[][] biomes = biomeMap.getSamples(worldSize,offsetX,offsetY,seed);
    float[][] output = new float[worldSize][];
    for (int i = 0; i < worldSize; i++) {
      output[i] = new float[worldSize];

    }

    Color[] colourMap = new Color[worldSize * worldSize];
    for (int y = 0; y < worldSize; y++) {
      for (int x = 0; x < worldSize; x++) {
        output[x][y]  = (carving[x][y]  + continentalness[x][y] ) / 2;


        if (output[x][y]  < 0f + errorMargin || output[x][y]  > 1f - errorMargin) {
          Debug.Log("SCALING ERROR");
        }

        //output[x, y] = (carving[x, y]);
        if (output[x][y]  > getBandByName("sand").threshold) {
          colourMap[y * worldSize + x] = biomeMap.setColorFromFloatBand((output[x][y]  + biomes[x][y] ) / 2);
        }
        else {
          if (thresholded) {
            colourMap[y * worldSize + x] = setColorFromFloatBand(output[x][y] );
          }
          else {
            colourMap[y * worldSize + x] = Color.Lerp(Color.black, Color.white, output[x][y] );
          }
        }
      }
    }


    _outputTextureHeight.SetPixels(colourMap);
    _outputTextureHeight.Apply();
    _Renderer.material.mainTexture = _outputTextureHeight;
    _Renderer.transform.localScale = new Vector3(worldSize, worldSize, 1);
  }


  private void matchDims(NoiseMap _otherMap)
  {
    _otherMap.worldSize = worldSize;
    _otherMap.offsetX = offsetX;
    _otherMap.offsetY = offsetY;
  }

  private void matchOtherMap(NoiseMap _otherMap)
  {
    _otherMap.scale = scale;
    _otherMap.octaves = octaves;
    _otherMap.lacunarity = lacunarity;
    _otherMap.persistence = persistence;
    _otherMap.worldSize = worldSize;
    _otherMap.offsetX = offsetX;
    _otherMap.offsetY = offsetY;
    _otherMap.thresholded = thresholded;
  }

  public PerlinDebugColor getBandByName(string name)
  {
    foreach (PerlinDebugColor band in bandColors) {
      if (band.name == name) {
        return band;
      }
    }

    return null;
  }
}