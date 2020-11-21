using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerBase : MonoBehaviour
{

    public static bool GameIsPaused = false;
    public GameObject towerbaseUI;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       if (Input.GetMouseButtonDown(0))
        {
            if(GameIsPaused)
            {
                Resume();
            } else
            {
                Pause();
            }
        }
        
    }

    private void OnMouseDown()
    {
        // print("click works");
        // Replace(this, )

        // if (FloatingTowerMenu)
        // {
        //     ShowFloatingTowerMenu();
        // }

      
    }

     void Resume()
    {
        towerbaseUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
    }

    void Pause()
    {
        towerbaseUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
    }
}
