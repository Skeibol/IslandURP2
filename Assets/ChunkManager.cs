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
  public List<Vector3> generatedChunkPositions;
  public Vector3 playerChunkPos;
  public GameObject currentChunk;
  public WorldGenerator _worldGenerator;

  // Start is called before the first frame update
  void Start()
  {
    if (_worldGenerator.renderMode) {
      return;
    }
    playerChunkPos = getClampedCameraPosition();
    currentChunk = generateChunk(playerChunkPos);
  }

  // Update is called once per frame
  void Update()
  {
    if (worldGenerator.renderMode) {
      return;
    }
    handleChunks9x9();
  }


  private void handleChunks9x9()
  {
    Vector3 currentChunkMiddle = currentChunk.transform.position;
    Vector3 playerPos = getCameraGroundPos();
    List<Vector3> neighbors = getNeighbourChunks(currentChunkMiddle);
  
    foreach (Vector3 _neighbor in neighbors) {
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
        if (dist < 0.51f) {
          currentChunk = _chunk;
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

  private GameObject generateChunk(Vector3 chunkPosition)
  {
    var _chunk = Instantiate(chunk, chunkPosition, quaternion.identity, transform);

    worldGenerator.PopulateChunk((int)chunkPosition.x, (int)chunkPosition.y, _chunk.transform);
    chunks.Add(_chunk);
    generatedChunkPositions.Add(chunkPosition);

    return _chunk;
  }

  

  private Vector3 getCameraGroundPos()
  {
    return new Vector3(_cam.transform.position.x, _cam.transform.position.y, 0);
  }
  


  private Vector3 getClampedCameraPosition()
  {
    return new Vector3(Mathf.Round(_cam.transform.position.x),
      Mathf.Round(_cam.transform.position.x), 0);
  }

  


  private List<Vector3> getNeighbourChunks(Vector3 startingChunk)
  {
    var startingX = startingChunk.x;
    var startingY = startingChunk.y;
    var worldSize = worldGenerator.worldSize;

    List<Vector3> neighbors = new List<Vector3>();
    var topLeft = new Vector3(startingX - worldSize, startingY + worldSize, 0);
    var top = new Vector3(startingX, startingY + worldSize, 0);
    var topRight = new Vector3(startingX + worldSize, startingY + worldSize, 0);
    var bottomLeft = new Vector3(startingX - worldSize, startingY - worldSize, 0);
    var bottom = new Vector3(startingX, startingY - worldSize, 0);
    var bottomRight = new Vector3(startingX + worldSize, startingY - worldSize, 0);
    var right = new Vector3(startingX + worldSize, startingY, 0);
    var left = new Vector3(startingX - worldSize, startingY, 0);

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
}