using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class GroundEnemyAI : MonoBehaviour
{
    public float speed = 200f;
    public float nextWaypointDistance = 3f;
    private Vector2 force;
    private EnemyBase eb;
    public Transform target;
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
        eb = GetComponent<EnemyBase>();

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
        Vector2 xDirection = new Vector2(direction.x, 0f);
        force = xDirection * speed * Time.deltaTime;
        if (path.vectorPath[currentWaypoint].y > rb.position.y + 2.5 && eb.GetGroundedState())
        {
            eb.Jump();
        }

        rb.AddForce(force);

        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);

        if (distance < nextWaypointDistance)
            currentWaypoint++;
    }
}
