using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class Enemy : MonoBehaviour
{
  public Rigidbody2D _rigidbody2D;
  public Material HPBarMaterial;
  public GenericDictionary<string, float> stats;
  public GenericDictionary<InventoryObject, int> drops;
  public HPBar HPBar;
  public GameObject player;
  public float distance;
  public float speed;
  public bool isHit;
  public float health;
  public float hitRecovery;

  public virtual void Start()
  {
    player = GameObject.Find("Player");
    stats.TryGetValue("health", out health);
    stats.TryGetValue("hitRecovery", out hitRecovery);
    stats.TryGetValue("speed", out speed);
  }

  // Update is called once per frame
  public virtual void Update()
  {
    Vector2 dir = player.transform.position - transform.position;
    HPBar.SetHPValue(health / 50 - 0.5f);
    if (health <= 0) {
      StopAllCoroutines();
      foreach (InventoryObject _drop in drops.Keys) {
        for (int i = 0; i < drops[_drop]; i++) {
          var rndVector = new Vector3(transform.position.x + Random.Range(0f, 3f),
            transform.position.y + Random.Range(0f, 3f), 0);
          Instantiate(_drop.groundObject, rndVector, quaternion.identity);
        }
      }

      Destroy(this.gameObject);
    }

    if (!isHit) {
      if (dir.magnitude < 5f) {
        HandleEnemyAI();
      }
    }
  }

  public virtual void HandleEnemyAI()
  {
    distance = Vector2.Distance(transform.position, player.transform.position);
    transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
  }

  public virtual void recieveDamage(float damage)
  {
    if (!isHit) {
      health -= damage;
      StartCoroutine(StartHitRecovery());
    }
  }

  public virtual void OnTriggerEnter2D(Collider2D other)
  {
    Debug.Log(other.tag);
    if (other.CompareTag("weapon")) {
      recieveDamage(PlayerStats.Instance.GetStat("attackDamage"));
    }
  }

  public IEnumerator StartHitRecovery()
  {
    isHit = true;

    yield return new WaitForSeconds(hitRecovery);

    isHit = false;
  }
}