using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Throw : MonoBehaviour
{
    Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    private void Update()
    {
        Vector2 movement = Vector2.right * 1000 * Time.deltaTime;
        rb.MovePosition(rb.position + movement);

        StartCoroutine(knifeDeleteTime());
    }

    private IEnumerator knifeDeleteTime()
    {
        yield return new WaitForSeconds(3f);
        Destroy(this.gameObject);
    }
}
