using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class B_Basic : MonoBehaviour, I_Bullet {
    private float damage;
    public GameObject destuctionEffect;
    public string targetTag;

    public void OnTriggerEnter2D(Collider2D col)
    {
        if (!col.isTrigger)
        {
            if (col.CompareTag(targetTag))
            {
                I_Killable killable = col.GetComponentInParent<I_Killable>();
                if (!killable.IsInvicible())
                {
                    killable.TakeDamage(damage);
                }

                DestroyBullet();
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
    }

}
