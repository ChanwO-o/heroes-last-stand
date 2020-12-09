using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public Text textCoins;
    public Text textHealth;
    public Text textDeath;
    public Text textWave;
    public Text textPower;
    public Button StartWave;
    public GameObject pauseMenuUI;
    public TextMeshProUGUI pauseMenuTitle;

    public LevelManager levelManager;

    private void Start()
    {
        if (!levelManager)
            levelManager = FindObjectOfType<LevelManager>();
        setTextCoins(levelManager.getCoin());
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            levelManager.GameIsPaused = !levelManager.GameIsPaused;
        }
    }

    public void setTextCoins(int coins)
    {
        if (textCoins)
            textCoins.text = coins.ToString();
    }
    public void setTextHealth(int health)
    {
        if (textHealth)
            textHealth.text = health.ToString();
    }
    public void setTextDeath(int death)
    {
        if (textDeath)
            textDeath.text = death.ToString();
    }
    public void setTextWave(string wave)
    {
        if (textWave)
            textWave.text = wave;
    }
    public void setTextPower(float power)
    {
        if (textPower)
            textPower.text = power.ToString();
    }
    public void startWaveEnable(bool interactable){
        if(StartWave){
            StartWave.interactable = interactable;
        }
    }
    public void displayPauseMenu(bool isActive, int gameState){
        // 0 = in progress, just a regular pause
        // 1 = Victory!
        // 2 = Defeat :(
        pauseMenuUI.SetActive(isActive);
        
        switch(gameState){
            case 0:
                pauseMenuTitle.text = "Paused";
                pauseMenuTitle.color = Color.white;
                break;
            case 1:
                pauseMenuTitle.text = "Victory!";
                pauseMenuTitle.color = Color.yellow;
                break;
            case 2:
                pauseMenuTitle.text = "Defeat";
                pauseMenuTitle.color = Color.red;
                break;
        }
    }
}
