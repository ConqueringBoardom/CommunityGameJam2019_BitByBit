using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerMovement : MonoBehaviour
{
    public CharacterController2D controller;

    // Update is called once per frame
    void Update()
    {
        // Input.GetAxisRaw("Horizontal");   
        Debug.Log(Input.GetAxisRaw("Horizontal"));


    }

    float horizontalMove = 0f;

    void FixedUpdate(){

        controller.Move(horizontalMove, false, false);
    }
}
