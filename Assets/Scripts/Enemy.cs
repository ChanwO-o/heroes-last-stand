using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float moveSpeed;
    private GameController gc;
    private float eX, eY;
    private Vector2 shotFrom;
    public GameObject knife;
    public bool isTurn;
    private int xDirection;
    private void Start()
    {
        isTurn = randomBoolean();
        knife = FindObjectOfType<GameObject>();
        gc = FindObjectOfType<GameController>();
        shotFrom = new Vector2(2.45f, 2.45f);
        xDirection = 4;
    }
    void Update()
    {
        eX = transform.position.x;
        eY = transform.position.y;
        moving();
    }
    public void moving()
    {
        float moveLeft = moveSpeed * -1 * Time.deltaTime;
        float moveRight = moveSpeed * 1 * Time.deltaTime;
        float moveDown = moveSpeed * -1 * Time.deltaTime;
        float moveUp = moveSpeed * 1 * Time.deltaTime;
        switch (xDirection)
        {
            case 1:
                transform.position = transform.position + new Vector3(0, moveUp, 0);
                break;
            case 2:
                transform.position = transform.position + new Vector3(moveRight, 0, 0);
                break;
            case 3:
                transform.position = transform.position + new Vector3(0, moveDown, 0);
                break;
            case 4:
                transform.position = transform.position + new Vector3(moveLeft, 0, 0);
                break;
        }
    }
    public bool randomBoolean()
    {
        return (Random.Range(0, 2) == 1);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Tower"))
        {
            gc.SpawnKnife(shotFrom);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Knife"))
        {
            Destroy(knife);
        }
        if (collision.gameObject.CompareTag("Blocker"))
        {
            if(eY > 0)
            {
                if (xDirection == 3 || xDirection == 1)
                {
                    xDirection = 4;
                }
                else if (xDirection == 4 && eY > 2)
                {
                    xDirection = 3;
                }
                else if (xDirection == 4 && eY < 2)
                {
                    xDirection = 1;
                }
            }
            else
            {
                if (xDirection == 3 || xDirection == 1)
                {
                    xDirection = 4;
                }
                else if (xDirection == 4 && eY >= -2.35)
                {
                    xDirection = 3;
                }
                else if (xDirection == 4 && eY < -2.40 && eX > 1.5)
                {
                    xDirection = 3;
                }
                else if (xDirection == 4 && eY < -2.35 && eX < 1.5)
                {
                    xDirection = 1;
                }
            }
        }
    }
}
