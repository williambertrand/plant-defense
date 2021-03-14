using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum EnemyState
{
    WANDER,
    MOVING, // Used when the enemy has a target aquired
    ATTACKING
}

[RequireComponent(typeof(Rigidbody2D))]
public class Enemy : GridObjectDamageable
{

    public int damage;
    public int damageBase;
    public float attackRange;
    public int maxHealthBase;

    public float moveSpeed;
    public float acceleration;
    public float seedDropCount;

    public Plant target;
    public Vector3 moveDest;
    public EnemyState state;

    public float lastAttack;
    public float attackTime;

    Rigidbody2D rb;

    Animator animator;


    /* Tune how much stronger the enemy gets over time */
    public int levelHealth;
    public float levelAttackFactor;
    public int levelDamage;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        rb = GetComponent<Rigidbody2D>();
        //Start by just giving the enemy a random target plant to attack
        UpdateTarget();
        //state = EnemyState.MOVING;
        animator = GetComponent<Animator>();
        health = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {

        if (moveDest != null && target != null
            && Vector3.Distance(transform.position, moveDest) <= attackRange)
        {
            state = EnemyState.ATTACKING;
            animator.SetTrigger("Attacking");
        } else if(moveDest != null && target == null
            && Vector3.Distance(transform.position, moveDest) <= attackRange)
        {
            UpdateTarget();
        }
        Vector3 desiredVelocity = Vector3.zero;
        /* Hanlde updating position here */
        switch(state)
        {
            case EnemyState.MOVING:
            case EnemyState.WANDER:
                if (moveDest == null) return;
                UpdateRotation();
                desiredVelocity = (moveDest - transform.position).normalized * moveSpeed;
                break;
            case EnemyState.ATTACKING:
                desiredVelocity = Vector3.zero;
                if (target == null) UpdateTarget();
                if (Time.time - lastAttack >= attackTime)
                {
                    Attack();
                }
                break;
            default:
                break;
        }

        // Let enemies slow down faster to attack a plant so they don't overshoot the targeta
        float decelFactor = desiredVelocity == Vector3.zero ? 2.5f : 1;
        rb.velocity = Vector3.MoveTowards(rb.velocity, desiredVelocity, acceleration * decelFactor * Time.deltaTime);

    }

    void Attack()
    {
        if (target == null) return;
        if (Vector2.Distance(transform.position, target.transform.position) > attackRange)
        {
            state = EnemyState.MOVING;
            animator.SetTrigger("Moving");
            return;
        }
        target.TakeDamage(damage);
        lastAttack = Time.time;
    }

    public void UpdateTarget()
    {
        //Get random plant from plant manager
        Plant potentialTarget = PlantManager.Instance.GetRandomPlant();
        if (potentialTarget == null)
        {
            UpdateWanderDest();
            return;
        }
        state = EnemyState.MOVING;
        target = potentialTarget;
        moveDest = target.transform.position;
    }

    private void UpdateWanderDest()
    {
        float x = Random.Range(-GameGrid.Instance.boardWidth, GameGrid.Instance.boardWidth);
        float y = Random.Range(-GameGrid.Instance.boardHeight, GameGrid.Instance.boardHeight);
        Vector3 loc = new Vector3(x, y, 0);
        moveDest = loc;
        state = EnemyState.WANDER;
    }

    public void UpdateTargetTo(Plant targetPlant)
    {
        target = targetPlant;
        moveDest = targetPlant.transform.position;
        state = EnemyState.MOVING;
        animator.SetTrigger("Moving");
    }

    void UpdateRotation()
    {
        Vector2 moveDirection = rb.velocity;
        if (moveDirection != Vector2.zero)
        {
            // Need -90 offset since we start 
            float angle = -90.0f + Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
    }

    public override void OnDeath()
    {
        for(int i = 0; i < seedDropCount; i++)
        {
            float delta = Random.Range(-0.25f, 0.25f);
            Vector3 pos = new Vector3(transform.position.x + delta, transform.position.y + delta, 0);
            Instantiate(SeedSpawnManager.Instance.seedPrefab, pos, Quaternion.identity);
        }
        EnemyManager.Instance.OnEnemyDeath(this);
        base.OnDeath();
    }
}
