using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeepEntrance : MonoBehaviour
{
  public CaveEntrance CaveEntrance;
  public GameObject CaveInterior;
  public GameObject CaveExterior;


  private void OnTriggerExit2D(Collider2D other)
  {
    if (other.name == "Player") {
      CaveExterior.SetActive(false);
      CaveInterior.SetActive(true);
      CaveEntrance.isPlayerInCave = true;
    }
  }
}