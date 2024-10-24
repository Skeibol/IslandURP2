using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
  public GameObject swingBox;
  public InventoryObject _InventoryObject;
  public bool isAttacking;
  [HideInInspector] public BoxCollider2D _hitCollider;

  // Start is called before the first frame update
  public virtual void Start()
  {
    _hitCollider = GetComponentInChildren<BoxCollider2D>();
    swingBox.SetActive(false);
  }

  public virtual void handleHit(Collider2D other)
  {
    if (!other.isTrigger) {
      return;
    }

    if (other.gameObject.CompareTag("Enemy")) {
      other.GetComponent<Enemy>().recieveDamage(10);
      other.GetComponent<Rigidbody2D>().AddForce((other.transform.position - transform.position) * 100f);
    }
    else {
      CheckHitObjectType(other);
    }
  }

  public virtual void CheckHitObjectType(Collider2D other)
  {
    if (other.GetComponent<InGameItem>() is null) {
      return;
    }

    var inGameObject = other.GetComponent<InGameItem>();
    if (inGameObject.harvestable && inGameObject.harvestableBy == _InventoryObject) {
      inGameObject.takeDamage(PlayerStats.Instance.GetStat("harvestDamage"));
      _hitCollider.enabled = false; // so it doesnt hit more obđekts niqqa
      Debug.Log(
        $"{inGameObject.name} is harvestable : {inGameObject.harvestable}, harvestable by : {inGameObject.harvestableBy}");
    }
    else {
      Debug.Log(
        $"{inGameObject.name} is harvestable : {inGameObject.harvestable}, harvestable by : {inGameObject.harvestableBy}");
    }
  }

  public virtual void Swing()
  {
    _hitCollider.enabled = true;
    StartCoroutine(WeaponSwingHitbox());
  }


  public IEnumerator WeaponSwingHitbox()
  {
    yield return new WaitForSeconds(0.2f);
    swingBox.SetActive(true);
    StartCoroutine(WeaponSwingHitboxOff());
  }

  public IEnumerator WeaponSwingHitboxOff()
  {
    yield return new WaitForSeconds(0.3f);
    swingBox.SetActive(false);
  }
}