using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E_ExplosionAngle : MonoBehaviour, I_Event
{
    public GameObject projectile;
    public float damage;
    public float angle;

    public void Activate()
    {
        GameObject o = Instantiate(projectile, transform.position, transform.rotation);
        o.GetComponent<I_Mouvement>().SetSpeed(damage);
        o.GetComponent<I_Bullet>().SetDamage(damage);
        o.transform.SetParent(GameObject.FindGameObjectWithTag("Temporary").transform);
    }
}
