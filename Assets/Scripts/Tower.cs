using UnityEngine;

public class Tower : MonoBehaviour
{
    public float thePower;
    public float theRange;
    public float cooldown;
    private float cooldown_tick;

    public PathFollower target = null;
    public GameObject projectilePrefab = null;

    private void Update() {
        
        // Every step, see if we have an enemy within range.
        // Don't look for a new one if the old one already exists
        if(target != null && Vector3.Distance( target.transform.position, transform.position) >= theRange){
            target = null;
        }

        if(target != null){
            // attack the target
            // launch pojectile at the enemy?
            // Or just deal direct damage?

            cooldown_tick += Time.deltaTime;
            if(cooldown_tick > cooldown){
                cooldown_tick = 0;
                GameObject newArrow = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
                //newArrow.transform.LookAt(target.transform);
                newArrow.transform.up = target.transform.position - transform.position;
            }

        }else{
            // look for an enemy within range
            foreach(GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy")){
                Debug.Log("ENEMY");
                if(Vector3.Distance( enemy.transform.position, transform.position) < theRange){
                    target = enemy.GetComponent<PathFollower>();
                    break;
                }
            }
            // NOTE: Potentially, there could be NO enemies withing range
            // at this point, target still == NULL
        }
    }

    private void Awake()
    {
        thePower = 10f;
        theRange = 3f;
        cooldown = 0.6f;
    }


    private void OnMouseDown()
    {
        UpgradeOverlay.showStatic(this);
    }
    public float getPower()
    {
        return thePower;
    }
    public void setPower(float power)
    {
        thePower = power;
    }
    public float getRange()
    {
        return theRange;
    }
    public void setRange(float range)
    {
        theRange = range;
    }
}
