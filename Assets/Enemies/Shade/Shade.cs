using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;

public class Shade : MonoBehaviour
{
  public void Start()
  {
    var plyr = GameObject.Find("Player").transform;
    GetComponent<AIDestinationSetter>().target = plyr;
  }

  public void Update()
  {
  }
}