using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : MonoBehaviour
{
    public float speed = 1.0f;
    public float distThreshold = 1.0f;


    public PathFollower target;
    private CircleCollider2D sightCollider;


    /// <summary>
    /// Used to determine the correct animation to play.
    /// </summary>
    private enum Direction { UP, DOWN, LEFT, RIGHT }

    // Start is called before the first frame update
    void Start()
    {
        sightCollider = GetComponent<CircleCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckMouseClickForTargetSelection();
        MoveToTarget();
    }

    /// <summary>
    /// If the target is non-null,
    /// and this hero is not with a certain distance of the target,
    /// moves toward the target.
    /// </summary>
    void MoveToTarget()
    {
        if (target != null)
        {
            Vector2 heroPos = gameObject.transform.position;
            Vector2 targetPos = target.gameObject.transform.position;

            if (Vector2.Distance(heroPos, targetPos) > distThreshold)
            {
                Vector2 directionToTarget = (targetPos - heroPos).normalized;

                Vector2 deltaDisplacement = Time.deltaTime * speed * directionToTarget;
                transform.Translate(deltaDisplacement, Space.World);
            }
        }
    }

    /// <summary>
    /// Checks whether the player is selecting an enemy to attack.
    /// If so, lets this hero target the enemy clicked on.
    /// </summary>
    void CheckMouseClickForTargetSelection()
    {
        if (Input.GetMouseButtonDown(0))
        {
            //convert mouse pos to 2D (vector2) World Pos
            // code adapted from https://answers.unity.com/questions/598169/select-2d-object-by-mouse-.html

            Vector2 rayPos = new Vector2(
                Camera.main.ScreenToWorldPoint(Input.mousePosition).x,
                Camera.main.ScreenToWorldPoint(Input.mousePosition).y);

            RaycastHit2D hit = Physics2D.Raycast(rayPos, Vector2.zero, 0f);

            if (hit)
            {
                if (hit.collider.tag.ToLower().Equals("Enemy".ToLower()))
                {
                    Debug.Log("Setting target to: " + hit.transform.gameObject.name);
                    target = hit.transform.gameObject.GetComponent<PathFollower>();
                }
            }
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("detected collision with enemy");
        }
    }
}
