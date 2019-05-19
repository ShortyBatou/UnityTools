using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
 * Vérifie toutes les cibles ( définies via les tags ) autour dans un certain rayon et leurs infliges des dégats
*/ 


public class E_Explosion : MonoBehaviour, I_Event,I_Damage
{
    public bool activated;
    public float range;
    public float damage;
    public string[] tags;

    public void Activate()
    {
        if(activated)
        {
            Collider2D[] cols = Physics2D.OverlapCircleAll(transform.position, range);
            foreach (Collider2D c in cols)
            {
                foreach (string tag in tags)
                {
                    if (c.CompareTag(tag))
                    {
                        c.GetComponent<I_Killable>().TakeDamage(damage);
                    }
                }
            }
        }
       
    }

    public void SetActive(bool state)
    {
        activated = state;
    }

    public void SetDamage(float damage)
    {
        this.damage = damage;
    }
}
