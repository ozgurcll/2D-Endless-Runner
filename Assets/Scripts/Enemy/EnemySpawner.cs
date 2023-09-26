using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private Transform respawnPosition;
    [SerializeField] private float changeToSpawn;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<CircleCollider2D>() != null)
        {
            if (Random.Range(0, 100) <= changeToSpawn)
            {
                GameObject newEnemy = Instantiate(enemyPrefab, respawnPosition.position, Quaternion.Euler(0f , 180f, 0f));
                Destroy(newEnemy, 30);
            }
        }
    }
}
