using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerBase : MonoBehaviour
{
    public GameObject theTower;
    private Vector3 basePos;
    public UIManager UIM;
    private LevelManager levelManager;

    private const int TOWER_PRICE = 500;

    private void Start()
    {
        UIM = FindObjectOfType<UIManager>();
        if(!theTower)
            theTower = FindObjectOfType<GameObject>();
        if (!levelManager)
            levelManager = FindObjectOfType<LevelManager>();
    }

    private void OnMouseDown()
    {
        int currentCoin = levelManager.getCoin();
        if (theTower && currentCoin >= TOWER_PRICE)
        {
            basePos = transform.position;
            Destroy(gameObject);
            Instantiate(theTower, basePos, Quaternion.identity);
            levelManager.setCoin(currentCoin - TOWER_PRICE);
            UIM.setTextCoins(currentCoin - TOWER_PRICE);
        }
    }
}
