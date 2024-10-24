using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minimap : MonoBehaviour
{
  public Transform PlayerTransform;

  public NoiseManager NoiseManager;

  public Renderer MinimapRenderer;

  public List<PerlinDebugColor> bands;

  public int minimapSize;

  private int frameCounter;

  // Start is called before the first frame update
  void Start()
  {
  }

  // Update is called once per frame
  void FixedUpdate()
  {
    if (frameCounter == 50) {
      RenderMinimap();
      frameCounter = 0;
    }

    frameCounter += 1;
  }

  public void RenderMinimap()
  {
    Texture2D minimapTex = new Texture2D(minimapSize, minimapSize);
    minimapTex.filterMode = FilterMode.Point;
    Color[] colorArray = new Color[minimapSize * minimapSize];
    float[][] heights =
      NoiseManager.calculateBiomes(minimapSize, PlayerTransform.position.x, PlayerTransform.position.y);
    for (int y = 0; y < minimapSize; y++) {
      for (int x = 0; x < minimapSize; x++) {
        colorArray[y * minimapSize + x] = getColorFromBand(heights[x][y]);
      }
    }

    minimapTex.SetPixels(colorArray);
    minimapTex.Apply();
    MinimapRenderer.material.mainTexture = minimapTex;
  }

  public Color getColorFromFloat(float value)
  {
    return new Color(value, value, value, 1f);
  }

  public Color getColorFromBand(float value)
  {
    foreach (PerlinDebugColor band in bands) {
      if (value < band.threshold) {
        return band.color;
      }
    }

    return new Color(255, 105, 180);
  }
}