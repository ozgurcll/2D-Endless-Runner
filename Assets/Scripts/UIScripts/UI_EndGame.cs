using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_EndGame : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI distance;
    [SerializeField] private TextMeshProUGUI coins;
    void Start()
    {
        GameManager manager = GameManager.instance;

        Time.timeScale = 0;

        if (manager.distance <= 0)
            return;

        if (manager.coins <= 0)
            return;

        distance.text = "Score: " + manager.distance.ToString("#,#");
        coins.text = "Coins: " + manager.coins.ToString("#,#");
    }
}
