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
    // It needs a path to follow
    public float speed = 0.1f;
    public PathNode target = null;

    // Start is called before the first frame update
    void Start()
    {
        // Upon init, move its position to the first path node
        transform.position = target.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        // If we reached the node, target the next one
        if(Vector2.Distance(target.transform.position, transform.position) < 0.1){
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
    }
}
