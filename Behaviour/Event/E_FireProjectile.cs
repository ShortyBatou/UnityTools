using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
 * Instancie un projectile et lui donne une vitesse et des dégats ( le projectile avant tout droit )
*/

public class E_FireProjectile : MonoBehaviour, I_Event
{
    public GameObject projectile;
    public float speed;
    public float damage;
    public void Activate()
    {
        GameObject o = Instantiate(projectile, transform.position, transform.rotation);
        o.GetComponent<I_Mouvement>().SetSpeed(speed);
        o.GetComponent<I_Bullet>().SetDamage(damage);
        o.transform.SetParent(GameObject.FindGameObjectWithTag("Temporary").transform);
    }
}
