using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHandle : MonoBehaviour
{
  public Cursor _cursor;
  public PlayerController _playerController;
  public Transform target; //Assign to the object you want to rotate
  float angle;
  public float setup;
  private Weapon _weapon;
  private float startingRot;
  public Animator weaponArmAnimator;
  public Weapon Weapon;

  private void Awake()
  {
    _weapon = transform.GetChild(0).GetComponent<Weapon>();
  }

  void Update()
  {
    //rotation - very haxy change this


    Vector3 mousePos = Input.mousePosition;
    mousePos.z = 0f;

    Vector3 objectPos = Camera.main.WorldToScreenPoint(target.position);
    mousePos.x = mousePos.x - objectPos.x;
    mousePos.y = mousePos.y - objectPos.y;
    var xRot = 0f;
    var flippedPlayer = 1f;
    if (_playerController.facingRight) {
      xRot = 180f;
      flippedPlayer = -1f;
    }

    float angle = Mathf.Atan2(flippedPlayer * mousePos.y, mousePos.x) * Mathf.Rad2Deg;
    transform.rotation = Quaternion.Euler(new Vector3(xRot, 0, angle));

    // Vector3 mousePos = Input.mousePosition;
    // mousePos.z = -(transform.position.x - Camera.mainCamera.transform.position.x);
    //
    // Vector3 worldPos = Camera.mainCamera.ScreenToWorldPoint(mousePos);
    //
    // transform.LookAt(worldPos);
  }

  public void swingHand()
  {
    Weapon.Swing();
    weaponArmAnimator.SetTrigger("swing");
  }
}