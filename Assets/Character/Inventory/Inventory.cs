using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;


public class Inventory : MonoBehaviour
{
  public InventorySlotContainer _InventorySlotContainer;
  public RectTransform _quickInventoryUIgrid;
  public RectTransform _equipmentUIgrid;
  public RectTransform _craftingUIgrid;
  public CanvasGroup _hideUIcanvasGroup;
  public List<InventoryObject> craftableObjectsAll;
  public List<InventoryObject> inventory;

  public GameObject equippedWeapon = null;
  public GameObject _craftingIconPrefab;
  public GameObject weaponHolder;
  public bool isWeaponEquipped;
  public bool isInventoryOpen = false;
  public bool isCraftingBenchOpen = false;
  private Image _image;
  private Button _button;
  public WeaponHandle WeaponHandle;

  // Start is called before the first frame update
  void Start()
  {
    _image = _craftingIconPrefab.GetComponent<Image>();
    addToInventory(getCraftableItemByName("axe"), isUIaction: false);
    addToInventory(getCraftableItemByName("hoe"), isUIaction: false);

  }

  private void Update()
  {
    if (Input.GetKeyDown(KeyCode.E) && !isInventoryOpen) {
      OpenInventory();
    }
    else if (Input.GetKeyDown(KeyCode.E)) {
      CloseInventory();
    }
  }

  public void OpenInventory()
  {
    RefreshCraftingInventory();
    FadeCanvasGroup(_hideUIcanvasGroup, true, 0.3f);
    isInventoryOpen = true;
  }

  public void CloseInventory()
  {
    FadeCanvasGroup(_hideUIcanvasGroup, false, 0.3f);
    isInventoryOpen = false;
    isCraftingBenchOpen = false;
  }


  public bool addToInventory(InventoryObject _itemToAdd, int amount = 1, int slotNumber = -1, bool isUIaction = true)
  {
    //Debug.Log($"Adding {_itemToAdd} amount {amount} slotnubmert = {slotNumber}");

    for (int i = 0; i < amount; i++) {
      if (!isUIaction) {
        if (!_InventorySlotContainer.AddItemToSlots(_itemToAdd)) {
          Debug.Log("Inventory Full");
          return false;
        }
      }

      inventory.Add(_itemToAdd);
      RefreshCraftingInventory();
    }

    return true;
  }

  public void removeFromInventory(InventoryObject _itemToRemove, int amount = 1, int slotNumber = -1)
  {
    for (int i = 0; i < amount; i++) {
      inventory.Remove(_itemToRemove);
      RefreshCraftingInventory();
    }
  }


  public void RefreshCraftingInventory()
  {
    // DESTROY PREVIOUS SCENE
    foreach (Transform child in _craftingUIgrid) {
      Destroy(child.gameObject);
    }

    checkCraftableItems();


    foreach (InventoryObject _craftableObject in craftableObjectsAll) {
      if (!_craftableObject.canBeCrafted) {
        continue;
      }

      _image.sprite = _craftableObject.inventorySprite;
      var spawned = Instantiate(_craftingIconPrefab, _craftingUIgrid);

      spawned.GetComponent<Button>().onClick.AddListener(() => // CALLBACK
      {
        if (_InventorySlotContainer.checkFreeSlotsAfterCraft(_craftableObject.requirements, _craftableObject) ==
            false) {
          Debug.Log("inventory fool");
          return;
        }

        foreach (InventoryObject _requirement in _craftableObject.requirements) {
          inventory.Remove(_requirement);
          _InventorySlotContainer.RemoveItemFromSlots(_requirement);
        }

        addToInventory(_craftableObject, isUIaction: false);
      });
    }
  }

  public void checkCraftableItems()
  {
    foreach (InventoryObject _craftableObject in craftableObjectsAll) {
      if (_craftableObject.requiresBench && !isCraftingBenchOpen) {
        _craftableObject.canBeCrafted = false;
        continue;
      }
      
      int foundMaterials = 0;
      var tempInventory = new List<InventoryObject>(inventory);
      foreach (InventoryObject _requirement in _craftableObject.requirements) {
        
        if (tempInventory.Contains(_requirement)) {
          foundMaterials += 1;
          tempInventory.Remove(_requirement);
        }
      }

      if (foundMaterials == _craftableObject.requirements.Count) {
        _craftableObject.canBeCrafted = true;
        // Debug.Log(
        //   $"You can craft {_craftableObject.name} , required {_craftableObject.ObjectRequirements.Count} found {foundMaterials}");
      }
      else {
        _craftableObject.canBeCrafted = false;
        // Debug.Log(
        //   $"You cannot craft {_craftableObject.name} , required {_craftableObject.ObjectRequirements.Count} found {foundMaterials}");
      }
    }
  }

  public bool isInventoryFull(InventoryObject _itemToAdd, int amount)
  {
    return _InventorySlotContainer.isInventoryFull(_itemToAdd, amount);
  }

  public void changeWeapon(GameObject weapon)
  {
    if (equippedWeapon is not null) {
      Destroy(equippedWeapon);
    }
  
    equippedWeapon = Instantiate(weapon, weaponHolder.transform);
    WeaponHandle.Weapon = equippedWeapon.GetComponent<Weapon>();
    isWeaponEquipped = true;
  }

  public void removeWeapon()
  {
    if (equippedWeapon is not null) {
      isWeaponEquipped = false;
      WeaponHandle.Weapon = null;
      Destroy(equippedWeapon);
    }
  }

  public static void FadeCanvasGroup(CanvasGroup canvasGroup, bool active, float duration, float delay = 0,
    Action onCompleteAction = null)
  {
    if (onCompleteAction != null) {
      if (active) {
        canvasGroup.blocksRaycasts = true;
        canvasGroup.DOFade(1, duration).SetDelay(delay).OnComplete(() =>
        {
          canvasGroup.interactable = true;
          onCompleteAction();
        });
      }
      else {
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
        canvasGroup.DOFade(0, duration).SetDelay(delay).OnComplete(() => onCompleteAction());
      }
    }
    else {
      if (active) {
        canvasGroup.blocksRaycasts = true;
        canvasGroup.DOFade(1, duration).SetDelay(delay).OnComplete(() => { canvasGroup.interactable = true; });
      }
      else {
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
        canvasGroup.DOFade(0, duration).SetDelay(delay);
      }
    }
  }
  
  private InventoryObject getCraftableItemByName(string name)
  {
    try {
      return craftableObjectsAll.First(o => o.Name == name);
    }
    catch (Exception e) {
      Debug.Log(e);
      return null;
    }
  }
}