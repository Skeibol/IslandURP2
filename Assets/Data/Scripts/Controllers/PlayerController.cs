using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed;

    private Rigidbody2D _rigidBody;
    // Start is called before the first frame update
    void Start()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        handlePhysicsMovement();
    }

    void handlePhysicsMovement()
    {
        float hor = Input.GetAxisRaw("Horizontal");
        float vert = Input.GetAxisRaw("Vertical");
        
        Vector3 direction = new Vector3(hor, vert, 0f);
        direction.Normalize();
        _rigidBody.velocity = new Vector3(direction.x * speed, direction.y * speed, 0);


    }




}
