﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeOverlay : MonoBehaviour
{
    public UpgradeOverlay theOverlay;
    private static UpgradeOverlay Instance { get; set; }
    public UIManager UIM;
    private LevelManager levelManager;
    private Tower currentTower;
    private void Awake()
    {
        Instance = this;

        UIM = FindObjectOfType<UIManager>();
        if (!theOverlay)
            theOverlay = FindObjectOfType<UpgradeOverlay>();
        if (!levelManager)
            levelManager = FindObjectOfType<LevelManager>();

        transform.Find("Power").GetComponent<Button_Sprite>().ClickFunc = increasePower;
        transform.Find("Rada").GetComponent<Button_Sprite>().ClickFunc = increaseRange;
        transform.Find("Cancel").GetComponent<Button_Sprite>().ClickFunc = closePanel;

        Hide();
    }
    public static void showStatic(Tower tower)
    {
        Instance.Show(tower);
        Instance.RefreshRange();
    }
    private void Show(Tower tower)
    {
        gameObject.SetActive(true);
        currentTower = tower;
        UIM.setTextPower(currentTower.getPower());
        transform.position = new Vector3(tower.transform.position.x, tower.transform.position.y, 2);
    }
    private void Hide()
    {
        gameObject.SetActive(false);
    }
    private void increasePower()
    {
        currentTower.setPower(currentTower.getPower() + 10);
        UIM.setTextPower(currentTower.getPower());
        quickReduceCoins(100);
        Debug.Log("Power +10");
    }
    private void increaseRange()
    {
        if(currentTower.getRange() < 2.5f)
        {
            currentTower.setRange(currentTower.getRange() + 0.5f);
            RefreshRange();
            quickReduceCoins(100);
            Debug.Log("Range +10");
        }
    }
    private void closePanel()
    {
        Hide();
    }
    private void quickReduceCoins(int amount)
    {
        int currentCoin = levelManager.getCoin();
        if (currentCoin > 100)
        {
            levelManager.setCoin(currentCoin - 100);
            UIM.setTextCoins(currentCoin - 100);
        }
    }
    private void RefreshRange()
    {
        transform.Find("Range").localScale = Vector3.one * currentTower.getRange() * 1f;
    }
}