using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerMovement : MonoBehaviour
{

    public CharacterController2D controller;

    public float speed = 200.0f;
    public bool grounded = false;
    public float jumpPower = 7.0f;
    public float fallMultiplyer = 2.5f;

    private Transform _transform;
    private Rigidbody2D _rigidbody;

    private Vector3 launchPad;  

    void Start()
    {
        _transform = GetComponent(typeof(Transform)) as Transform;
        _rigidbody = GetComponent(typeof(Rigidbody2D)) as Rigidbody2D;

    }

    void Update()
    {
        MovePlayer();
        MaybeJump();

        
        if(_rigidbody.velocity.y < 0){
            _rigidbody.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplyer -1) * Time.deltaTime;
        }
    }

    void MovePlayer()
    {
        float translate = Input.GetAxis("Horizontal") * speed * Time.deltaTime;
        // _transform.Translate(translate, 0, 0);
        _rigidbody.velocity = new Vector3((_rigidbody.velocity.x + translate)/2, _rigidbody.velocity.y, 0);

    }

    void Jump()
    {
        _rigidbody.AddForce(new Vector3(0, jumpPower, 0) - launchPad, ForceMode2D.Impulse);
    }

    void MaybeJump()
    {
        if (Input.GetButtonDown("Jump"))
        {
            Debug.Log(string.Format("{0}, {1}", Input.GetButtonDown("Jump"), grounded));

            if (_rigidbody.velocity.y < 0.01 && _rigidbody.velocity.y > -0.01)
            {
                Jump();
            }
            else if (grounded && _rigidbody.velocity.y <= 0 && _rigidbody.velocity.magnitude < jumpPower)
            {
                Jump();
            }
        }

    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Debug.Log("GROUND");
        // Debug.Log(collision.gameObject.transform.position);
        
        launchPad = collision.GetContact(0).normal;

        print(launchPad);


        grounded = true;
    }

    void OnCollisionExit2D()
    {
        launchPad = new Vector3(0, 0, 0);
        grounded = false;
    }

}
