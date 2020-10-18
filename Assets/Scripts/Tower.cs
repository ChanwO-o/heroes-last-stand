using UnityEngine;

public class Tower : MonoBehaviour
{
    private Vector3 shotFrom;
    private void Awake()
    {
        shotFrom = transform.Find("shotFrom").position;
    }
    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Debug.Log("Click");
        }
    }
}
