using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spikes : MonoBehaviour
{
    [SerializeField] private int damageAmount;
    private void OnTriggerEnter2D(Collider2D collider)
    {
        Player player = collider.GetComponent<Player>();
        if (player != null)
        {
            // We hit the player
            Vector3 knockbackDir = (player.GetPosition() - transform.position).normalized;
            player.DamageKnockback(knockbackDir, 15f, damageAmount);
        }
    }
}
