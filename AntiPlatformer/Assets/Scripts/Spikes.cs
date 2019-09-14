using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spikes : MonoBehaviour
{
    [SerializeField] private int damageAmount = 1;
    private float bounceVelocity;
    private Player player;
    private EnemyAI enemy;
    private Rigidbody2D rb;
    private bool invincibility = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        rb = collision.GetComponent<Rigidbody2D>();
        switch (collision.tag) {
            case "Player":
                player = collision.GetComponent<Player>();
                invincibility = player.GetInvincibilityState();
                Debug.Log(invincibility);
                if (player != null && !invincibility)
                {
                    player.DamageFlash();
                    HeartsHealthVisual.heartsHealthSystemStatic.Damage(damageAmount);
                    bounceVelocity = player.GetBounceVelocity(rb, 9f, 1.8f);
                    rb.AddForce(new Vector2(0f, bounceVelocity), ForceMode2D.Impulse);
                    player.SetInvincibilityState(true);
                }
                break;
            case "Enemy":
                enemy = collision.GetComponent<EnemyAI>();
                invincibility = enemy.GetInvincibilityState();
                if (!invincibility)
                {
                    enemy.DamageFlash();
                    bounceVelocity = enemy.GetBounceVelocity(rb, 9f, 1.8f);
                    rb.AddForce(new Vector2(0f, bounceVelocity), ForceMode2D.Impulse);
                    enemy.SetInvincibilityState(true);
                }
                break;

        }
    }
}
