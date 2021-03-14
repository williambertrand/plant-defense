using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class GridObjectDamageable : GridObject
{

    public int health;
    public int maxHealth;

    SpriteRenderer renderer;
    public Color damageColor;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        health = maxHealth;
        renderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            OnDeath();
            Destroy(gameObject);
        } else
        {
            //StartCoroutine(FlashOnDamage());
        }
    }

    public virtual void OnDeath() { }

    IEnumerator FlashOnDamage()
    {
        for (int n = 0; n < 2; n++)
        {
            renderer.color = damageColor;
            yield return new WaitForSeconds(0.1f);
            renderer.color = damageColor;
            yield return new WaitForSeconds(0.1f);
        }
    }
}
