using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seed : InGameItem
{
  public float growTimeInitial;
  private float growTime;
  public List<Sprite> growSprites;
  public int plantGrowthLevel;

  public override void Start()
  {
  
    plantGrowthLevel = 0;
    base.Start();
    growTime = growTimeInitial;
    grow();
  }

  public override void Update()
  {
    base.Update();
    if (!isOnCursor) {
      grow();
    }
  }

  public void grow()
  {
    if (growTime > 0) {
      growTime -= Time.deltaTime;
    }

    if (growTime <= 0) {
      plantGrowthLevel += 1;
      if (plantGrowthLevel == growSprites.Count) {
        plantGrowthLevel = 0;
      }

      _spriteRenderer.sprite = growSprites[plantGrowthLevel];
      growTime = growTimeInitial; // this is the interval between firing.
    }
  }
}