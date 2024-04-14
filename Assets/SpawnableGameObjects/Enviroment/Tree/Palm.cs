using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Palm : SpawnableObject
{
  public List<Sprite> spriteList;
  public override void Start()
  {
    base.Start();
    var _rnd = Random.Range(0, spriteList.Count);
    _spriteRenderer.sprite = spriteList[_rnd];

  }
}
