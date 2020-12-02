using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Text textCoins;
    public Text textHealth;
    public Text textDeath;
    public Text textWave;
    public Text textPower;
    public LevelManager levelManager;
    private void Start()
    {
        if (!levelManager)
            levelManager = FindObjectOfType<LevelManager>();
        setTextCoins(levelManager.getCoin());
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
    public void setTextWave(int wave)
    {
        if (textWave)
            textWave.text = wave.ToString();
    }
    public void setTextPower(float power)
    {
        if (textPower)
            textPower.text = power.ToString();
    }
}
