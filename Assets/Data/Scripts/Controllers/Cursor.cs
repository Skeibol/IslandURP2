using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Cursor : MonoBehaviour
{
    RaycastHit hit;
    public bool isOverUI = false;
    public bool isItemOnCursor = false;
    public GameObject _prefabOnCursor = null;
    public GameObject itemOnCursor = null;
    public float placementCooldown;
    private float timeElapsed = -1f;
    private bool isItemOverShit = false;
    [SerializeField] private Camera _camera;

    // Start is called before the first frame update
    void Start()
    {
        placementCooldown = 0.1f;
    }
    void FixedUpdate()
    {
        if (isItemOnCursor)
        {

            isItemOverShit = itemOnCursor.GetComponent<InGameObject>().isOnObject;
        }

    }
    void Update()
    {
        var _mousePos = Input.mousePosition;
        _mousePos.z = 0;
        transform.position = _camera.ScreenToWorldPoint(_mousePos);

        if (isItemOnCursor && itemOnCursor is not null)
        {
            handleCursorItemMove(itemOnCursor);
        }
        if (Input.GetMouseButton(0) && isItemOnCursor && !isOverUI && !isItemOverShit && Time.time > timeElapsed)
        {
            placeItem();
            timeElapsed = Time.time + placementCooldown;
        }

        if (Input.GetKeyDown(KeyCode.Q)){
            
            if(itemOnCursor is not null){
                itemOnCursor.SetActive(false);
            }
            destroyItemUnderCursor();
            if(itemOnCursor is not null){
                itemOnCursor.SetActive(true);
            }
        }
        if (Input.GetMouseButtonDown(1) && GameController.EDIT_MODE_ON)
        {
            removeItem();
        }
        else if (GameController.EDIT_MODE_ON == false)
        {
            removeItem();
        }
    }
    public void pickupItem(GameObject _item)
    {
        if (itemOnCursor is not null)
        {
            Destroy(itemOnCursor);
        }
        isItemOnCursor = true;
        itemOnCursor = _item;

    }

    public void removeItem()
    {
        if (itemOnCursor is not null)
        {
            Destroy(itemOnCursor);
        }
        _prefabOnCursor = null;
        isItemOnCursor = false;
        itemOnCursor = null;
    }

    public void placeItem()
    {
        if (itemOnCursor is null)
        {
            return;
        }
        var prevItem = itemOnCursor;
        itemOnCursor = Instantiate(_prefabOnCursor,GameObject.Find("World").transform);
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


    public void setPrefabHeldInCursor(GameObject _prefab)
    {
        if (isItemOnCursor)
        {
            Destroy(itemOnCursor);
        }
        _prefabOnCursor = _prefab;
        var _item = Instantiate(_prefabOnCursor,GameObject.Find("World").transform);
        pickupItem(_item);
    }

    public void destroyItemUnderCursor()
    {
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

        if (hit.collider != null)
        {
            Destroy(hit.collider.gameObject);
        }

    }


}
