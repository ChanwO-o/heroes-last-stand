using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathNode : MonoBehaviour
{
    public List<PathNode> next = null;
    public bool end;

    // If we have multiple nodes, return random
    public PathNode getNextNode(){
        if(next.Count > 1){
            int i = Random.Range(0, next.Count);
            return next[i];
        }
        else
        {
            return next[0];
        }
    }

    void OnDrawGizmosSelected()
    {
#if UNITY_EDITOR
        if (next[0] != null)
        {
            for(int i = 0; i < next.Count; i++){
                // Draws a blue line from this transform to the target
                Gizmos.color = Color.blue;
                Gizmos.DrawLine(transform.position, next[i].transform.position);
            }

        }
    }
#endif
}
