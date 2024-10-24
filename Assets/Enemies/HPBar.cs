using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPBar : MonoBehaviour
{
  public SpriteRenderer _sprriteRenderer;
  protected MaterialPropertyBlock _MaterialPropertyBlock;

  public void Awake()
  {
    _MaterialPropertyBlock = new MaterialPropertyBlock();
  }

  public void SetHPValue(float value)
  {
    _sprriteRenderer.GetPropertyBlock(_MaterialPropertyBlock);
    _MaterialPropertyBlock.SetFloat("_Offset", value);
    _sprriteRenderer.SetPropertyBlock(_MaterialPropertyBlock);
  }
}