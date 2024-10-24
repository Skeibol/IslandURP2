using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class WallFade : MonoBehaviour
{
  private Tilemap _tilemap;

  public bool playerInRoom;

  // Start is called before the first frame update
  void Start()
  {
    _tilemap = GetComponent<Tilemap>();
  }

  void Update()
  {
    if (playerInRoom) {
      _tilemap.color = new Color(_tilemap.color.r, _tilemap.color.g, _tilemap.color.b, 0.2f);
      
    }
    else {
      _tilemap.color = new Color(_tilemap.color.r, _tilemap.color.g, _tilemap.color.b, 1f);
    }
  }
}