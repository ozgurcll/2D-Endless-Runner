using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class UI_InGame : MonoBehaviour
{
    private Player player;
    [SerializeField] private TextMeshProUGUI distanceText;
    [SerializeField] private TextMeshProUGUI coinsText;

    [SerializeField] private Image heartEmpty;
    [SerializeField] private Image heartFull;

    private float distance;
    private float coins;

    private void Start()
    {
        player = GameManager.instance.player;
        InvokeRepeating("UpdateInfo", 0, .2f);
    }

    private void UpdateInfo()
    {
        distance = GameManager.instance.distance;
        coins = GameManager.instance.coins;

        if (distance > 0)
            distanceText.text = GameManager.instance.distance.ToString("#,#") + " M";
        if (coins > 0)
            coinsText.text = GameManager.instance.coins.ToString("#,#");

        heartEmpty.enabled = !player.extraLife;
        heartFull.enabled = player.extraLife;
    }
}
