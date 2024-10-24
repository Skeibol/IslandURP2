using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomSpawner : InGameItem
{
    public House _house;

    public override void Start()
    {
        _house = GameObject.Find("House").GetComponent<House>();
        base.Start();
    }
    public override void Interact()
    {
        _house.activateRoomByName("second");
    }
}
