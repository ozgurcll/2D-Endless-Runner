using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnController : MonoBehaviour
{
    [SerializeField] private GameObject enemy;
    private Rigidbody2D rb;
    private int amountOfEnemy;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        amountOfEnemy = Random.Range(3, 7);
        int additionalOffset = amountOfEnemy / 2;

        for (int i = 0; i < amountOfEnemy; i++)
        {
            Vector3 offset = new Vector2(i - additionalOffset, 0);
            Instantiate(enemy, transform.position + offset, Quaternion.identity, transform);
        }

    }

    private void Update()
    {

    }





}
