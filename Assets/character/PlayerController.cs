using System;
using System.Collections;
using Unity.Mathematics;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
  public float speed;
  public float damageRecovery;
  public int bodyDirection;
  private Rigidbody2D _rigidBody;
  private Animator _animator;
  public float harvestCooldown;
  public Cursor _cursor;
  private float _lastHarvestTime;
  public bool isDamaged = false;
  public bool facingRight = false;
  public Inventory _inventory;
  private SpriteRenderer _spriteRenderer;
  public WeaponHandle WeaponHandle;


  public GameObject lamp;
  // Start is called before the first frame update
  void Start()
  {
    _lastHarvestTime = Time.time;
    _animator = GetComponent<Animator>();
    _spriteRenderer = GetComponent<SpriteRenderer>();
    _rigidBody = GetComponent<Rigidbody2D>();
    bodyDirection = 0;
  }

  // Update is called once per frame
  void Update()
  {
    if (Input.GetKeyDown(KeyCode.G)) {
      if (lamp.activeSelf) {
        lamp.SetActive(false);
      }
      else {
        lamp.SetActive(true);
      }
    }
    
    if (!_inventory.isInventoryOpen) {
      if (!_cursor.isItemOnCursor) {
        if (Input.GetMouseButton(0) && _lastHarvestTime < Time.time) {
          _rigidBody.velocity = Vector2.zero;
          //isAttacking = true;
          if (_inventory.isWeaponEquipped) {
            WeaponHandle.swingHand();
          }

          _lastHarvestTime = Time.time + harvestCooldown;
        }

        if (Input.GetMouseButtonDown(1)) {
          _cursor.hitItemUnderCursor(500);
        }
      }
      else {
        if (Input.GetMouseButtonDown(1)) {
          _cursor.handleCursorItemPlacement();
        }
      }


      if (!isDamaged) {
        handlePhysicsMovement();
      }
    }
    else {
      _rigidBody.velocity = Vector2.zero;
    }

    getBodyDirection();
    handleAnimations();
  }

  

  void handlePhysicsMovement()
  {
    float hor = Input.GetAxisRaw("Horizontal");
    float vert = Input.GetAxisRaw("Vertical");

    Vector3 direction = new Vector3(hor, vert, 0f);
    direction.Normalize();
    _rigidBody.velocity = new Vector3(direction.x * PlayerStats.Instance.GetStat("speed"),
      direction.y * PlayerStats.Instance.GetStat("speed"), 0);
  }

  void getBodyDirection()
  {
    var cursorX = _cursor.cursorLocalPosition().x;
    if (cursorX > Screen.width / 2f && facingRight) {
      transform.Rotate(0f, 180f, 0f);
      facingRight = false;
    }
    else if (cursorX < Screen.width / 2f && !facingRight) {
      transform.Rotate(0f, -180f, 0f);
      facingRight = true;
    }
  }

  void handleAnimations()
  {
    if (_rigidBody.velocity.x != 0 || _rigidBody.velocity.y != 0) {
      _animator.SetInteger("speedLevel", 1);
    }
    else if (_animator.GetInteger("speedLevel") != 0) {
      _animator.SetInteger("speedLevel", 0);
    }
  }

  private void OnCollisionEnter2D(Collision2D other)
  {
    if (other.gameObject.CompareTag("Enemy")) {
      StartCoroutine(TakingDamage());
      _rigidBody.AddForce((transform.position - other.transform.position) * 700f);
    }
  }

  public IEnumerator TakingDamage()
  {
    isDamaged = true;

    yield return new WaitForSeconds(damageRecovery);

    isDamaged = false;
  }
}