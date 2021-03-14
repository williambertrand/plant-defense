using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum PlantType
{
    DEFENSE, // Defense plants attack enemies
    POINTS, // Point plants do not attack
    AUGMENT // Augment plants affect the behavior of other plants
}

public enum PlantState
{
    GROWING,
    ACTIVE
}

public class Plant : GridObjectDamageable
{

    public static float GrowRate = 0.15f;
    public static float MaxAge = 25.0f;

    public string plantName;
    public string spriteId;
    public string color; // The hex color to use as a background for the ui element

    /* All plants have: 
     *  a type
     *  age
     *  point multiplier
     * 
     */
    public PlantType plantType;

    public int age;
    public float plantedAt;
    public float pointMultiplier;
    public float basePointRate;
    public float currentPointVal;
    public float newPoints;

    // How much does it cost to plant this little guy
    public int seedCost;
    // How long does this lil fella need to go from a seed to a bloom
    public float growTime;

    public PlantState currentState;

    // Animation related constants
    private Animator animator;
    private float GROW_DUR = 1.1f;
    private float ACTIVE_SPEED = 0.8f;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();

        currentState = PlantState.GROWING;
        animator = GetComponent<Animator>();
        animator.speed = CalculateSpeed();
        StartCoroutine(OnGrowComplete(growTime));
        SpriteRenderer renderer = GetComponent<SpriteRenderer>();
        if(transform.position.y > 0)
        {
            renderer.sortingOrder = -(int)(transform.position.y) * 2;
        }
        else
        {
            renderer.sortingOrder = 6 - (int)transform.position.y;
        }
    }


    protected virtual void Update()
    {
        
        if (currentState == PlantState.GROWING) return;
        float scale = Mathf.Lerp(0.5f, 1.75f, age / Plant.MaxAge);
        transform.localScale = new Vector3(scale, scale, 1.0f);
    }

    public static string TypeToString(PlantType t)
    {
        switch (t)
        {
            case PlantType.DEFENSE:
                return "Defense";
            case PlantType.POINTS:
                return "Point";
            case PlantType.AUGMENT:
                return "Booster";
            default:
                return "Unknown";
        }
    }

    private float CalculateSpeed()
    {
        return GROW_DUR / growTime;
    }


    // Switch a growing plant over to the active state
    IEnumerator OnGrowComplete(float time)
    {
        yield return new WaitForSeconds(time);
        animator.SetTrigger("OnGrowComplete");

        // Weird little quirk where it looks better for the plant to bea tad bit higher than the seedling
        // transform.position = transform.position + new Vector3(0.0f, 0.15f, 0.0f); TODO!

        currentState = PlantState.ACTIVE;
        animator.speed = ACTIVE_SPEED;
        PlantManager.Instance.OnPlantDidActivate(this, gridPositon);
        plantedAt = Time.time;
    }

    public override void OnDeath()
    {
        PlantManager.Instance.OnPlantDeactivate(this);
        base.OnDeath();
        Destroy(gameObject);
    }

    public override string ToString()
    {
        return "Plant: " + name + " at: " + gridPositon.ToString();
    }

}
