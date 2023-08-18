using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LedgeDetected : MonoBehaviour
{
    [SerializeField] private float radius;
    [SerializeField] private LayerMask whatIsGround;
    private Player player;
    private bool canDetected;

    public bool ledgeDetected;

    private BoxCollider2D boxCD => GetComponent<BoxCollider2D>();
    private void Awake()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
    }

    private void Update()
    {
        if (canDetected)
            player.ledgeDetected = Physics2D.OverlapCircle(transform.position, radius, whatIsGround);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Ground"))
            canDetected = false;

    }

    private void OnTriggerExit2D(Collider2D other)
    {
        Collider2D[] colliders = Physics2D.OverlapBoxAll(boxCD.bounds.center, boxCD.size, 0);

        foreach (var hit in colliders)
        {
            if (hit.gameObject.GetComponent<PlatformGenerator>() != null)
                return;
        }

        if (other.gameObject.layer == LayerMask.NameToLayer("Ground"))
            canDetected = true;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
