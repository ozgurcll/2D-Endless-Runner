using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public UI_Main ui;
    public static GameManager instance;

    public Player player;
    public Enemy enemy;

    [Header("Skybox Material")]
    [SerializeField] private Material[] skyboxMat;

    [Header("Color")]
    public bool colorEntierPlatform;
    public Color platformColor;

    [Header("Score")]
    public int coins;
    public float distance;
    public float score;

    private void Awake()
    {
        instance = this;
        Time.timeScale = 1;
        LoadColor();

        SetupSkyBox(PlayerPrefs.GetInt("SkyBoxSetting"));
    }

    public void SetupSkyBox(int i)
    {
        if (i <= 1)
            RenderSettings.skybox = skyboxMat[i];
        else
            RenderSettings.skybox = skyboxMat[Random.Range(0, skyboxMat.Length)];

        PlayerPrefs.SetInt("SkyBoxSetting", i);
    }

    public void SaveColor(float r, float b, float g)
    {
        PlayerPrefs.SetFloat("ColorR", r);
        PlayerPrefs.SetFloat("ColorG", g);
        PlayerPrefs.SetFloat("ColorB", b);
    }
    private void LoadColor()
    {
        SpriteRenderer sr = player.GetComponent<SpriteRenderer>();

        Color newColor = new Color(PlayerPrefs.GetFloat("ColorR"),
                                   PlayerPrefs.GetFloat("ColorB"),
                                   PlayerPrefs.GetFloat("ColorB"),
                                   PlayerPrefs.GetFloat("ColorA", 1));

        sr.color = newColor;
    }

    private void Update()
    {
        if (player.transform.position.x > distance)
            distance = player.transform.position.x;
    }

    public void UnlockPlayer() => player.playerUnlocked = true;

    public void RestartLevel()
    {
        SceneManager.LoadScene(0);
    }

    public void SaveInfo()
    {
        int savedCoins = PlayerPrefs.GetInt("Coins");

        PlayerPrefs.SetInt("Coins", savedCoins + coins);

        score = distance;

        PlayerPrefs.SetFloat("LastScore", score);

        if (PlayerPrefs.GetFloat("HighScore") < score)
            PlayerPrefs.SetFloat("HighScore", score);
    }

    public void GameEnded()
    {
        SaveInfo();
        ui.OpenEndGame();
    }
}
