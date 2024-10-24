using System;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class InGameItem : MonoBehaviour
{
  public InventoryObject inventoryObject;
  public int health;
  public int maxHealth;
  public bool isOverObject;
  public bool isOnCursor;
  public bool interactable;
  public bool harvestable;
  public InventoryObject harvestableBy;
  public List<Sprite> spriteList;

  [HideInInspector] public Inventory _inventory;
  [HideInInspector] public SpriteRenderer _spriteRenderer;
  public bool isDead = false;

  // Start is called before the first frame update
  public virtual void Start()
  {
    maxHealth = health;
    _spriteRenderer = GetComponent<SpriteRenderer>();
    _inventory = GameObject.Find("Player").GetComponent<Inventory>();
    var _rnd = Random.Range(0, spriteList.Count);
    _spriteRenderer.sprite = spriteList[_rnd];
  }

  // Update is called once per frame
  public virtual void Update()
  {
    if (health < 1 && !isDead) {
      onHealthDepleted();
      isDead = true;
    }
  }

  public void OnDestroy()
  {
    StopAllCoroutines();
  }

  public virtual void onHealthDepleted()
  {
    if (harvestable) {
      if (!_inventory.addToInventory(inventoryObject, isUIaction: false)) {
        Instantiate(inventoryObject.groundObject, transform.position, quaternion.identity);
      }
    }
      
    Destroy(this.gameObject);
  }

  public virtual void debugCursorDestroy()
  {
    if (harvestable) {
      if (!_inventory.addToInventory(inventoryObject, isUIaction: false)) {
        Instantiate(inventoryObject.groundObject, transform.position, quaternion.identity);
      }
    }
      
    Destroy(this.gameObject);
  }

  public virtual bool takeDamage(int damage,bool isSourcePlayer = true)
  {
    health -= damage;
    if (health <= 0) {
      return false;
    }
    else {
      return true;
    }
  }

  public void OnTriggerStay2D(Collider2D other)
  {
    if (!isOnCursor) {
      return;
    }

    _spriteRenderer.color = Color.red;
    isOverObject = true;
  }

  public virtual void Interact()
  {
  }

  public void OnTriggerExit2D(Collider2D other)
  {
    if (!isOnCursor) {
      return;
    }

    _spriteRenderer.color = Color.white;
    isOverObject = false;
  }
}