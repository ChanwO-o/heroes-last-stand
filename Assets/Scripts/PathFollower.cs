using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A base class that will move the parent along a path. 
/// Upon instantiation, set its target to the first PathNode
/// of a path
/// </summary>
public class PathFollower : MonoBehaviour
{
    private const int COLLECT_MONEY = 100; // amount of coins collected from killing an enemy
    // It needs a path to follow
    public float speed = 0.1f;
    public int health = 100;
    public int MAX_HEALTH = 100;
    public PathNode target = null;
    public event System.Action onReachedEndEvent;

    public UIManager UIM;
    private LevelManager levelManager;

    // int reachedEndCount = 0;

    // Start is called before the first frame update
    void Start()
    {
        UIM = FindObjectOfType<UIManager>();
        if (!levelManager)
            levelManager = FindObjectOfType<LevelManager>();
        
        // Upon init, move its position to the first path node
        transform.position = target.transform.position;

        onReachedEndEvent += LevelManager.ins.endReached;
    }

    // Update is called once per frame
    void Update()
    {
        if(health <= 0){
            // start dying
            // DeathAnimation()
            Destroy(this.gameObject);

            // get money
            int currentCoin = levelManager.getCoin();
            levelManager.setCoin(currentCoin + COLLECT_MONEY);
            UIM.setTextCoins(currentCoin + COLLECT_MONEY);
        }

        // If we reached the node, target the next one
        if(Vector2.Distance(target.transform.position, transform.position) < 0.3){
            if(target.end == true){
                // We reached the end, do something
                reachedEnd();
            }else{
                target = target.getNextNode();
            }
        }

        

        float step = speed * Time.deltaTime;

        // move sprite towards the target location
        transform.position = Vector2.MoveTowards(
            transform.position,
            target.transform.position, step
        );
    }


    // called when this object has reached the last node
    // Good place to destroy the object, incr. score, etc...
    void reachedEnd() {
        Debug.Log("Reached the end!");

        if (onReachedEndEvent != null)
        {
            onReachedEndEvent();
        }

        Destroy(this.gameObject);
    }

    // Returns a number between 0-1
    // I made this specifically for the healthbar class
    public float getNormHealth(){
        return (float)health/ (float)MAX_HEALTH;
    }

    // Returns the health
    public int getHealth(){
        return health;
    }

    // Use this to deal damage to this enemy
    public void doDamage(float damage){
        health -= (int)damage;
    }
}
