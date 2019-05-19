using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
 *  Inflige des dégats à la cible souhaité ( via les tags ) lors d'une collision avec celle-ci
*/
public class D_OnCollision : MonoBehaviour, I_Damage
{
    public bool activated;
    public float damage;
    public string[] tags;
    public bool trueDamage;
    public void SetActive(bool state)
    {
        activated = state;
    }

    public void SetDamage(float damage)
    {
        this.damage = damage;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(activated)
        {
            Collider2D c = collision.collider;
            foreach(string t in tags)
            {
                if(c.CompareTag(t))
                {
                    if (trueDamage)
                    {
                        c.GetComponent<I_Killable>().TakeTrueDamage(damage);
                    }
                    else
                    {
                        c.GetComponent<I_Killable>().TakeDamage(damage);
                    }
                }
            }
        }
    }
}
