using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdateHealthUI : MonoBehaviour
{
    private Text healthText;

    public LevelManager levelManager;

    // Start is called before the first frame update
    void Start()
    {
        levelManager.TakeDamage += UpdateUI;
        healthText = GetComponent<Text>();
        Debug.Log("Player health read from level manager is: " + levelManager.player_health);
        UpdateUI();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// Updates the player_health displayed in UI
    /// to the player_health in the levelManager script.
    /// </summary>
    void UpdateUI()
    {
        healthText.text = levelManager.player_health.ToString();
    }
}
