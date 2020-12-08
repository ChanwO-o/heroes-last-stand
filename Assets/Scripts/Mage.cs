using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mage : MonoBehaviour
{
    public  Animator animator;

    public float power = 20.0f;
    public float speed = 5.0f;
    public float distThreshold = 4.0f;
    public float attackRangeThreshold = 5.0f; // hero's attack range
    public int direction = 2;
    public bool isAttacking = false;
    private int clock = 0;

    public PathFollower target;
    private CircleCollider2D sightCollider;


    /// <summary>
    /// Used to determine the correct animation to play.
    /// </summary>

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
        clock += 1;
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
            speed = 5.0f; // informs animator to play move animation

            Vector2 heroPos = gameObject.transform.position;
            Vector2 targetPos = target.gameObject.transform.position;
            float distance = Vector2.Distance(heroPos, targetPos);

            if (distance > distThreshold) // not at target; move towards target
            {
                Vector2 directionToTarget = (targetPos - heroPos).normalized;
                UpdateAnimatorDirection(directionToTarget);

                Vector2 deltaDisplacement = Time.deltaTime * speed * directionToTarget;
                transform.Translate(deltaDisplacement, Space.World);
            }

            // Debug.Log("distance: " + distance + " " + attackRangeThreshold);

            if (distance > attackRangeThreshold) // target not in range
            {
                UpdateAnimatorIsAttacking(false);
            }
            else // in range; attack!
            {
                // Debug.Log("TARGET IN RANGE");
                Debug.Log("distance: " + distance + " range: " + attackRangeThreshold);
                UpdateAnimatorIsAttacking(true);
                AttackTarget();
            }
        }
        else // no target set or target is killed
        {
            speed = 0.0f;
            UpdateAnimatorIsAttacking(false);
        }
        UpdateAnimatorSpeed();
    }

    void UpdateAnimatorSpeed()
    {
        animator.SetFloat("Speed", speed);
    }

    void UpdateAnimatorDirection(Vector2 directionVector)
    {
        direction = DirectionVectorToInt(directionVector);
        // Debug.Log("direction: " + direction);
        animator.SetInteger("Direction", direction);
    }

    void UpdateAnimatorIsAttacking(bool attacking)
    {
        isAttacking = attacking;
        animator.SetBool("IsAttacking", attacking);
    }

    void AttackTarget()
    {
        if (clock % 200 == 0) // do damage once every 10th tick
        {
            target.gameObject.GetComponent<PathFollower>().doDamage(power);
        }
    }

    /*
        Converts a velocity to cardinal direction value (NESW)
        North: x is small, y is large +
        South: x is small, y is large -
        East: x is large +, y is small
        West: x is large -, y is small
    */
    int DirectionVectorToInt(Vector2 directionVector)
    {
        float x = directionVector[0];
        float y = directionVector[1];
        
        if (y > 0 && Mathf.Abs(y) >= Mathf.Abs(x)) {
            return 0; // north
        }
        else if (x > 0 && Mathf.Abs(x) > Mathf.Abs(y)) {
            return 1; // east
        }
        else if (y < 0 && Mathf.Abs(y) >= Mathf.Abs(x)) {
            return 2; // south
        }
        else {
            return 3; // west
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
        
    }
}
