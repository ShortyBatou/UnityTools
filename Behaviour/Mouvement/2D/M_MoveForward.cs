using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
 * Avance tout droit 
*/
public class M_MoveForward : MonoBehaviour , I_Mouvement{

    public bool control;
    public float speed;
    private Rigidbody2D rb2d;

    public bool Activated()
    {
        return control;
    }

    public void SetControl(bool state)
    {
        control = state;
    }
    public void SetSpeed(float speed)
    {
        this.speed = speed;
    }


    private void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }
    void Update () {
        if(control)
        {
            rb2d.velocity = transform.up.normalized * speed;
        }
    }
}
