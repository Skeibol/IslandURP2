using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class TreeObject : InGameItem
{
  private Animator _animator;
  private TreeAudioManager audioManager;

  public override void Start()
  {
    base.Start();
    _animator = GetComponent<Animator>();
    audioManager = GetComponentInChildren<TreeAudioManager>();
  }

  public void HitAnimationPlay()
  {
    _animator.SetTrigger("hit");
  }

  public override bool takeDamage(int damage, bool isSourcePlayer = true)
  {
    health -= damage;
    if (isSourcePlayer) {
      audioManager.playHitSound();
      HitAnimationPlay();
    }

    if (health <= 0) {
      return false;
    }
    else {
      return true;
    }
  }

  public override void onHealthDepleted()
  {
    audioManager.playFallSound();
    _animator.SetTrigger("fall");
  }

  public void OnTreeFall()
  {
    Instantiate(inventoryObject.groundObject, transform.position, quaternion.identity);
    Destroy(this.gameObject);
  }
}