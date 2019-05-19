using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
 * Avance tout droit avec une vitesse donnée. Si en collision avec un objet alors le "vaisseau" est dévié
*/ 

public class M_Bounce : MonoBehaviour, I_Mouvement {
    public GameObject ship;
    public float speed;
    public bool control;
    private Rigidbody2D rb2d;
    private void Start()
    {
        rb2d = ship.GetComponent<Rigidbody2D>();
        ship.transform.localEulerAngles = new Vector3(0,0,Random.Range(0, 360));
    }

    public void SetControl(bool state)
    {
        control = state;
    }

    public void SetSpeed(float speed)
    {
        this.speed = speed;
    }

    void Update()
    {
        if (control)
        {
            rb2d.velocity = transform.up.normalized * speed;

        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (control)
        {
            // get the point of contact
            ContactPoint2D contact = collision.contacts[0];

            // reflect our old velocity off the contact point's normal vector
            Vector3 reflected = Vector3.Reflect(ship.transform.up, contact.normal);
            ship.transform.up = reflected;
        }
        
    }

    public bool Activated()
    {
        return control;
    }
}
