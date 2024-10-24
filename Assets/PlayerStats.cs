using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerStats : Stats
{
  public static PlayerStats Instance { get; private set; }

  private void Awake()
  {
    // If there is an instance, and it's not me, delete myself.

    if (Instance != null && Instance != this) {
      Destroy(this);
    }
    else {
      Instance = this;
    }
  }

}