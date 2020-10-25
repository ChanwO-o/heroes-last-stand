using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathObject : MonoBehaviour
{
    // Hold an array of transforms aka path nodes
    [SerializeField]
    public List<Transform> nodes = new List<Transform>();    
}
