using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class NoiseMap : MonoBehaviour
{
  public bool isRendering;
  public Renderer _Renderer;
  public Texture2D _outputTextureHeight;
  public float scale;
  public int octaves;
  public float lacunarity;
  public float persistence;
  public int seed;
  public int worldSize;
  public float offsetX;
  public float offsetY;
  public bool thresholded;
  public float alpha;
  [SerializeField] public List<PerlinDebugColor> bandColors;


  public virtual void Start()
  {
    _Renderer = GetComponent<Renderer>();
  }

  public virtual void Update()
  {
    if (isRendering) {
      renderWorldScreenshot();
    }
  }

  public virtual void renderWorldScreenshot()
  {
    _outputTextureHeight = new Texture2D(worldSize, worldSize);
    _outputTextureHeight.filterMode = FilterMode.Point;

    float[][] noiseMap = getSamples(worldSize, offsetX, offsetY,seed);

    Color[] colourMap = new Color[worldSize * worldSize];
    for (int y = 0; y < worldSize; y++) {
      for (int x = 0; x < worldSize; x++) {
        if (thresholded) {
          colourMap[y * worldSize + x] = setColorFromFloatBand(noiseMap[x][y]);
        }
        else {
          colourMap[y * worldSize + x] = Color.Lerp(Color.black, Color.white, noiseMap[x][y]);
        }
      }
    }

    _outputTextureHeight.SetPixels(colourMap);
    _outputTextureHeight.Apply();
    _Renderer.material.mainTexture = _outputTextureHeight;
  }

  public virtual void setMainTexture()
  {
    _outputTextureHeight.Apply();
    _Renderer.material.mainTexture = _outputTextureHeight;
  }

  public virtual Color setColorFromFloat(float flt)
  {
    return new Color(flt, flt, flt, alpha);
  }


  public virtual float[][] getSamples(int size, float startX, float startY,int _seed)
  {
    return Noise.GenerateNoiseMap((int)size, (int)size, _seed, scale, octaves, persistence,
      lacunarity, new Vector2(startX, startY));
  }

  public virtual Color setColorFromFloatBand(float flt)
  {
    foreach (PerlinDebugColor color in bandColors) {
      if (flt < color.threshold) {
        var clr = color.color;
        clr.a = alpha;
        return clr;
      }
    }

    return new Color(255, 105, 180, alpha);
  }
}