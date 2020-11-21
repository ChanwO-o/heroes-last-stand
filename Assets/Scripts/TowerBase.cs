using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerBase : MonoBehaviour
{
    public GameObject theTower;
    private Vector3 basePos;
    public UIManager UIM;
    private LevelManager levelManager;
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
        if (theTower && currentCoin > 300)
        {
            basePos = transform.position;
            Destroy(gameObject);
            Instantiate(theTower, basePos, Quaternion.identity);
            levelManager.setCoin(currentCoin - 300);
            UIM.setTextCoins(currentCoin - 300);
        }
    }
}
