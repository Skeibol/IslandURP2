using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk : MonoBehaviour
{
  public Transform outworld;
  public Transform caves;

  public bool populated;

  // Start is called before the first frame update
  void Start()
  {
    populated = false;
  }

  public void EnableCaves()
  {
    outworld.gameObject.SetActive(false);
    caves.gameObject.SetActive(true);
  }


  public void EnableOutworld()
  {
    caves.gameObject.SetActive(false);
    outworld.gameObject.SetActive(true);
  }
  

  public bool setIsPopulated(bool isPopulated)
  {
    populated = isPopulated;

    return populated;
  }
}