using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class TreeFade : MonoBehaviour
{
  public SpriteRenderer _renderer;

  private BoxCollider2D boxCollider;

  // Start is called before the first frame update
  void Start()
  {
    _renderer = transform.parent.GetComponent<SpriteRenderer>();
    boxCollider = GetComponent<BoxCollider2D>();
  }

  private void OnTriggerEnter2D(Collider2D other)
  {
    // if (other.gameObject.CompareTag("player")) {
    //   StopAllCoroutines();
    //   StartCoroutine(startFadeOut());
    // }
  }

  private void OnTriggerExit2D(Collider2D other)
  {
    // if (other.gameObject.CompareTag("player")) {
    //   StopAllCoroutines();
    //   StartCoroutine(startFadeIn());
    // }
  }


  IEnumerator startFadeOut()
  {
    var fadeVal = _renderer.color.a;
    while (_renderer.color.a > 0.75f) {
      fadeVal -= 0.02f;
      _renderer.color = new Color(255, 255, 255, fadeVal);

      yield return null;
    }

    yield return 0;
  }

  IEnumerator startFadeIn()
  {
    var fadeVal = _renderer.color.a;
    while (_renderer.color.a < 1f) {
      fadeVal += 0.02f;
      _renderer.color = new Color(255, 255, 255, fadeVal);
      yield return null;
    }

    yield return 0;
  }
}