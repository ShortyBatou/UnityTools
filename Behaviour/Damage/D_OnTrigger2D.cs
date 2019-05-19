using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
 * Inflige des dégats à la cible souhaité ( via les tags ) lorsqu'elle entre dans le trigger
*/
public class D_OnTrigger2D : MonoBehaviour, I_Damage {

    public bool activated;
    public bool trueDamage;
    public float damage;
    public string[] tags;
    public void SetActive(bool state)
    {
        activated = state;
    }

    public void SetDamage(float damage)
    {
        this.damage = damage;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (activated)
        {
            foreach (string t in tags)
            {
                if (col.CompareTag(t))
                {
                    if(trueDamage)
                    {
                        col.GetComponent<I_Killable>().TakeTrueDamage(damage);
                    }
                    else
                    {
                        col.GetComponent<I_Killable>().TakeDamage(damage);
                    }
                    
                }
            }
        }
    }
}
