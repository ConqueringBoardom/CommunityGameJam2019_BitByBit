using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player instance;

    private Color materialTintColor;
    private Material material;
    private Rigidbody2D rb;
    
    private void Awake()
    {
        instance = this;
        rb = GetComponent(typeof(Rigidbody2D)) as Rigidbody2D;
        material = transform.GetComponent<SpriteRenderer>().material;
        materialTintColor = new Color(1, 0, 0, 0);
    }

    private void FixedUpdate()
    {
        if (materialTintColor.a > 0)
        {
            float tintFadeSpeed = 6f;
            materialTintColor.a -= tintFadeSpeed * Time.deltaTime;
            material.SetColor("_Tint", materialTintColor);
        }
    }

    private void DamageFlash()
    {
        materialTintColor = new Color(1, 0, 0, 1f);
        material.SetColor("_Tint", materialTintColor);
    }

    public void DamageKnockback(Vector3 knockbackDir, float knockbackDistance, int damageAmount)
    {
        //transform.position += knockbackDir * knockbackDistance;
        rb.AddForce(knockbackDir, ForceMode2D.Impulse);
        DamageFlash();
        HeartsHealthVisual.heartsHealthSystemStatic.Damage(damageAmount);
    }

    public Vector3 GetPosition()
    {
        return transform.position;
    }
}
