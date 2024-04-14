using UnityEngine;

public class PlayerController : MonoBehaviour
{
  public float speed;
  public int bodyDirection;
  private Rigidbody2D _rigidBody;
  private Animator _animator;
  public float harvestCooldown;
  public Cursor _cursor;
  private float _lastHarvestTime;
  public bool isAttacking = false;

  private SpriteRenderer _spriteRenderer;

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
    if (Input.GetMouseButton(0) && _lastHarvestTime < Time.time && _cursor.isCursorInAOE()) {
      _rigidBody.velocity = Vector2.zero;
      isAttacking = true;
      _cursor.hitItemUnderCursor();
      _lastHarvestTime = Time.time + harvestCooldown;
    }
    else if(Input.GetMouseButtonUp(0)) {
      isAttacking = false;
    }
    else if(!isAttacking){
      handlePhysicsMovement();
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
    _rigidBody.velocity = new Vector3(direction.x * speed, direction.y * speed, 0);
  }

  void getBodyDirection()
  {
    if (Input.GetAxisRaw("Horizontal") > 0) {
      bodyDirection = 0;
      _spriteRenderer.flipX = false;
    }
    else if (Input.GetAxisRaw("Horizontal") < 0) {
      bodyDirection = 1;
      _spriteRenderer.flipX = true;
    }
  }

  void handleAnimations()
  {
    _animator.SetBool("isAttacking",isAttacking);

    if (_rigidBody.velocity.x != 0 || _rigidBody.velocity.y != 0) {
      _animator.SetInteger("speedLevel", 1);
    }
    else if (_animator.GetInteger("speedLevel") != 0) {
      _animator.SetInteger("speedLevel", 0);
    }
  }


}