using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

public class WorldGenerator : MonoBehaviour
{
  public int worldSize;
  public float scale;
  public int seed;
  public int octaves;
  public float persistence;
  public float lacunarity;
  [Range(0.0f, 1.0f)] public float seaThreshold;
  [Range(0.0f, 1.0f)] public float sandThreshold;
  [Range(0.0f, 1.0f)] public float treeThreshold;

  public Tilemap _tilemap;
  public RuleTile _seaTile;
  public Tile _sandTile;
  public Tile _deepSeaTile;
  public Tile _groundTile;
  public Tile _grassTile;

  public List<ObjectToSpawn> ots; 

  public Color seaColor;
  public Color treeColor;
  public Color groundColor;
  public Color cliffColor;
  public Color sandColor;
  public Camera _cam;
  public Renderer _renderer;
  public bool renderMode;
  private Texture2D _outputTexture;
  private Vector3 clampedGridPosition;
  private int _generatedChunks = 0;


  private void Start()
  {
    Random.InitState(123);
    if (renderMode) {
      _renderer.enabled = true;
    }
  }

  private void Update()
  {
    if (renderMode) {
      renderWorldScreenshot();
    }
  }


  public void PopulateChunk(int chunkX, int chunkY, Transform worldParent) // FPS - 120
  {
    var bottomLeft = new Vector3(chunkX - (worldSize / 2), chunkY - (worldSize / 2),
      0);

    Vector3Int[] tilePositions = new Vector3Int[worldSize * worldSize];
    TileBase[] tileArray = new TileBase[worldSize * worldSize];
    Vector3Int[] spawnArray = new Vector3Int[worldSize * worldSize];
    float[] sampleArray = new float[worldSize * worldSize];
    int[] gameObjectArray = new int[worldSize * worldSize];
    Vector3Int spawn = new Vector3Int(0, 0, 0);


    // GENERATE TILES - SAVE SAMPLES - PREPARE FOR GAMEOBJECTS
    for (int y = 0; y < worldSize; y++) {
      for (int x = 0; x < worldSize; x++) {
        // get perlin sample
        var sample = HeightMapGenerator.GetSample(x + worldParent.position.x, y + worldParent.position.y, scale,
          octaves, persistence, lacunarity, seed);

        spawn = new Vector3Int((int)bottomLeft.x + x, (int)bottomLeft.y + y, 0);
        spawnArray[x * worldSize + y] = spawn;
        sampleArray[x * worldSize + y] = sample;
        tilePositions[x * worldSize + y] = spawn;

        if (sample < seaThreshold) {
          if (sample < seaThreshold - 0.2f) {
            tileArray[x * worldSize + y] = _deepSeaTile;
          }
          else {
            tileArray[x * worldSize + y] = _seaTile;
          }
        }
        else if (sample < sandThreshold) {
          tileArray[x * worldSize + y] = _sandTile;
        }
        else {
          tileArray[x * worldSize + y] = _grassTile;
        }
      }
    }

    // READ SAMPLES - GENERATE GAMEO BJECT MATRIX


    for (int y = 0; y < worldSize; y++) {
      for (int x = 0; x < worldSize; x++) {
        var _sample = sampleArray[x * worldSize + y];
        if (gameObjectArray[x * worldSize + y] != 0) {
          continue;
        }

        if (_sample > seaThreshold) {
          if (_sample > sandThreshold) {
            var _rnd = Random.Range(0, 6);
            if (_rnd == 3 && _generatedChunks % 7 == 0) {
              fillArrayByObjectDimensions(getIDByName("giant rock"), x, y, 6, 5, gameObjectArray);
            }
            else if (_rnd < 2) {
              fillArrayByObjectDimensions(getIDByName("tree"), x, y, 1, 1, gameObjectArray);
            }
            else if (_rnd > 4) {
              fillArrayByObjectDimensions(getIDByName("rock"), x, y, 1, 1, gameObjectArray);
            }
          }
        }
      }
    }

    // INSTANTIATE GAME OBJECTS
    for (int y = 0; y < worldSize; y++) {
      for (int x = 0; x < worldSize; x++) {
        {
          var _objectId = gameObjectArray[x * worldSize + y];
          var _spawn = spawnArray[x * worldSize + y];

          if (_objectId <= 0 || _objectId == 99) {
            continue;
          }

          var objToSpwn = getPrefabByID(_objectId);
          if (objToSpwn != null) {
            Instantiate(getPrefabByID(_objectId), _spawn, quaternion.identity, worldParent);
          }
        }
      }


      _tilemap.SetTiles(tilePositions, tileArray);
      _generatedChunks += 1;
    }
  }


  private int getIDByName(string _name)
  {
    try {
      return ots.First(o => o.Name == _name).ID;
    }
    catch (Exception e) {
      Debug.Log(e);
      return -1;
    }
  }

  private GameObject getPrefabByID(int id)
  {
    try {
      return ots.First(o => o.ID == id).GameObjectPrefab;
    }
    catch (Exception e) {
      Debug.Log(e);
      return null;
    }
  }

  private void fillArrayByObjectDimensions(int objectID, int startX, int startY, int dimX, int dimY, int[] objArray)
  {
    if (startX + dimX > worldSize || startY + dimY > worldSize) {
      return;
    }

    var objX = startX;
    var objY = startY;
    var objectPos = objX * worldSize + objY;

    for (int i = 0; i < dimX; i++) {
      for (int j = 0; j < dimY; j++) {
        objArray[(startX + i) * worldSize + (startY + j)] = 99;
      }
    }

    objArray[objectPos] = objectID;
  }

  private void renderWorldScreenshot()
  {
    _outputTexture = new Texture2D(worldSize, worldSize);
    var bottomLeft = new Vector3(0 - (worldSize / 2), 0 - (worldSize / 2),
      0);
    Vector2 camPos = _cam.transform.position;

    _outputTexture.filterMode = FilterMode.Point;
    for (int y = 0; y < worldSize; y++) {
      for (int x = 0; x < worldSize; x++) {
        // get perlin sample

        var sample =
          HeightMapGenerator.GetSample(x + camPos.x, y + camPos.y, scale, octaves, persistence, lacunarity, seed);
        // REIMPLEMENT

        var treeSample = TreeMapGenerator.GetSample(x + camPos.x, y + camPos.y);
        Vector3 spawn = new Vector3(bottomLeft.x + x, bottomLeft.y + y, 0);
        //generate world
        if (sample < seaThreshold) {
          _outputTexture.SetPixel(x, y, seaColor);
        }
        else if (sample < sandThreshold) {
          if (sample < sandThreshold + 0.001f) {
            _outputTexture.SetPixel(x, y, sandColor);
          }

          _outputTexture.SetPixel(x, y, cliffColor);
        }
        else {
          if (treeSample > treeThreshold) {
            _outputTexture.SetPixel(x, y, treeColor);
          }
          else {
            _outputTexture.SetPixel(x, y, groundColor);
          }
        }
      }
    }

    setMainTexture(_outputTexture);
  }

  private void setMainTexture(Texture2D _texture)
  {
    _outputTexture = _texture;
    _outputTexture.Apply();
    _renderer.material.mainTexture = _outputTexture;
  }

  private Color setColorFromFloat(float flt)
  {
    return new Color(flt, flt, flt);
  }
}