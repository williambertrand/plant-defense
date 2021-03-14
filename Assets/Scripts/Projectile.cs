using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Rigidbody2D))]
public class Projectile : MonoBehaviour
{

    public int damage;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Enemy"))
        {
            Enemy hit = collision.gameObject.GetComponent<Enemy>();
            hit.TakeDamage(damage);
        }
        // This removes the projectile when it hits out board bounds
        gameObject.SetActive(false); // This object is pooled so dont destroy it 
    }
}
