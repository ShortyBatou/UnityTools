using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
 * Lance des projectiles avec une taille, vitesse et dégat aléatoire dans des directions aléatoires  
*/


public class E_ExplosionProjectiles : MonoBehaviour, I_Event
{
    public GameObject projetile;
    public int nbProjectiles;
    public Vector2 sizeInterval;
    public Vector2 speedInterval;
    public Vector2 damageInterval;
    public void Activate()
    {
        
        for(int i =0;i<nbProjectiles;i++)
        {
            Vector3 euleurAngle = new Vector3(0,0,Random.Range(0,360));
            float radomFactor = Random.Range(0f, 1f);
            float randomScale =  sizeInterval.x + (sizeInterval.y -sizeInterval.x) * radomFactor;
            float randomDamage = damageInterval.x + (damageInterval.y - damageInterval.x) * radomFactor;
            float speed = Random.Range(speedInterval.x, speedInterval.y);
            GameObject o = Instantiate(projetile, transform.position, transform.rotation);
            o.transform.eulerAngles = euleurAngle;
            o.transform.localScale = new Vector3(randomScale, randomScale, 0);
            o.GetComponent<I_Mouvement>().SetSpeed(speed);
            o.transform.SetParent(GameObject.FindGameObjectWithTag("Temporary").transform);
            o.GetComponent<I_Bullet>().SetDamage(randomDamage);
        }
    }
}
