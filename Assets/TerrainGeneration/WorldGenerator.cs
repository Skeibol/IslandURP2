using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

public class WorldGenerator : MonoBehaviour
{
  public GameObject Player;
  public ChunkManager ChunkManager;
  public NoiseManager NoiseManager;
  public int worldSize;
  public int seed;
  public int treeNoiseScale;
  public Tilemap sandTilemap;
  public TileBase sandTile;
  public Tilemap caveFloorTilemap;
  public TileBase caveFloorTile;
  [Range(0.0f, 1.0f)] public float spawnLowerThreshold;
  [Range(0.0f, 1.0f)] public float spawnUpperThreshold;
  public List<Biome> biomeList;

  private RuleTile _tile;
  private int _generatedChunks;
  private Vector3 clampedGridPosition;
  private TileBase[] sandTileArray;
  private TileBase[] caveFloorTileArray;

  private void Start()
  {
    sandTileArray = new TileBase[worldSize * worldSize];
    for (int i = 0; i < sandTileArray.Length; i++) {
      sandTileArray[i] = sandTile;
    }

    caveFloorTileArray = new TileBase[worldSize * worldSize];
    for (int i = 0; i < caveFloorTileArray.Length; i++) {
      caveFloorTileArray[i] = caveFloorTile;
    }

    Random.InitState(seed);
    SpawnPlayer();
  }

  public void Update()
  {
    if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.F)) {
      var rndVec = new Vector3Int(Random.Range(0, 10000), Random.Range(0, 100000), 0);
      ChunkManager.generateRandomChunk(rndVec);
    }
  }

  public void SpawnPlayer(int startx = 0, int starty = 0)
  {
    var spawn = NoiseManager.GetSuitableSpawnLocation(worldSize, startx, starty, spawnLowerThreshold,
      spawnUpperThreshold);
    Player.transform.position = spawn;
    ChunkManager.generateStartingChunk(spawn);
  }


  IEnumerator placeTilesAndGameObjects(float[][] biomes, float[][] caves, Vector3Int[] spawnArray,
    Transform outworldParent, Transform caveParent, bool spawnObjects = true)
  {
    int[] gameObjectArray = new int[worldSize * worldSize];
    var noiseScale = worldSize / treeNoiseScale;
    for (int y = 0; y < worldSize; y++) {
      for (int x = 0; x < worldSize; x++) {
        if (_generatedChunks == 0) {
          yield return 0;
        }


        var _spawn = spawnArray[y * worldSize + x];
        var _rnd = Mathf.PerlinNoise((float)_spawn.x / noiseScale, (float)_spawn.y / noiseScale);
        SpawnableObject obj = null;

        foreach (Biome biome in biomeList) {
          if (biomes[x][y] <= biome.upperBound && biomes[x][y] > biome.lowerBound) {
            biome.placeTileOutworld(_spawn, biomes[x][y]);
            biome.placeTileCaves(_spawn, caves[x][y]);
            if (gameObjectArray[y * worldSize + x] != 99 && biomes[x][y] > biome.treeTreshold && spawnObjects) {
              // Za sada nema spawnanja u more ali dodaj
              obj = biome.getBiomeObject(_rnd);
            }

            break;
          }
        }

        if (obj is not null) {
          fillArrayByObjectDimensions(obj.ID, x, y, obj.dimensions.x, obj.dimensions.y, gameObjectArray);
        }

        var _objectId = gameObjectArray[y * worldSize + x];

        if (_objectId > 0 && _objectId != 99) {
          var offsetSpawn = new Vector3(_spawn.x + Random.Range(-0.5f, 0.5f), _spawn.y + Random.Range(-0.5f, 0.5f), 0);
          Instantiate(obj.GameObjectPrefab, offsetSpawn, quaternion.identity, outworldParent);
        }
      }

      yield return null;
    }

    yield return 0;
  }

  public void PopulateChunk(int chunkX, int chunkY, Transform outworldParent, Transform cavesParent) // FPS - 120
  {
    float[][] caves = NoiseManager.calculateCaves(worldSize, outworldParent.position.x, outworldParent.position.y);
    float[][] biomes =
      NoiseManager.calculateBiomes(worldSize, outworldParent.position.x, outworldParent.position.y);
    var bottomLeft = new Vector3Int(chunkX - (worldSize / 2), chunkY - (worldSize / 2),
      0);
    Vector3Int[] spawnArray = new Vector3Int[worldSize * worldSize];


    // GENERATE SPAWN POSITIONS - PREPARE FOR GAMEOBJECTS
    for (int y = 0; y < worldSize; y++) {
      for (int x = 0; x < worldSize; x++) {
        spawnArray[y * worldSize + x] = new Vector3Int(bottomLeft.x + x, bottomLeft.y + y, 0);
      }
    }

    // Place objects slowly
    StartCoroutine(placeTilesAndGameObjects(biomes, caves, spawnArray, outworldParent, cavesParent,
      _generatedChunks != 0));


    // Place sand fast
    sandTilemap.SetTiles(spawnArray, sandTileArray);
    caveFloorTilemap.SetTiles(spawnArray, caveFloorTileArray);


    _generatedChunks += 1;
  }

  public float getHeight(float x, float y)
  {
    float[][] samples = NoiseManager.calculateHeights(worldSize, x, y);

    return samples[worldSize / 2][worldSize / 2];
  }


  private void fillArrayByObjectDimensions(int objectID, int startX, int startY, int dimX, int dimY, int[] objArray,
    bool centered = false)
  {
    // ARRAY SE ISPUNJAVA OD DOLE LIJEVO, PAZI NA SPRITE PIVOT POINT
    var halfX = (dimX - 1) / 2;
    var halfY = (dimY - 1) / 2;
    var objX = 0;
    var objY = 0;
    var objectPos = 0;


    // Not centered
    if (startX + dimX > worldSize || startY + dimY > worldSize) {
      return;
    }

    if (centered) {
      objX = startX;
      objY = startY + halfY;
    }
    else {
      objX = startX;
      objY = startY;
    }

    objectPos = objY * worldSize + objX;

    for (int y = 0; y < dimY; y++) {
      for (int x = 0; x < dimX; x++) {
        if (objArray[(startY + y) * worldSize + (startX + x)] == 99) {
          return; // check if has space
        }
      }
    }

    for (int y = 0; y < dimY; y++) {
      for (int x = 0; x < dimX; x++) {
        objArray[(startY + y) * worldSize + (startX + x)] = 99;
      }
    }

    objArray[objectPos] = objectID;
  }

  public void EnableOutworldTilemaps()
  {
    foreach (Biome biome in biomeList) {
      biome.EnableOutworldTilemaps();
    }
  }

  public void EnableCaves()
  {
    foreach (Biome biome in biomeList) {
      biome.EnableCaveTilemaps();
    }
  }
}