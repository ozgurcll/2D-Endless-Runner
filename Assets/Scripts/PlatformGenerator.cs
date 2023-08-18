using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformGenerator : MonoBehaviour
{
    [SerializeField] private Transform[] levelPart;
    [SerializeField] private Vector3 endPoint;

    [SerializeField] private float distanceToSpawn;
    [SerializeField] private float distanceToDelete;
    [SerializeField] private Transform player;



    void Update()
    {
        DeletePlatform();
        GeneratePlatform();
    }

    private void GeneratePlatform()
    {
        while (Vector2.Distance(player.transform.position, endPoint) < distanceToSpawn) // Oyuncu ve EndPoint mesafesi farkı Küçükse distanceToSpawndan
        {
            Transform part = levelPart[Random.Range(0, levelPart.Length)];


            Vector2 newPosition = new Vector2(endPoint.x - part.Find("StartPoint").position.x, 0); // endPoint ile oluşturulan platformun StartPointinin farkı newPositiona aktarılıyor

            Transform newPart = Instantiate(part, newPosition, transform.rotation, transform);

            endPoint = newPart.Find("EndPoint").position; //endPoint yeni oluşturulan Platformun EndPoint öğesinin pozisyon bilgilerini alıyor

        }
    }

    private void DeletePlatform()
    {
        if (transform.childCount > 0)
        {
            Transform partToDelete = transform.GetChild(0);

            if (Vector2.Distance(player.transform.position, partToDelete.transform.position) > distanceToDelete)
                Destroy(partToDelete.gameObject);

        }
    }
}
