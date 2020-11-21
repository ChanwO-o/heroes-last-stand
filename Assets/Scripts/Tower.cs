using UnityEngine;

public class Tower : MonoBehaviour
{
    private float thePower;
    private float theRange;
    private void Awake()
    {
        thePower = 10f;
        theRange = 1f;
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
