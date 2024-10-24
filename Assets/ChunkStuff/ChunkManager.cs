using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ChunkManager : MonoBehaviour
{
  public WorldGenerator worldGenerator;
  public Camera _cam;
  public GameObject chunk;
  public float chunkSpawnRange = 0.7f;
  public List<GameObject> chunks;
  public List<Vector3Int> generatedChunkPositions;
  public GameObject currentChunk;
  public GameObject Player;
  public GameObject startingChunk;
  public bool isPlayerAboveGround;

  public void Start()
  {
    if (isPlayerAboveGround) {
      enableOutworldChunks();
    }
    else {
      enableCaveChunks();
    }
  }

  // Update is called once per frame
  void Update()
  {
    if (Input.GetKeyDown(KeyCode.B)) {
      if (isPlayerAboveGround) {
        enableCaveChunks();
        isPlayerAboveGround = false;
      }
      else {
        enableOutworldChunks();
        isPlayerAboveGround = true;
      }
    }

    handleChunks9x9();
  }


  private void handleChunks9x9()
  {
    Vector3Int currentChunkMiddle =
      new Vector3Int((int)currentChunk.transform.position.x, (int)currentChunk.transform.position.y, 0);
    Vector3 playerPos = getCameraGroundPos();
    List<Vector3Int> neighbors = getNeighbourChunks(currentChunkMiddle);

    foreach (Vector3Int _neighbor in neighbors) {
      if (!generatedChunkPositions.Contains(_neighbor)) {
        generateChunk(_neighbor);
      }
    }

    foreach (GameObject _chunk in chunks) {
      if (_chunk == currentChunk) {
        continue;
      }

      var dist = Vector3.Distance(_chunk.transform.position, playerPos) /
                 (chunkSpawnRange * worldGenerator.worldSize);
      if (dist < 1.5f) {
        if (dist < 0.55f) {
          setCurrentChunk(_chunk);
        }

        if (!_chunk.activeSelf) {
          _chunk.SetActive(true);
        }
      }
      else {
        if (_chunk.activeSelf) {
          _chunk.SetActive(false);
        }
      }
    }
  }

  private GameObject generateChunk(Vector3Int chunkPosition)
  {
    var _chunk = Instantiate(chunk, chunkPosition, quaternion.identity, transform);
    Chunk chunkScript = _chunk.GetComponent<Chunk>();
    if (!isPlayerAboveGround) {
      chunkScript.EnableCaves();
    }

    worldGenerator.PopulateChunk(chunkPosition.x, chunkPosition.y, chunkScript.outworld, chunkScript.caves);
    chunks.Add(_chunk);
    generatedChunkPositions.Add(chunkPosition);

    return _chunk;
  }

  public GameObject generateStartingChunk(Vector3Int chunkPosition)
  {
    var _chunk = Instantiate(startingChunk, chunkPosition, quaternion.identity, transform);
    chunks.Add(_chunk);
    Chunk chunkScript = _chunk.GetComponent<Chunk>();
    if (!isPlayerAboveGround) {
      chunkScript.EnableCaves();
    }

    worldGenerator.PopulateChunk(chunkPosition.x, chunkPosition.y, chunkScript.outworld, chunkScript.caves);
    generatedChunkPositions.Add(chunkPosition);
    setCurrentChunk(_chunk);

    return _chunk;
  }

  public GameObject generateRandomChunk(Vector3Int chunkPosition)
  {
    Player.transform.position = chunkPosition;
    var _chunk = Instantiate(chunk, chunkPosition, quaternion.identity, transform);
    chunks.Add(_chunk);
    Chunk chunkScript = _chunk.GetComponent<Chunk>();
    worldGenerator.PopulateChunk(chunkPosition.x, chunkPosition.y, chunkScript.outworld, chunkScript.caves);
    generatedChunkPositions.Add(chunkPosition);
    setCurrentChunk(_chunk);

    return _chunk;
  }

  public void enableOutworldChunks()
  {
    worldGenerator.EnableOutworldTilemaps();
    foreach (GameObject _chunk in chunks) {
      _chunk.GetComponent<Chunk>().EnableOutworld();
    }
  }

  public void enableCaveChunks()
  {
    worldGenerator.EnableCaves();
    foreach (GameObject _chunk in chunks) {
      _chunk.GetComponent<Chunk>().EnableCaves();
    }
  }


  private Vector3 getCameraGroundPos()
  {
    return new Vector3(_cam.transform.position.x, _cam.transform.position.y, 0);
  }


  private Vector3Int getClampedCameraPosition()
  {
    return new Vector3Int(Mathf.RoundToInt(_cam.transform.position.x),
      Mathf.RoundToInt(_cam.transform.position.x), 0);
  }


  private List<Vector3Int> getNeighbourChunks(Vector3Int startingChunk)
  {
    var startingX = startingChunk.x;
    var startingY = startingChunk.y;
    var worldSize = worldGenerator.worldSize;

    List<Vector3Int> neighbors = new List<Vector3Int>();
    var topLeft = new Vector3Int(startingX - worldSize, startingY + worldSize, 0);
    var top = new Vector3Int(startingX, startingY + worldSize, 0);
    var topRight = new Vector3Int(startingX + worldSize, startingY + worldSize, 0);
    var bottomLeft = new Vector3Int(startingX - worldSize, startingY - worldSize, 0);
    var bottom = new Vector3Int(startingX, startingY - worldSize, 0);
    var bottomRight = new Vector3Int(startingX + worldSize, startingY - worldSize, 0);
    var right = new Vector3Int(startingX + worldSize, startingY, 0);
    var left = new Vector3Int(startingX - worldSize, startingY, 0);

    neighbors.Add(topLeft);
    neighbors.Add(top);
    neighbors.Add(topRight);
    neighbors.Add(bottomLeft);
    neighbors.Add(bottom);
    neighbors.Add(bottomRight);
    neighbors.Add(right);
    neighbors.Add(left);

    return neighbors;
  }

  private void setCurrentChunk(GameObject newChunk)
  {
    currentChunk = newChunk;
    AstarScript.Instance.centerPathfindingGraph(currentChunk.transform.position);
  }
}