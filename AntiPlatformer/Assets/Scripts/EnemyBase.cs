using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    private SpriteRenderer sr;
    private Rigidbody2D rb;
    public Transform enemyGfx;
    private bool invincibility = false;
    public float jumpPower = 7.0f;
    private bool grounded = false;
    private Vector3 launchPad;
    private bool m_FacingRight = true;

    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Flip Enemy Based on Target Direction
        if ((rb.velocity.x > 0 && !m_FacingRight) || (rb.velocity.x < 0 && m_FacingRight))
            Flip();
    }

    public void Jump()
    {
        Debug.Log("Enemy Jumped");
        rb.AddForce(new Vector3(0, jumpPower, 0), ForceMode2D.Impulse);
        grounded = false;
    }

    public float GetBounceVelocity(Rigidbody2D rb, float minimum, float multiplier)
    {
        if (Mathf.Abs(rb.velocity.y) < 9f) // If velocity is too small set a minimum bounce
            return Mathf.Abs(rb.velocity.y) + minimum;
        else // Otherwise bounce 1.8 times your velocity
            return Mathf.Abs(rb.velocity.y) * multiplier;
    }

    private IEnumerator Flash()
    {
        sr.color = new Color(sr.color.r, sr.color.b, sr.color.g, 0f);
        yield return new WaitForSeconds(0.2f);
        sr.color = new Color(sr.color.r, sr.color.b, sr.color.g, 1f);
    }

    public void DamageFlash()
    {
        StartCoroutine(Flash());
    }

    public bool GetInvincibilityState()
    {
        return invincibility;
    }

    public void SetInvincibilityState(bool state)
    {
        invincibility = state;
        StartCoroutine(InvincibilityTimeout());
    }

    private IEnumerator InvincibilityTimeout()
    {
        yield return new WaitForSeconds(0.5f);
        SetInvincibilityState(false);
    }

    public bool GetGroundedState()
    {
        return grounded;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        launchPad = collision.GetContact(0).normal;
        grounded = true;
    }

    void OnCollisionExit2D()
    {
        launchPad = new Vector3(0, 0, 0);
        grounded = false;
    }

    private void Flip()
    {
        // Switch the way the player is labelled as facing.
        m_FacingRight = !m_FacingRight;

        // Multiply the player's x local scale by -1.
        transform.Rotate(0f, 180f, 0f);
    }
}
