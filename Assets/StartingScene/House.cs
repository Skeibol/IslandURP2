using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class House : MonoBehaviour
{
  public List<Room> roomList;

  // Start is called before the first frame update
  void Start()
  {
  }

  // Update is called once per frame
  void Update()
  {
  }

  public void activateRoomByName(string Name)
  {
    if (getRoomByName(Name) is not null) {
      getRoomByName(Name).gameObject.SetActive(true);
    }
    else {
      Debug.Log("Room not found");
    }
  }

  private Room getRoomByName(string Name)
  {
    foreach (Room _room in roomList) {
      if (_room.RoomName == Name) {
        return _room;
      }
    }

    return null;
  }
}