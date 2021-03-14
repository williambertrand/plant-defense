using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class DefensePlant : Plant
{
    public int range;
    public float attackSpeed;
    public float lastAttack;
    public int attackDamage;

    public Enemy target;

    public string attackPrefabTag;
    public float projectileSpeed;


    protected override void Start()
    {
        base.Start();
        CircleCollider2D coll = GetComponent<CircleCollider2D>();
        coll.radius = range;
    }

    protected override void Update()
    {
        base.Update();
        if (currentState != PlantState.ACTIVE) return;

        if (Time.time - lastAttack >= attackSpeed)
        {
            if (target != null)
            {
                Attack();
            }
        } 
    }


    private void Attack()
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
}
