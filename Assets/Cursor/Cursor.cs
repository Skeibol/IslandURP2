using System.Collections.Generic;
using UnityEngine;

public class Cursor : MonoBehaviour
{
  RaycastHit hit;
  public bool isOverUI = false;
  public bool isItemOnCursor = false;
  public GameObject _prefabOnCursor = null;
  public GameObject itemOnCursor = null;
  public GameObject player = null;
  public List<Sprite> cursors;
  public int pickupAOE;
  private Animator _playerAnimator;
  private SpriteRenderer _spriteRenderer;
  private PlayerController _playerController;
  [SerializeField] private Camera _camera;

  // Start is called before the first frame update
  void Start()
  {
    _playerAnimator = player.GetComponent<Animator>();
    _playerController = player.GetComponent<PlayerController>();
    _spriteRenderer = GetComponent<SpriteRenderer>();
    UnityEngine.Cursor.visible = false;
  }

  void FixedUpdate()
  {
  }

  void Update()
  {
    var _mousePos = _camera.ScreenToWorldPoint(Input.mousePosition);
    _mousePos.z = 0;
    transform.position = _mousePos;
    changeCursorSprite();
  }

  public void pickupItem(GameObject _item)
  {
    if (itemOnCursor is not null) {
      Destroy(itemOnCursor);
    }

    isItemOnCursor = true;
    itemOnCursor = _item;
  }

  public void removeItem()
  {
    if (itemOnCursor is not null) {
      Destroy(itemOnCursor);
    }

    _prefabOnCursor = null;
    isItemOnCursor = false;
    itemOnCursor = null;
  }

  public void placeItem()
  {
    if (itemOnCursor is null) {
      return;
    }

    var prevItem = itemOnCursor;
    itemOnCursor = Instantiate(_prefabOnCursor, GameObject.Find("World").transform);
    itemOnCursor.transform.position = getClampedGridPosition();
  }


  public void handleCursorItemMove(GameObject _itemToMove)
  {
    _itemToMove.transform.position = getClampedGridPosition();
    Debug.Log(_itemToMove.transform.position);
  }

  public bool isTileOccupied()
  {
    RaycastHit2D hit = Physics2D.Raycast(_camera.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
    return hit.collider != null;
  }

  public Vector3 getClampedGridPosition()
  {
    return new Vector3(Mathf.Ceil(cursorWorldPosition().x) - 1, Mathf.Ceil(cursorWorldPosition().y) - 1, 0);
  }

  public Vector3 cursorWorldPosition()
  {
    var _cursorPosWorld = _camera.ScreenToWorldPoint(Input.mousePosition);
    return new Vector3(_cursorPosWorld.x, _cursorPosWorld.y, 0);
  }

  public Vector3 cursorLocalPosition()
  {
    var _cursorPosLocal = Input.mousePosition;
    return new Vector3(_cursorPosLocal.x, _cursorPosLocal.y, 0);
  }

  public bool isCursorInAOE()
  {
    var _distance = Vector3.Distance(player.transform.position, cursorWorldPosition());
    return _distance < pickupAOE;
  }


  public void setPrefabHeldInCursor(GameObject _prefab)
  {
    if (isItemOnCursor) {
      Destroy(itemOnCursor);
    }

    _prefabOnCursor = _prefab;
    var _item = Instantiate(_prefabOnCursor, GameObject.Find("World").transform);
    pickupItem(_item);
  }

  public void hitItemUnderCursor()
  {
    RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

    if (hit.collider != null && hit.collider.name != "Player") {
      hit.collider.GetComponent<SpawnableObject>().takeDamage(10);
    }
  }

  public void changeCursorSprite()
  {

    if (!isCursorInAOE()) {
      _spriteRenderer.color = new Color(120,0,0);
    }
    else {
      _spriteRenderer.color = Color.white;
    }
    RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

    if (hit.collider != null && hit.collider.name != "Player") {
      _spriteRenderer.sprite = cursors[1];
    }
    else {
      _spriteRenderer.sprite = cursors[0];
      _spriteRenderer.color = Color.white;
    }
  }
}