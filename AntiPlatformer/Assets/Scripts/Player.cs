using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private SpriteRenderer sr;
    private Rigidbody2D rb;
    private float bounceVelocity;
    private bool invincibility = false;
    
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
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

    public float GetBounceVelocity(Rigidbody2D rb, float minimum, float multiplier)
    {
        if (Mathf.Abs(rb.velocity.y) < 9f) // If velocity is too small set a minimum bounce
            return Mathf.Abs(rb.velocity.y) + minimum;
        else // Otherwise bounce 1.8 times your velocity
            return Mathf.Abs(rb.velocity.y) * multiplier;
    }
}
