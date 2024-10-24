using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemInSlot : MonoBehaviour
{

  public Transform _parent;
  public InventorySlot _parentSlot;
  public InventoryObject InventoryObject;
  private TMP_Text _amountText;
  public int amount = 0;
  private Image _image;

  // Start is called before the first frame update
  void Start()
  {
    _parent = transform.parent;
    _parentSlot = _parent.GetComponent<InventorySlot>();
    _image = GetComponent<Image>();
    _amountText = transform.GetChild(0).GetComponent<TMP_Text>();
    _image.sprite = _parentSlot.ItemInSlot.inventorySprite;
  }

  // Update is called once per frame
  void Update()
  {
    _amountText.text = amount.ToString();
  }
  
  
}