using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BAR_SIZE {SMALL, MEDIUM, LARGE};

/*
Ideally this class can simply be dragged onto a prefab,
find the right component, and link itself to the required
member function (in this case, health)
*/
public class HealthBar : MonoBehaviour {

    public BAR_SIZE bar_size = BAR_SIZE.MEDIUM;
    private float bar_scale;
    private float y_scale = 0.5f;
    float health_percentage;

    public SpriteRenderer sr;
    PathFollower _pf;

    void Start()
    {
        //_sr = GetComponent<SpriteRenderer>();

        switch (bar_size)
        {
            case BAR_SIZE.SMALL:
             bar_scale = 2f;
                break;
            case BAR_SIZE.MEDIUM:
             bar_scale = 4f;
                break;
            case BAR_SIZE.LARGE:
             bar_scale = 6f;
                break;
        }

        // Get the pathFollower component, so we can
        // use its getHealth() method
        _pf = GetComponent<PathFollower>();
    }


    void Update()
    {
        //healthPercentage = _current_health / maxHealth;                 // gets a float between 0-1
        //x_offset = (float)maxHealth * (1 - healthPercentage) * 0.5f;    // gets the offset needed to align the bar left
        health_percentage = _pf.getNormHealth();
        //Debug.Log("HEALTH: " + health_percentage);

        sr.transform.localScale = new Vector3(health_percentage * bar_scale, y_scale, 1);
    }
}
