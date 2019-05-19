using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tu_BigLaser : MonoBehaviour, I_Turret
{

    public int damage;
    public float fireRate;
    public bool canFire;
    public LayerMask layers;
    public string[] tagsTarget;
    public string[] tagsStop;
    private Animator anim;
    private LineRenderer lineRender;
    private Vector2 target;
    private AudioSource fireSound;
    void Start () {
        canFire = true;
        anim = gameObject.GetComponent<Animator>();
        lineRender = gameObject.GetComponent<LineRenderer>();
        fireSound = gameObject.GetComponent<AudioSource>();
    }
	
	void Update () {
        
        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, transform.up, layers);
        if (hits.Length > 0)
        {
                
            lineRender.SetPosition(0, transform.position);
                
            foreach (RaycastHit2D h in hits)
            {
                //collision
                bool blocked = false;
                foreach(string tagStop in tagsStop)
                {
                    if (h.collider.CompareTag(tagStop))
                    {
                        lineRender.SetPosition(1, h.point);
                        blocked = true;
                    }
                }
                if(blocked)
                {
                    break;
                }

                //cible
                foreach(string tagTarget in tagsTarget)
                {
                    if (h.collider.CompareTag(tagTarget))
                    {
                        if (canFire)
                        {
                            GetComponentInParent<I_Mouvement>().SetControl(false);
                            anim.SetBool("Fire", true);
                            StartCoroutine(WaitNextShoot());
                        }
                    }
                }

            }
                
        }

        
    }

    public void EndFire()
    {
        anim.SetBool("Fire", false);
        GetComponentInParent<I_Mouvement>().SetControl(true);
    }
 
    public void SetCanFire(bool state)
    {
        canFire = state;
        if(canFire)
        {
            lineRender.enabled = true;
        }
        else
        {
            lineRender.enabled = false;
        }
    }

    public void Fire()
    {
        fireSound.Play();
        RaycastHit2D hit = Physics2D.CircleCast(transform.position, 2, transform.up, Vector2.Distance(target, transform.position), layers);
        if (hit)
        {
            hit.collider.GetComponent<I_Killable>().TakeDamage(damage);
        }
    }

    public void SetFireRate(float rate)
    {
        this.fireRate = rate;
    }

    public IEnumerator WaitNextShoot()
    {
        canFire = false;
        yield return new WaitForSeconds(1 / fireRate);
        canFire = true;
    }
}
