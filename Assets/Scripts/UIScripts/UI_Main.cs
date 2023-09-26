using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UI_Main : MonoBehaviour
{
    private bool gamePaused;
    private bool gameMuted;

    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject endGame;

    // [SerializeField] private TextMeshProUGUI lastScoreText;
    [SerializeField] private TextMeshProUGUI highScoreText;
    [SerializeField] private TextMeshProUGUI coinsText;

    [SerializeField] private UI_VolumeSlider[] slider;
    [SerializeField] private Image muteIcon;

    [SerializeField] private Image inGameMuteIcon;



    private void Start()
    {
        for (int i = 0; i < slider.Length; i++)
        {
            slider[i].SetupSlider();
        }

        SwitchToMenu(mainMenu);

        // lastScoreText.text = PlayerPrefs.GetFloat("LastScore").ToString("#,#") + " M";
        highScoreText.text = PlayerPrefs.GetFloat("HighScore").ToString("#,#") + " M";
    }
    public void SwitchToMenu(GameObject uiMenu)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }

        uiMenu.SetActive(true);

        AudioManager.instance.PlaySFX(3);
        coinsText.text = PlayerPrefs.GetInt("Coins").ToString("#,#");
    }

    public void SwitchSkyBox(int index)
    {
        GameManager.instance.SetupSkyBox(index);
    }
    public void MuteButton()
    {
        gameMuted = !gameMuted;

        if (gameMuted)
        {
            muteIcon.color = new Color(1, 1, 1, .5f);
            AudioListener.volume = 0;
        }
        else
        {
            muteIcon.color = Color.white;
            AudioListener.volume = 1;
        }
    }
    public void StartGame()
    {
        muteIcon = inGameMuteIcon;

        if (gameMuted)
            muteIcon.color = new Color(1, 1, 1, .5f);
        GameManager.instance.UnlockPlayer();
    }

    public void PauseGame()
    {
        if (gamePaused)
        {
            Time.timeScale = 1;
            gamePaused = false;
        }
        else
        {
            Time.timeScale = 0;
            gamePaused = true;
        }
    }

    public void RestartGame() => GameManager.instance.RestartLevel();

    public void OpenEndGame()
    {
        SwitchToMenu(endGame);
    }
}
