using TMPro;
using UnityEngine;

public class SpawnableObject : MonoBehaviour
{
  public TextMeshPro _tmpText;
  public InventoryObject inventoryObject;
  public int health;
  public int maxHealth;
  public SpriteRenderer _spriteRenderer;
  public Inventory _inventory;

  // Start is called before the first frame update
  public virtual void Start()
  {
    maxHealth = health;
    _spriteRenderer = GetComponent<SpriteRenderer>();
    _inventory = GameObject.Find("Player").GetComponent<Inventory>();
  }

  // Update is called once per frame
  public virtual void Update()
  {
    if (health < 1) {
      _inventory.addToInventory(inventoryObject);
      Destroy(this.gameObject);
    }

    if (health < maxHealth) {
      _tmpText.text = health.ToString();
    }
    else {
      _tmpText.text = "";
    }
  }

  public virtual void takeDamage(int damage)
  {
    health -= damage;
  }
}