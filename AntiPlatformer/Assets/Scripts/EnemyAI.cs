using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyAI : MonoBehaviour
{

    public Transform target;

    public float speed = 200f;
    public float jumpPower = 20f;
    public float nextWaypointDistance = 3f;
    private bool grounded = false;
    private Vector3 launchPad;
    public bool flying = false;
    private Vector2 force;
    private bool invincibility = false;

    public Transform enemyGfx;
    private SpriteRenderer sr;

    Path path;
    int currentWaypoint = 0;
    bool reachedEndOfPath = false;

    Seeker seeker;
    Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        sr = this.enemyGfx.GetComponent<SpriteRenderer>();

        InvokeRepeating("UpdatePath", 0f, .5f);
    }

    void UpdatePath()
    {
        if (seeker.IsDone())
            seeker.StartPath(rb.position, target.position, OnPathComplete);
    }

    void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }

    void Jump()
    {
        rb.AddForce(new Vector3(0, jumpPower, 0), ForceMode2D.Impulse);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (path == null)
            return;

        if (currentWaypoint >= path.vectorPath.Count)
        {
            reachedEndOfPath = true;
            return;
        }
        else
            reachedEndOfPath = false;

        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
        if (flying)
        {
            if (rb.gravityScale == 0)
            {
                force = direction * speed * Time.deltaTime;
            }
            else
            {
                Debug.Log("Gravity Needs To Be Set to 0 for Flying Enemies");
            }
        }
        else
        {
            Vector2 xDirection = new Vector2(direction.x, 0f);
            force = xDirection * speed * Time.deltaTime;
            if (path.vectorPath[currentWaypoint].y > rb.position.y + 2.5 && grounded)
            {
                Jump();
            }
        }

        rb.AddForce(force);

        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);

        if (distance < nextWaypointDistance)
            currentWaypoint++;

        // Flip Enemy Based on Target Direction
        if (rb.velocity.x >= 0.01f)
            enemyGfx.localScale = new Vector3(-1f, 1f, 1f);
        else if (rb.velocity.x <= -0.01f)
            enemyGfx.localScale = new Vector3(1f, 1f, 1f);
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
    }

    private void Update()
    {
        StartCoroutine(InvincibilityTimeout());
    }

    private IEnumerator InvincibilityTimeout()
    {
        if (invincibility)
        {
            yield return new WaitForSeconds(0.5f);
            SetInvincibilityState(false);
        }
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
}
