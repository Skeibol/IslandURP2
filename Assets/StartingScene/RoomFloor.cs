using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomFloor : MonoBehaviour
{
  public WallFade _wallFade;

  // Start is called before the first frame update
  void Start()
  {
  }

  // Update is called once per frame
  private void OnTriggerEnter2D(Collider2D other)
  {
    if (other.name == "Player") {
      _wallFade.playerInRoom = true;
    }
  }

  private void OnTriggerExit2D(Collider2D other)
  {
    if (other.name == "Player") {
      _wallFade.playerInRoom = false;
    }
  }
}