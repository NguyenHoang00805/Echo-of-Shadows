using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    Collider2D attackCol;
    public int attackDamage = 10;
    public Vector2 knockback = Vector2.zero;

    private void Awake()
    {
        attackCol = GetComponent<Collider2D>(); 
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //See if hit or not
        Damageable damageable = collision.GetComponent<Damageable>();
        if (damageable != null)
        {
            Vector2 deliveredKnockback = transform.parent.localScale.x > 0 ? knockback : new Vector2(-knockback.x, knockback.y);
            //Hit the target
            bool _gotHit = damageable.Hit(attackDamage, deliveredKnockback);
            if (_gotHit)
            {
                Debug.Log(collision.name +" hit for " + attackDamage);
                PlayerPogo pogo = GetComponentInParent<PlayerPogo>();
                if (pogo != null && pogo.IsDoingDownAttack())
                {
                    pogo.Bounce();
                }
            }
        }
    }

}
