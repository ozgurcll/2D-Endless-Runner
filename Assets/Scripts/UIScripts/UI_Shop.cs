using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


[Serializable]

public struct ColorTheSell
{
    public Color color;
    public int price;
}

public enum ColorType
{
    playerColor,
    platformColor,
    gunColor
}
public class UI_Shop : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI coinText;
    [SerializeField] private TextMeshProUGUI scoreText;

    [SerializeField] private Text notifyText;

    [Header("Platform")]
    [SerializeField] private GameObject platformColorButton;
    [SerializeField] private Transform platformColorParent;
    [SerializeField] private Image platformDisplay;
    [SerializeField] private ColorTheSell[] platformColors;

    [Header("Player")]
    [SerializeField] private GameObject playerColorButton;
    [SerializeField] private Transform playerColorParent;
    [SerializeField] private Image playerDisplay;
    [SerializeField] private ColorTheSell[] playerColors;

    /* [Header("Gun")]
     [SerializeField] private GameObject gunColorButton;
     [SerializeField] private Transform gunColorParent;
     [SerializeField] private Image gunDisplay;
     [SerializeField] private ColorTheSell[] gunColors;*/

    private void Start()
    {
        scoreText.text = GameManager.instance.score.ToString("#,#");
        coinText.text = PlayerPrefs.GetInt("Coins").ToString("#,#");
        for (int i = 0; i < platformColors.Length; i++)
        {
            Color color = platformColors[i].color;
            int price = platformColors[i].price;


            GameObject newButton = Instantiate(platformColorButton, platformColorParent);
            newButton.transform.GetChild(0).GetComponent<Image>().color = color;
            newButton.transform.GetChild(1).GetComponent<Text>().text = price.ToString("#,#");

            newButton.GetComponent<Button>().onClick.AddListener(() => PurchaseColor(price, color, ColorType.platformColor));
        }

        for (int i = 0; i < playerColors.Length; i++)
        {
            Color color = playerColors[i].color;
            int price = playerColors[i].price;


            GameObject newButton = Instantiate(playerColorButton, playerColorParent);
            newButton.transform.GetChild(0).GetComponent<Image>().color = color;
            newButton.transform.GetChild(1).GetComponent<Text>().text = price.ToString("#,#");

            newButton.GetComponent<Button>().onClick.AddListener(() => PurchaseColor(price, color, ColorType.playerColor));
        }
    }

    public void PurchaseColor(int price, Color color, ColorType colorType)
    {
        AudioManager.instance.PlaySFX(3);

        if (EnoughMoney(price))
        {
            if (colorType == ColorType.platformColor)
            {
                GameManager.instance.platformColor = color;
                platformDisplay.color = color;
            }
            else if (colorType == ColorType.playerColor)
            {
                GameManager.instance.player.GetComponent<SpriteRenderer>().color = color;
                GameManager.instance.SaveColor(color.r, color.g, color.b);
                playerDisplay.color = color;
            }
            StartCoroutine(Notify("Purchased Successful", 1f));
        }
        else
            StartCoroutine(Notify("Not Enough Money", 1f));
    }

    private bool EnoughMoney(int price)
    {
        int myCoins = PlayerPrefs.GetInt("Coins");

        if (myCoins > price)
        {
            int newAmountOfCoins = myCoins - price;
            PlayerPrefs.SetInt("Coins", newAmountOfCoins);
            coinText.text = PlayerPrefs.GetInt("Coins").ToString("#,#");

            return true;
        }
        return false;
    }

    IEnumerator Notify(string text, float seconds)
    {
        notifyText.text = text;

        yield return new WaitForSeconds(seconds);

        notifyText.text = "Click To Buy";
    }
}
