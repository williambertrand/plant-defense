using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class DefensePlant : Plant
{
    public string attackType;
    public float range;
    public float baseRange;
    public float rangeAgeFactor;

    public float attackSpeed;
    public float attackSpeedBase;
    public float attackSpeedAgeFactor;
    public float minAttackSpeed;

    public float lastAttack;
    
    public int attackDamage;
    public float attackDamageAgeFactor;

    public Enemy target;

    public string attackPrefabTag;
    public float projectileSpeed;

    private CircleCollider2D coll;


    protected override void Start()
    {
        base.Start();
        coll = GetComponent<CircleCollider2D>();
        range = baseRange;
        coll.radius = range;
        attackSpeed = attackSpeedBase;
    }

    protected override void Update()
    {
        base.Update();
        if (currentState != PlantState.ACTIVE) return;

        if (Time.time - lastAttack >= attackSpeed)
        {
            if (target != null)
            {
                if(attackType == null || attackType == "ranged")
                {
                    AttackRanged();
                } else
                {
                    AttackMele();
                }
                
            }
        } 
    }

    private void AttackMele()
    {
        lastAttack = Time.time;
        target.TakeDamage(this.attackDamage);
        Vector3 vectorToTarget = target.transform.position - transform.position;
        // Add 90 as our sprite is facing up
        float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg + 90.0f;
        Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
        Vector3 vel = (target.transform.position - this.transform.position).normalized * projectileSpeed;
        GameObject effect = ObjectPooler.Instance.SpawnFromPool(attackPrefabTag, transform.position, q);
        effect.GetComponent<Rigidbody2D>().velocity = vel;
        Effect e = effect.GetComponent<Effect>();
        e.OnSpawn();
    }

    private void AttackRanged()
    {
        lastAttack = Time.time;
        // Start with simple, one type of attack
        Vector3 pos = transform.position;
        Vector3 vel = (target.transform.position - this.transform.position).normalized * projectileSpeed;

        Vector3 vectorToTarget = target.transform.position - transform.position;
        // Add 90 as our sprite is facing up
        float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg + 90.0f;
        Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);

        GameObject projectile = ObjectPooler.Instance.SpawnFromPool(attackPrefabTag, pos, q);
        projectile.GetComponent<Projectile>().damage = attackDamage;
        projectile.GetComponent<Rigidbody2D>().velocity = vel;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {

        if (target != null) return; //Ignore if we already have a target

        GameObject other = collision.gameObject;
        if(other.CompareTag("Enemy"))
        {
            target = other.GetComponent<Enemy>();
        } 
    }

    public override void OnIncreaseAge()
    {
        base.OnIncreaseAge();
        range = baseRange +  (age * rangeAgeFactor);
        coll.radius = range;
        attackDamage = attackDamage + (int)(attackDamageAgeFactor * age);
        attackSpeed = Mathf.Max(attackSpeedBase - (attackSpeedAgeFactor * age), minAttackSpeed);
    }
}
