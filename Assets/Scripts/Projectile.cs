using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public int damage = 10;
    public float speed = 10.0f;

    // Update is called once per frame
    void Update()
    {
        transform.position += transform.up * speed * Time.deltaTime;
    }

    // This callback is only triggered if one of the colliders also
    // has a Rigidbody2d, set to Dynamic mode (not Kinematic)
    private void OnCollisionEnter2D(Collision2D other) {
        Debug.Log("WE HIT SOMETHING");
        // We only want to do damage to enemies.
        // No friendly fire!
        if(other.gameObject.tag == "Enemy"){
            Debug.Log("We hit an ENEMY");
            other.gameObject.GetComponent<PathFollower>().doDamage(damage);
            Destroy(this.gameObject);
        }
    }
}
