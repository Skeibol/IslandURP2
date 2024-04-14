using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public RectTransform _inventoryUIgrid;
    public List<CraftableObject> craftableObjects;
    public List<InventoryObject> inventoryObjects;

    public Image _iconPrefab;
    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void addToInventory(InventoryObject _item)
    {
        _iconPrefab.sprite = _item.inventorySprite;
        Instantiate(_item.worldGameObject,_inventoryUIgrid);
        inventoryObjects.Add(_item);
    }

   
}
