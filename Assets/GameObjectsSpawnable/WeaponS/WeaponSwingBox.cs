using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSwingBox : MonoBehaviour
{
    private Weapon _weapon;
    // Start is called before the first frame update
    void Start()
    {
        _weapon = transform.parent.GetComponent<Weapon>();
    }

    // Update is called once per frame
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        _weapon.handleHit(other);
    }
}
