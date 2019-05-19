using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class B_Pierce : MonoBehaviour, I_Bullet
{
    private float damage;
    public GameObject destuctionEffect;
    public string targetTag;
    private float maxDamage;
    private Vector2 startScale;

    private void Start()
    {
        startScale = transform.localScale;
    }

    public void OnTriggerEnter2D(Collider2D col)
    {
        if (!col.isTrigger)
        {
            if (col.CompareTag(targetTag))
            {
                I_Killable killable = col.GetComponentInParent<I_Killable>();
                if (!killable.IsInvicible())
                {
                    float targetLife = killable.GetLife();
                    killable.TakeDamage(damage);
                    damage -= targetLife;
                    float coef = damage / maxDamage;
                    transform.localScale = startScale * coef;
                    if(damage <=0)
                    {
                        DestroyBullet();
                    }
                }
            }
            else if (col.CompareTag("Environment"))
            {
                DestroyBullet();
            }

        }
    }

    public void DestroyBullet()
    {
        if (destuctionEffect != null)
        {
            GameObject o = Instantiate(destuctionEffect);
            o.transform.position = transform.position;
            Destroy(o, o.GetComponent<ParticleSystem>().main.duration);
        }
        Destroy(gameObject);
    }

    public void SetDamage(float damage)
    {
        this.damage = damage;
        this.maxDamage = damage;
    }
}
