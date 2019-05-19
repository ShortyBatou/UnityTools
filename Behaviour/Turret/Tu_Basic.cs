using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tu_Basic : MonoBehaviour, I_Turret
{
    public bool canFire;
    public GameObject bullet;
    public float fireRate;
    public ParticleSystem fireParticle;
    public AudioSource shootSound;

    public void Fire()
    {
        if(canFire)
        {
            shootSound.Play();
            fireParticle.Play();
            GameObject obj =  Instantiate(bullet, transform.position, transform.rotation);
            canFire = false;
            StartCoroutine(WaitNextShoot());
        }
    }

    public void SetCanFire(bool state)
    {
        canFire = state;
    }

    public void SetFireRate(float rate)
    {
        fireRate = rate;
    }

    public IEnumerator WaitNextShoot()
    {
        yield return new WaitForSeconds(fireRate);
        canFire = true;
    }


}
