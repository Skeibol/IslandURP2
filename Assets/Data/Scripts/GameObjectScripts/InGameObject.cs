using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class InGameObject : MonoBehaviour
{
    public string Name;
    public string ObjectCategory;
    public string Description;
    public bool isOnObject = false;
    private BoxCollider2D _collider;
    public bool colliderInitialType;
    private SpriteRenderer _sprite;
    private Color _initSpriteColor;
    public int numOfColliders;



    // Start is called before the first frame update
    public virtual void Start()
    {
        numOfColliders = 0;
        initObjectName();
        _collider = GetComponent<BoxCollider2D>();
        _sprite = transform.Find(Name + "_sprite").GetComponent<SpriteRenderer>();
        _initSpriteColor = _sprite.color;
        colliderInitialType = _collider.isTrigger;

    }
    public virtual void FixedUpdate()
    {
        if (numOfColliders == 0)
        {
            isOnObject = false;
        }
        else
        {
            isOnObject = true;
        }
        changeColorOnOccupied();
        //Debug.Log($"item {Name} initial collider is {colliderInitialType}");
        if (colliderInitialType == false)
        {
            if (GameController.EDIT_MODE_ON)
            {
                _collider.isTrigger = true;
            }
            else
            {
                _collider.isTrigger = false;
            }
        }
    }

    public virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<InGameObject>() is not null)
        {
            if (other.GetComponent<InGameObject>().ObjectCategory == "Tile")
            {
                return;
            }
            numOfColliders += 1;
        }
    }
    public virtual void OnTriggerExit2D(Collider2D other)
    {
        if (other.GetComponent<InGameObject>() is not null)
        {
            if (other.GetComponent<InGameObject>().ObjectCategory == "Tile")
            {
                return;
            }
            numOfColliders -= 1;
        }
    }

    private void initObjectName()
    {
        var cleanName = this.transform.name.Replace("_object(Clone)", "");
        this.transform.name = cleanName;
        Name = cleanName;
    }


    public void changeColorOnOccupied()
    {
        if (isOnObject)
        {
            var rgb = Color.HSVToRGB(0, 1, 1);
            _sprite.color = new Color(rgb.r, rgb.g, rgb.b, 0.75f);
        }
        else
        {
            _sprite.color = _initSpriteColor;
        }
    }


}
