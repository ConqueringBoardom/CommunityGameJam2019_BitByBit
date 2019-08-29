using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerMovement1 : MonoBehaviour
{

    public float maxAcceleration = 40f;
    public float terminalVelocity = 10f;
    public float jumpPower = 7.0f;

    private Transform _transform;
    private Rigidbody2D _rigidbody;

    private bool grounded = false;
    private float idealDrag = 0;
    private Vector3 launchPad;  
        
    // Start is called before the first frame update
    void Start()
    {
        _transform = GetComponent(typeof(Transform)) as Transform;
        _rigidbody = GetComponent(typeof(Rigidbody2D)) as Rigidbody2D;

        idealDrag = (maxAcceleration / terminalVelocity);
        _rigidbody.drag = idealDrag / (idealDrag * Time.fixedDeltaTime + 1 );

    }

    // Update is called once per frame
    void Update()
    {
        MovePlayer();
        MaybeJump();
    }

    void Jump()
    {
        _rigidbody.AddForce(new Vector3(0, jumpPower, 0) , ForceMode2D.Impulse);
    }

    void MaybeJump()
    {
        if (Input.GetButtonDown("Jump"))
        {
            Debug.Log(string.Format("{0}, {1}", Input.GetButtonDown("Jump"), grounded));

            if (_rigidbody.velocity.y == 0)
            {
                Jump();
            }
            else if (grounded && _rigidbody.velocity.y <= 0 && _rigidbody.velocity.magnitude < jumpPower)
            {
                Jump();
            }
        }

    }


    void MovePlayer()
    {
        float translate = Input.GetAxis("Horizontal") * maxAcceleration * Time.deltaTime;

        // _rigidbody.velocity = new Vector3((_rigidbody.velocity.x + translate)/2, _rigidbody.velocity.y, 0);

        _rigidbody.AddForce(new Vector3(translate, 0, 0), ForceMode2D.Impulse);

    }

    void OnCollisionEnter2D(Collision2D collision)
    {
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
