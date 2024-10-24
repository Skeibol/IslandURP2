using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Cursor : MonoBehaviour
{
  RaycastHit hit;
  public bool isOverUI = false;
  public bool isItemOnCursor = false;
  public GameObject _prefabOnCursor = null;
  public GameObject itemOnCursor = null;
  public GameObject selectedTileSprite;
  public GameObject player = null;
  public List<Sprite> cursors;
  public int pickupAOE;
  private Animator _playerAnimator;
  private SpriteRenderer _spriteRenderer;
  private PlayerController _playerController;
  private GameObject itemGhost;
  private InventoryObject itemOnCursorInventory;
  public Inventory _inventory;
  [SerializeField] private Camera _camera;
  public List<Tilemap> harvestableTilemaps;

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
    selectedTileSprite.transform.position = getClampedGridPosition();
    var _mousePos = _camera.ScreenToWorldPoint(Input.mousePosition);
    _mousePos.z = 0;
    transform.position = _mousePos;
    changeCursorSprite();
    if (isItemOnCursor) {
      if (itemOnCursorInventory.isPlaceable && !_inventory.isInventoryOpen) {
        if (itemGhost is null) {
          pickupGhost(itemOnCursorInventory.worldGameObject);
        }
        else {
          handleCursorItemMoveClamped(itemGhost);
        }
      }
      else {
        if (itemGhost is not null) {
          removeGhost();
        }
        else {
          handleCursorItemMove(itemOnCursor);
        }
      }
    }
  }

  public void pickupGhost(GameObject _ghost)
  {
    if (itemOnCursor is not null) {
      Destroy(itemOnCursor);
    }

    itemGhost = Instantiate(_ghost, transform.position, Quaternion.identity, transform);
    itemGhost.GetComponent<InGameItem>().isOnCursor = true;
    itemGhost.GetComponent<BoxCollider2D>().isTrigger = true;
  }

  public void removeGhost()
  {
    var ghostInventory = itemGhost.GetComponent<InGameItem>().inventoryObject;
    _inventory.addToInventory(ghostInventory, isUIaction: false);
    removeItem();
    Destroy(itemGhost);
    itemGhost = null;
  }


  public void pickupItem(GameObject _item)
  {
    if (itemOnCursor is not null) {
      Destroy(itemOnCursor);
    }

    isItemOnCursor = true;
    itemOnCursor = _item;
    itemOnCursorInventory = itemOnCursor.GetComponent<ItemInSlot>().InventoryObject;
  }

  public void removeItem()
  {
    if (itemOnCursor is not null) {
      Destroy(itemOnCursor);
    }

    _prefabOnCursor = null;
    isItemOnCursor = false;
    itemOnCursor = null;
    itemOnCursorInventory = null;
  }


  public void handleCursorItemPlacement()
  {
    if (itemGhost == null) {
      return;
    }

    if (itemGhost.GetComponent<InGameItem>().isOverObject) {
      return;
    }

    itemGhost.transform.SetParent(GameObject.Find("Objects").transform);
    itemGhost.GetComponent<InGameItem>().isOnCursor = false;
    itemGhost.GetComponent<BoxCollider2D>().isTrigger = false;
    itemGhost = null;
    removeItem();
  }

  public void handleCursorItemMoveClamped(GameObject _itemToMove)
  {
    _itemToMove.transform.position = getClampedGridPosition();
  }

  public void handleCursorItemMove(GameObject _itemToMove)
  {
    _itemToMove.transform.position = cursorWorldPosition();
  }


  public Vector3Int getClampedGridPosition()
  {
    return new Vector3Int((int)Mathf.Ceil(cursorWorldPosition().x) - 1, (int)Mathf.Ceil(cursorWorldPosition().y) - 1, 0);
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


  public void hitItemUnderCursor(int damage)
  {
    RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

    if (hit.collider != null) {
      if (hit.collider.GetComponent<InGameItem>() is not null) {
        var _item = hit.collider.GetComponent<InGameItem>();
        if (_item.interactable) {
          _item.Interact();
        }
        else {
          _item.debugCursorDestroy();
        }
      }
    }
  }

  public void changeCursorSprite()
  {
    if (!isCursorInAOE()) {
      _spriteRenderer.color = new Color(120, 0, 0);
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

 

  public Tilemap getTileUnderCursor()
  {
    foreach (Tilemap tilemap in harvestableTilemaps) {
      if (!tilemap.transform.parent.GetComponent<Grid>().enabled) {
        continue;
      }
      var pos = tilemap.layoutGrid.WorldToCell(cursorWorldPosition());
      if (tilemap.GetTile(pos) is not null) {
        return tilemap;
      }
      
    }

    return null;
  }
}