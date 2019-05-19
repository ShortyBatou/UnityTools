using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M_GravityTo : MonoBehaviour, I_Mouvement
{
    public bool control = true;
    public float speed;
    public string targetTag;
    private bool move;
    private GameObject target;
    private Rigidbody2D rb2d;
    private void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (control && move)
        {
            Vector2 direction = target.transform.position - transform.position;
            rb2d.velocity = direction.normalized * (speed / direction.magnitude);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag(targetTag))
        {
            move = true;
            target = collision.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag(targetTag))
        {
            move = false;
        }
    }

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

    public void SetActivate(bool state)
    {
        control = state;
    }

    
    
}
