using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : InGameObject
{
    public override void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<InGameObject>() is not null)
        {
            if (other.GetComponent<InGameObject>().ObjectCategory != "Tile")
            {
                return;
            }
            numOfColliders += 1;
        }
    }
    public override void OnTriggerExit2D(Collider2D other)
    {
        if (other.GetComponent<InGameObject>() is not null)
        {

            if (other.GetComponent<InGameObject>().ObjectCategory != "Tile")
            {
                return;
            }
            numOfColliders -= 1;
        }
    }
}
