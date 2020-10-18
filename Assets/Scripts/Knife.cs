using UnityEngine;

public class Knife : MonoBehaviour
{
    public float moveSpeed;
    public GameObject enemy;

    // Update is called once per frame
    private void Start()
    {
        enemy = FindObjectOfType<GameObject>();
    }
    void Update()
    {
        float moveX = moveSpeed * 1 * Time.deltaTime;
        float moveY = moveSpeed * -1 * Time.deltaTime;
        transform.position = transform.position + new Vector3(0, moveY, 0);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Destroy(enemy);
        }
    }
}
