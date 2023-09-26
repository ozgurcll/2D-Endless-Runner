using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Throw : MonoBehaviour
{
    Rigidbody2D rb;

    [SerializeField] private GameObject dedFx;
    [SerializeField] private GameObject brokeFx;

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
        yield return new WaitForSeconds(1f);
        Destroy(this.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<Enemy>() != null)
        {
            AudioManager.instance.PlaySFX(4);
            Instantiate(dedFx, transform.position, Quaternion.identity);
            Destroy(other.gameObject);
            Destroy(gameObject);
        }
        if (other.CompareTag("Object"))
        {
            AudioManager.instance.PlaySFX(5);
            Instantiate(brokeFx, transform.position, Quaternion.identity);
            Destroy(other.gameObject);
            Destroy(gameObject);
        }
    }
}
