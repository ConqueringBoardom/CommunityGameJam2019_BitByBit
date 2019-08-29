using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class jumpModifier : MonoBehaviour
{
    public float fallMultiplyer = 2.5f;
    public float lowJumpMultiplyer = 2f;

    Rigidbody2D rb;

    void awaker(){
        rb = GetComponent<Rigidbody2D>();

    }

    void update(){
        if(rb.velocity.y < 0){
            rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplyer -1) * Time.deltaTime;
        }
        else if(rb.velocity.y > 0 && !Input.GetButtonDown("Jump") ){
            rb.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplyer -1) * Time.deltaTime;
        }

    }
}
